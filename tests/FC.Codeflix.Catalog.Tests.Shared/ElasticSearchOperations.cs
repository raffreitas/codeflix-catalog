using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public static class ElasticSearchOperations
{
    public static async Task CreateCategoryIndexAsync(ElasticsearchClient esClient)
    {
        var existsResponse = await esClient.Indices.ExistsAsync(ElasticsearchIndices.Category);
        if (existsResponse.Exists)
        {
            await esClient.Indices.DeleteAsync(ElasticsearchIndices.Category);
        }

        await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Mappings<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t.Id)
                    .Text(t => t.Name, descriptor => descriptor
                        .Fields(x => x.Keyword(k => k.Name!.Suffix("keyword")))
                    )
                    .Text(t => t.Description)
                    .Boolean(b => b.IsActive)
                    .Date(d => d.CreatedAt)
                )
            )
        );
    }

    public static void DeleteCategoryDocuments(ElasticsearchClient elasticClient)
    {
        elasticClient.DeleteByQuery<CategoryModel>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
            .Conflicts(Conflicts.Proceed));
    }

    public static void DeleteCategoryIndex(ElasticsearchClient elasticClient)
    {
        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}