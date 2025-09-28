using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

public sealed class SaveGenreUseCaseTestFixture : GenreUseCaseTestFixture
{
    public SaveGenreInput GetValidInput()
    {
        var genre = DataGenerator.GetValidGenre();
        return new SaveGenreInput(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name))
        );
    }

    public SaveGenreInput GetInalidInput()
    {
        var genre = DataGenerator.GetValidGenre();
        return new(
            genre.Id,
            null!,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name))
        );
    }
}