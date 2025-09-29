using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.GetGenresByIds;

public interface IGetGenreByIdsUseCase : IRequestHandler<GetGenreByIdsInput, IEnumerable<GenreModelOutput>>
{
}