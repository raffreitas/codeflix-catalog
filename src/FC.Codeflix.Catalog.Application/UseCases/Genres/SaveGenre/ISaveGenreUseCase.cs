using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;

public interface ISaveGenreUseCase : IRequestHandler<SaveGenreInput, GenreModelOutput>
{
}