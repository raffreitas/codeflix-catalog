using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Genres.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.SearchGenre;

public sealed class SearchGenreTestFixture : GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(IEnumerable<string> genreNames)
        => DataGenerator.GetGenreModelList(genreNames);

    public IList<GenreModel> CloneGenresListOrdered(IList<GenreModel> genresList, string orderBy, SearchOrder direction)
    {
        var listClone = new List<GenreModel>(genresList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}

[CollectionDefinition(nameof(SearchGenreTestFixture))]
public class SearchGenreTestFixtureCollection : ICollectionFixture<SearchGenreTestFixture>
{
}