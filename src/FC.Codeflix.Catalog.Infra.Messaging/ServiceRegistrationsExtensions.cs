using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;
using FC.Codeflix.Catalog.Infra.Messaging.Extensions;
using FC.Codeflix.Catalog.Infra.Messaging.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationsExtensions
{
    private const string KafkaConfigurationSection = nameof(KafkaConfiguration);
    private const string CategoryConsumerConfigurationSection = "CategoryConsumer";

    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration(KafkaConfigurationSection);

        var kafkaConfiguration = configuration.GetSection(KafkaConfigurationSection);

        return services
            .AddScoped<SaveCategoryMessageHandler>()
            .AddScoped<DeleteCategoryMessageHandler>()
            .AddKafkaConsumer<CategoryPayloadModel>()
            .WithRetries(3)
            .Configure(kafkaConfiguration.GetSection(CategoryConsumerConfigurationSection))
            .With<SaveCategoryMessageHandler>()
            .When(message => message.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteCategoryMessageHandler>()
            .When(message => message.Operation is MessageModelOperation.Delete)
            .Register();
    }
}