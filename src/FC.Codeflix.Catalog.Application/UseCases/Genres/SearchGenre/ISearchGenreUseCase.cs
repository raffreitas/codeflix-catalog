using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.SearchGenre;

public interface ISearchGenreUseCase : IRequestHandler<SearchGenreInput, SearchListOutput<GenreModelOutput>>
{
}