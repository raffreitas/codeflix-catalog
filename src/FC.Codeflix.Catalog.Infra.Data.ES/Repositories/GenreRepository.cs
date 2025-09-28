using Elastic.Clients.Elasticsearch;

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

    public Task<SearchOutput<Genre>> SearchAsync(SearchInput input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}