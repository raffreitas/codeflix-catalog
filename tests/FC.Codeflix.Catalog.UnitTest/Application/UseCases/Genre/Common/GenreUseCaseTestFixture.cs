using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

public class GenreUseCaseTestFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();

    public IGenreRepository GetMockRepository() => Substitute.For<IGenreRepository>();

    public Catalog.Domain.Entities.Genre GetValidGenre() => DataGenerator.GetValidGenre();

    public List<Catalog.Domain.Entities.Genre> GetGenreList(int length = 10)
        => DataGenerator.GetGenreList(length);
}

[CollectionDefinition(nameof(GenreUseCaseTestFixture))]
public sealed class GenreUseCateTestFixtureCollection : ICollectionFixture<GenreUseCaseTestFixture>
{
}