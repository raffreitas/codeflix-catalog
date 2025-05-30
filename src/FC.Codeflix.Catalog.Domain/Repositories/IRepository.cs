﻿using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task SaveAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SearchOutput<T>> SearchAsync(SearchInput input, CancellationToken cancellationToken = default);
}
