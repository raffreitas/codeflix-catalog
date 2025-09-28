using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;

public sealed class SaveGenreUseCase(IGenreRepository genreRepository) : ISaveGenreUseCase
{
    public async Task<GenreModelOutput> Handle(SaveGenreInput request, CancellationToken cancellationToken)
    {
        var genre = new Genre(
            request.Id,
            request.Name,
            request.IsActive,
            request.CreatedAt,
            request.Categories.Select(item => new Category(item.Id, item.Name))
        );

        await genreRepository.SaveAsync(genre, cancellationToken);

        return GenreModelOutput.FromGenre(genre);
    }
}