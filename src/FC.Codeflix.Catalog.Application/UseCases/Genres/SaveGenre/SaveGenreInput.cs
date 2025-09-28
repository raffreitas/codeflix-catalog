using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;

public sealed record SaveGenreInput(
    Guid Id,
    string Name,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<SaveGenreInputCategory> Categories
) : IRequest<GenreModelOutput>;

public sealed record SaveGenreInputCategory(Guid Id, string Name);