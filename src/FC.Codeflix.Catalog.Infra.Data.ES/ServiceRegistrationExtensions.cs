using Elastic.Clients.Elasticsearch;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Data.ES;
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddElasticSearch(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("ElasticSearch")
            ?? throw new InvalidOperationException("ElasticSearch Connection string not found.");

        var uri = new Uri(connectionString);
        var clientSettings = new ElasticsearchClientSettings(uri)
            .PrettyJson()
            .ThrowExceptions()
            .RequestTimeout(TimeSpan.FromMinutes(2));

        var client = new ElasticsearchClient(clientSettings);
        services.AddSingleton(client);
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }
}
