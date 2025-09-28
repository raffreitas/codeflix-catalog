using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Genres;

public sealed class GenreTestFixture
{
    private readonly GenreDataGenerator _dataGenerator = new();

    public Genre GetValidGenre() => _dataGenerator.GetValidGenre();
}