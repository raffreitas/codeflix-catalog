using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.IntegrationTests.Genres.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.SaveGenre;

public sealed class SaveGenreTestFixture : GenreTestFixture
{
    public SaveGenreInput GetValidInput() => DataGenerator.GetValidSaveGenreInput();

    public SaveGenreInput GetInvalidInput() => DataGenerator.GetInvalidSaveGenreInput();
}

[CollectionDefinition(nameof(SaveGenreTestFixture))]
public class SaveGenreTestFixtureCollection : ICollectionFixture<SaveGenreTestFixture>
{
}