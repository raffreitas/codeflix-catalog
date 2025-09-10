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
    {
        var baseDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Enumerable.Range(0, length)
            .Select(index =>
            {
                var createdAt = baseDateTime.AddDays(index);
                var id = new Guid($"00000000-0000-0000-0001-{index:D12}");
                var category = new Category(
                    id,
                    $"Category {index:D3}",
                    $"Description for category {index}",
                    createdAt,
                    index % 2 == 0);
                return CategoryModel.FromEntity(category);
            }).ToList();
    }


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