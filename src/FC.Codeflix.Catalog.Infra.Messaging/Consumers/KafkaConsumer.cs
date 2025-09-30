using System.Text.Json;

using Confluent.Kafka;
using Confluent.Kafka.Admin;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers;

public sealed class KafkaConsumer<TMessage>(
    ILogger<KafkaConsumer<TMessage>> logger,
    KafkaConsumerConfiguration configuration,
    IServiceProvider serviceProvider
) : BackgroundService where TMessage : class
{
    private readonly List<(Predicate<MessageModelPayload<TMessage>> Predicate, Type Type)> _messageHandlers = [];

    private readonly IProducer<string, string> _producer = new ProducerBuilder<string, string>(new ProducerConfig
    {
        BootstrapServers = configuration.BootstrapServers, Acks = Acks.None
    }).Build();

    private ConsumerConfig GetConsumerConfig() => new()
    {
        BootstrapServers = configuration.BootstrapServers,
        GroupId = configuration.GroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = true,
        EnableAutoOffsetStore = false,
    };


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureTopicsAreCreatedAsync();
        var config = GetConsumerConfig();
        var topic = configuration.Topic;
        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumerResult = await Task.Run(() =>
                consumer.Consume((int)TimeSpan.FromSeconds(30).TotalMilliseconds), stoppingToken);

            if (consumerResult is null || consumerResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
                continue;

            if (configuration.ConsumeDelaySeconds > 0)
                await Task.Delay(configuration.ConsumeDelaySeconds * 1_000, stoppingToken);

            await HandleMessageAsync(consumerResult.Message, stoppingToken);
            consumer.StoreOffset(consumerResult);
        }

        consumer.Close();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _producer.Dispose();
        return base.StopAsync(cancellationToken);
    }

    private async Task HandleMessageAsync(Message<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var messageModel = JsonSerializer.Deserialize<MessageModelPayload<TMessage>>(
                message.Value, SerializerConfiguration.JsonSerializerOptions);

            var messageHandlerType = _messageHandlers
                .FirstOrDefault(tuple => tuple.Predicate.Invoke(messageModel!))
                .Type;

            if (messageHandlerType is null)
            {
                logger.LogError("None of the message handlers predicates was satisfied by the message: {@Message}",
                    message);
                return;
            }

            var handler = (IMessageHandler<TMessage>?)scope.ServiceProvider.GetRequiredService(messageHandlerType);

            if (handler is null)
            {
                logger.LogError("No message handle found of type: {MessageHandlerType}", messageHandlerType);
                return;
            }

            await handler.HandleMessageAsync(messageModel!, cancellationToken);
        }
        catch (Exception ex)
        {
            await OnErrorAsync(ex, message, cancellationToken);
        }
    }

    private async Task OnErrorAsync(Exception exception, Message<string, string> message,
        CancellationToken cancellationToken)
    {
        if (exception is JsonException or BusinessRuleException)
        {
            logger.LogError(exception, "Non-retryable error. Sending to DLQ. {Error}", exception.Message);
            await _producer.ProduceAsync(configuration.DlqTopic, message, cancellationToken);
            return;
        }

        string topic = configuration.HasRetry
            ? configuration.RetryTopic!
            : configuration.DlqTopic!;

        logger.LogError(exception, "Retryable error. Sending to {Topic}. Error: {Error}", topic, exception.Message);
        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public void AddMessageHandler<T>(Predicate<MessageModelPayload<TMessage>> predicate)
        => _messageHandlers.Add((predicate, typeof(T)));

    public void AddMessageHandler(Type type, Predicate<MessageModelPayload<TMessage>> predicate)
        => _messageHandlers.Add((predicate, type));

    private async Task EnsureTopicsAreCreatedAsync()
    {
        var kafkaAdminConfig = new AdminClientConfig { BootstrapServers = configuration.BootstrapServers };
        var topics = new[] { configuration.Topic, configuration.DlqTopic };
        using var kafkaAdmin = new AdminClientBuilder(kafkaAdminConfig).Build();
        try
        {
            var topicsSpecification = topics.Select(topicName => new TopicSpecification
            {
                Name = topicName, NumPartitions = 1, ReplicationFactor = 1
            });

            await kafkaAdmin.CreateTopicsAsync(topicsSpecification);
        }
        catch (CreateTopicsException ex)
            when (ex.Message.Contains("already exists"))
        {
            logger.LogWarning(ex, "Topic already exists: {Error}", ex.Message);
        }
    }
}