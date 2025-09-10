using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.Common;

public class CategoryTestFixture : BaseFixture, IDisposable
{
    public CategoryDataGenerator DataGenerator { get; } = new();
    public readonly ElasticsearchClient ElasticClient;

    public CategoryTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();
        ElasticSearchOperations.CreateCategoryIndexAsync(ElasticClient).GetAwaiter().GetResult();
    }

    public Category GetValidCategory() => DataGenerator.GetValidCategory();

    public IList<CategoryModel> GetCategoryModelList(int length = 10)
        => DataGenerator.GetCategoryModelList(length);


    public void DeleteAll()
    {
        ElasticSearchOperations.DeleteCategoryDocuments(ElasticClient);
    }

    public void Dispose()
    {
        ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{
}