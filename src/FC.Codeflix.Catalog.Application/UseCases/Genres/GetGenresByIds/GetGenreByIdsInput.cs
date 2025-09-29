using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.GetGenresByIds;

public sealed record GetGenreByIdsInput(
    IEnumerable<Guid> Ids
) : IRequest<IEnumerable<GenreModelOutput>>;