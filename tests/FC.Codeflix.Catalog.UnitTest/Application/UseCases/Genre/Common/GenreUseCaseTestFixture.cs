using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

public class GenreUseCaseTestFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();

    public IGenreRepository GetMockRepository() => Substitute.For<IGenreRepository>();

    public Catalog.Domain.Entities.Genre GetValidGenre() => DataGenerator.GetValidGenre();
}