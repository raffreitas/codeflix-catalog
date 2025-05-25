using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Repositories;
public class CategoryRepository(ElasticsearchClient client) : ICategoryRepository
{
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Category entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Category>> SearchAsync(SearchInput input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
