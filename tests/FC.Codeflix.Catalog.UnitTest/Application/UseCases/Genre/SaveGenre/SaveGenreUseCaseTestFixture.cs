using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

public sealed class SaveGenreUseCaseTestFixture : GenreUseCaseTestFixture
{
    public SaveGenreInput GetValidInput() => DataGenerator.GetValidSaveGenreInput();

    public SaveGenreInput GetInvalidInput() => DataGenerator.GetInvalidSaveGenreInput();
}