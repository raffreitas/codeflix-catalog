using FC.Codeflix.Catalog.Infra.Messaging.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging.Extensions;

internal static class ServiceCollectionExtensions
{
    public static KafkaConsumerBuilder<TMessage> AddKafkaConsumer<TMessage>(this IServiceCollection services)
        where TMessage : class
    {
        return new KafkaConsumerBuilder<TMessage>(services);
    }
}