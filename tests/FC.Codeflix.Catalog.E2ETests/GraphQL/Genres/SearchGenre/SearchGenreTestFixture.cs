using FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using RepositoriesDto = FC.Codeflix.Catalog.Domain.Repositories.DTOs;


namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.SearchGenre;

public sealed class SearchGenreTestFixture : GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(IEnumerable<string> genreName) =>
        DataGenerator.GetGenreModelList(genreName).ToList();

    public IList<GenreModel> CloneGenresListOrdered(
        List<GenreModel> examples,
        string orderBy,
        RepositoriesDto.SearchOrder direction)
        => DataGenerator.CloneGenresListOrdered(examples, orderBy, direction);
}

[CollectionDefinition(nameof(SearchGenreTestFixture))]
public sealed class SearchGenreTestFixtureCollection : ICollectionFixture<SearchGenreTestFixture>
{
}