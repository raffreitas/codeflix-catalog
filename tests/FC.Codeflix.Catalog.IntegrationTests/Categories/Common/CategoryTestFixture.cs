using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.Common;

public class CategoryTestFixture : BaseFixture, IDisposable
{
    public CategoryTestFixture() : base()
    {
        CreateCategoryIndex().GetAwaiter().GetResult();
    }

    public async Task CreateCategoryIndex()
    {
        var esClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();
        await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Mappings<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t.Id)
                    .Text(t => t.Name, ps => ps
                        .Fields(x => x.Keyword(k => k.Name!.Suffix("keywork")))
                    )
                    .Text(t => t.Description)
                    .Boolean(b => b.IsActive)
                    .Date(d => d.CreatedAt)
                )
            )
        );
    }

    public string GetValidCategoryName()
    {
        var categoryName = string.Empty;
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        return categoryDescription;
    }

    public Category GetValidCategory()
        => new(
            Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean());

    public void DeleteAll()
    {
        var esClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();
        esClient.DeleteByQuery<CategoryModel>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed));
    }

    public void Dispose()
    {
        var esClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();
        esClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{
}