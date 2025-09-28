using System.Text.Json;

using Confluent.Kafka;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers;

public sealed class CategoryConsumer(
    ILogger<CategoryConsumer> logger,
    IOptions<KafkaConfiguration> options,
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    private readonly KafkaConfiguration _configuration = options.Value;

    private ConsumerConfig GetConsumerConfig() => new()
    {
        BootstrapServers = _configuration.BootstrapServers,
        GroupId = _configuration.CategoryConsumer.GroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = true,
        EnableAutoOffsetStore = false,
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = GetConsumerConfig();
        var topic = _configuration.CategoryConsumer.Topic;
        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumerResult = await Task.Run(() =>
                consumer.Consume((int)TimeSpan.FromSeconds(30).TotalMilliseconds), stoppingToken);

            if (consumerResult is null || consumerResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
                continue;

            await HandleMessageAsync(consumerResult.Message, stoppingToken);
            consumer.StoreOffset(consumerResult);
        }

        consumer.Close();
    }

    private async Task HandleMessageAsync(Message<string, string> message, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messageModel = JsonSerializer.Deserialize<MessageModelPayload<CategoryPayloadModel>>(
            message.Value, SerializerConfiguration.JsonSerializerOptions);

        switch (messageModel!.Operation)
        {
            case MessageModelOperation.Create:
            case MessageModelOperation.Read:
            case MessageModelOperation.Update:
                var saveInput = messageModel.After!.ToSaveCategory();
                await mediator.Send(saveInput, cancellationToken);
                break;
            case MessageModelOperation.Delete:
                try
                {
                    var deleteInput = messageModel.Before!.ToDeleteCategory();
                    await mediator.Send(deleteInput, cancellationToken);
                }
                catch (NotFoundException ex)
                {
                    logger.LogError(ex, "Category with Id: {CategoryId} was not found. Message: {Message}",
                        messageModel.Before!.Id, message.Value);
                }

                break;
            default:
                logger.LogError("Invalid Operation: {Operation}", messageModel.Op);
                break;
        }
    }
}