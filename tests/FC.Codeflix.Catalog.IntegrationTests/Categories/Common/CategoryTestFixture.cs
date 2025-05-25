using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Infra.Data.ES;
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
        var response = await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Mappings<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix("keyword")))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
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
            .Query(q => q.QueryString(qs => qs.Query("*"))));
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
