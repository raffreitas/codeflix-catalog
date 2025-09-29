using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Domain.Repositories;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IReadOnlyList<Genre>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}