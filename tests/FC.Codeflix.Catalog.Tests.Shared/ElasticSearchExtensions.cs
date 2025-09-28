using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public static class ElasticSearchExtensions
{
    public static async Task CreateGenreIndexAsync(this ElasticsearchClient esClient)
    {
        await DeleteIndexAsync(esClient, ElasticsearchIndices.Genre);
        await esClient.Indices.CreateAsync(ElasticsearchIndices.Genre, c => c
            .Mappings<GenreModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t.Id)
                    .Text(t => t.Name, descriptor => descriptor
                        .Fields(x => x.Keyword(k => k.Name.Suffix("keyword")))
                    )
                    .Boolean(b => b.IsActive)
                    .Date(d => d.CreatedAt)
                    .Nested(n => n.Categories, n => n
                        .Properties(np => np
                            .Keyword(t => t.Id)
                            .Text(t => t.Name, descriptor => descriptor
                                .Fields(x => x.Keyword(k => k.Name.Suffix("keyword")))
                            )
                        )
                    )
                )
            )
        );
    }

    public static void DeleteGenreIndex(this ElasticsearchClient elasticClient)
        => elasticClient.Indices.Delete(ElasticsearchIndices.Genre);

    public static async Task CreateCategoryIndexAsync(this ElasticsearchClient esClient)
    {
        await DeleteIndexAsync(esClient, ElasticsearchIndices.Category);
        await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Mappings<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t.Id)
                    .Text(t => t.Name, descriptor => descriptor
                        .Fields(x => x.Keyword(k => k.Name.Suffix("keyword")))
                    )
                    .Text(t => t.Description)
                    .Boolean(b => b.IsActive)
                    .Date(d => d.CreatedAt)
                )
            )
        );
    }

    public static void DeleteCategoryIndex(this ElasticsearchClient elasticClient)
        => elasticClient.Indices.Delete(ElasticsearchIndices.Category);

    public static void DeleteDocuments<T>(this ElasticsearchClient elasticClient) where T : class
        => elasticClient.DeleteByQuery<T>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed));

    private static async Task DeleteIndexAsync(ElasticsearchClient elasticClient, string indexName)
    {
        var existsResponse = await elasticClient.Indices.ExistsAsync(indexName);
        if (existsResponse.Exists)
            await elasticClient.Indices.DeleteAsync(indexName);
    }
}