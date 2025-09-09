using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;

public class CategoryRepository(ElasticsearchClient client) : ICategoryRepository
{
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await client.DeleteAsync<CategoryModel>(id, cancellationToken);
        if (response.Result == Result.NotFound)
            throw new NotFoundException($"Category '{id}' not found.");
    }

    public async Task SaveAsync(Category entity, CancellationToken cancellationToken = default)
    {
        var model = CategoryModel.FromEntity(entity);
        await client.IndexAsync(model, cancellationToken);
    }

    /**
     * {
     *     "query": {
     *          "match": {
     *              "name": {
     *                  "query": "action"
     *              }
     *          }
     *      },
     *      "from": 20,
     *      "size": 10,
     *      "sort": [
     *          { "name.keyword": "asc"},
     *          { "id": "asc"},
     *      ]
     *  }
     */
    public async Task<SearchOutput<Category>> SearchAsync(SearchInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await client.SearchAsync<CategoryModel>(s => s
                .Query(q =>
                {
                    if (string.IsNullOrWhiteSpace(input.Search))
                        q.MatchAll();
                    else
                        q.Match(m => m
                            .Field(f => f.Name)
                            .Query(input.Search)
                        );
                })
                .From(input.From)
                .Size(input.PerPage)
                .Sort(BuildSortExpression(input.OrderBy, input.Order))
            , cancellationToken);

        var categories = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Category>(input.Page, input.PerPage, (int)response.Total, categories);
    }

    private static Action<SortOptionsDescriptor<CategoryModel>> BuildSortExpression(
        string orderBy,
        SearchOrder order
    ) => (orderBy.ToLower(), order) switch
    {
        ("name", SearchOrder.Asc) => s =>
            s.Field(f => f.Name.Suffix("keyword"), SortOrder.Asc)
                .Field(f => f.Id, SortOrder.Asc),
        ("name", SearchOrder.Desc) => s =>
            s.Field(f => f.Name.Suffix("keyword"), SortOrder.Desc).Field(f => f.Id, SortOrder.Desc),
        ("id", SearchOrder.Asc) => s => s.Field(f => f.Id, SortOrder.Asc),
        ("id", SearchOrder.Desc) => s => s.Field(f => f.Id, SortOrder.Desc),
        ("createdat", SearchOrder.Asc) => s => s.Field(f => f.CreatedAt, SortOrder.Asc),
        ("createdat", SearchOrder.Desc) => s => s.Field(f => f.CreatedAt, SortOrder.Desc),
        _ => s => s.Field(f => f.Name.Suffix("keyword"), SortOrder.Asc).Field(f => f.Id, SortOrder.Asc)
    };
}