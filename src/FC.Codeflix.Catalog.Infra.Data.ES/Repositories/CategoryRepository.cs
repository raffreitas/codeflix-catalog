using Elastic.Clients.Elasticsearch;

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

    public Task<SearchOutput<Category>> SearchAsync(SearchInput input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}