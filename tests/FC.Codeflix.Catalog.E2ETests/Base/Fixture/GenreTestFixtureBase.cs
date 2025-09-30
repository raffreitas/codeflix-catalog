using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Api;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class GenreTestFixtureBase : IDisposable
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; }
    public ElasticsearchClient ElasticClient { get; }
    public GenreDataGenerator DataGenerator { get; } = new();

    protected GenreTestFixtureBase()
    {
        WebApplicationFactory = new CustomWebApplicationFactory<Program>();
        _ = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(CustomWebApplicationFactory<Program>.BaseUrl)
        });
        ElasticClient = WebApplicationFactory.Services.GetRequiredService<ElasticsearchClient>();
        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult();
    }

    public List<GenreModel> GetGenreModelList(int count = 10) => DataGenerator.GetGenreModelList(count).ToList();

    public void DeleteAll() => ElasticClient.DeleteDocuments<GenreModel>();

    public void Dispose() => ElasticClient.DeleteGenreIndex();
}