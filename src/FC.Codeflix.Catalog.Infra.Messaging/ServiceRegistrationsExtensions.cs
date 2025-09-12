using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationsExtensions
{
    public static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration(nameof(KafkaConfiguration));
        services.AddHostedService<CategoryConsumer>();

        return services;
    }
}