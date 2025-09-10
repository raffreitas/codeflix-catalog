using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

public class CategoryTestFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> WebApplicationFactory { get; }
    public CatalogClient GraphQlClient { get; }
    public ElasticsearchClient ElasticClient { get; }
    public CategoryDataGenerator DataGenerator { get; } = new();

    public CategoryTestFixture()
    {
        WebApplicationFactory = new CustomWebApplicationFactory<Program>();
        _ = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(CustomWebApplicationFactory<Program>.BaseUrl)
        });
        ElasticClient = WebApplicationFactory.Services.GetRequiredService<ElasticsearchClient>();
        GraphQlClient = WebApplicationFactory.Services.GetRequiredService<CatalogClient>();
        ElasticSearchOperations.CreateCategoryIndexAsync(ElasticClient).GetAwaiter().GetResult();
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
    {
        ElasticSearchOperations.DeleteCategoryDocuments(ElasticClient);
    }

    public void Dispose() => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
}