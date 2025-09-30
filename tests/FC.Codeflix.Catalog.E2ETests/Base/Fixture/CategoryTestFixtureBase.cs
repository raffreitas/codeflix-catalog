using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Api;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.Base.Fixture;

public class CategoryTestFixtureBase : IDisposable
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; }
    public ElasticsearchClient ElasticClient { get; }
    public CategoryDataGenerator DataGenerator { get; } = new();

    protected CategoryTestFixtureBase()
    {
        WebApplicationFactory = new CustomWebApplicationFactory<Program>();
        _ = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(CustomWebApplicationFactory<Program>.BaseUrl)
        });
        ElasticClient = WebApplicationFactory.Services.GetRequiredService<ElasticsearchClient>();
        ElasticClient.CreateCategoryIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10) => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll() => ElasticClient.DeleteDocuments<CategoryModel>();

    public void Dispose() => ElasticClient.DeleteCategoryIndex();
}