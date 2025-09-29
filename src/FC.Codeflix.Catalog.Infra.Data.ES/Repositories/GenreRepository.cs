using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;

public sealed class GenreRepository(ElasticsearchClient client) : IGenreRepository
{
    public async Task SaveAsync(Genre entity, CancellationToken cancellationToken = default)
    {
        var model = GenreModel.FromEntity(entity);
        await client.IndexAsync(model, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await client.DeleteAsync<GenreModel>(id, cancellationToken);
        if (response.Result == Result.NotFound)
            throw new NotFoundException($"Genre '{id}' not found.");
    }

    public async Task<SearchOutput<Genre>> SearchAsync(SearchInput input, CancellationToken cancellationToken = default)
    {
        var response = await client.SearchAsync<GenreModel>(s => s
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

        var genres = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Genre>(input.Page, input.PerPage, (int)response.Total, genres);
    }

    public async Task<IReadOnlyList<Genre>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var response = await client.SearchAsync<GenreModel>(s => s
            .Query(q => q
                .Bool(b => b
                    .Filter(f => f
                        .Ids(i => i.Values(ids.Select(id => id.ToString()).ToArray()))
                    )
                )
            ), cancellationToken);

        return response.Documents.Select(doc => doc.ToEntity()).ToList();
    }

    private static Action<SortOptionsDescriptor<GenreModel>> BuildSortExpression(string orderBy, SearchOrder order)
        => (orderBy.ToLower(), order) switch
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