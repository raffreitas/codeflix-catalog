using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public sealed class GenreDataGenerator : DataGeneratorBase
{
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    public string GetValidName() => Faker.Commerce.Categories(1)[0];

    public Genre GetValidGenre()
    {
        var categories = new[] { _categoryDataGenerator.GetValidCategory(), _categoryDataGenerator.GetValidCategory() };
        var genre = new Genre(
            Guid.NewGuid(),
            GetValidName(),
            GetRandomBoolean(),
            DateTime.Now,
            categories
        );

        return genre;
    }

    public IEnumerable<GenreModel> GetGenreModelList(int count = 10)
    {
        var baseDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Enumerable.Range(0, count)
            .Select(index =>
            {
                var createdAt = baseDateTime.AddDays(index);
                var id = new Guid($"00000000-0000-0000-0002-{index:D12}");
                var categories = new[]
                {
                    _categoryDataGenerator.GetValidCategory(), _categoryDataGenerator.GetValidCategory()
                };
                var genre = new Genre(
                    id,
                    $"Genre {index:D3}",
                    index % 2 == 0,
                    createdAt,
                    categories
                );
                return GenreModel.FromEntity(genre);
            }).ToList();
    }

    public IList<GenreModel> GetGenreModelList(IEnumerable<string> genreNames)
    {
        var baseDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return genreNames
            .Select((name, index) =>
            {
                var createdAt = baseDateTime.AddDays(index);
                var id = new Guid($"00000000-0000-0000-0002-{index:D12}");
                var categories = new[]
                {
                    _categoryDataGenerator.GetValidCategory(), _categoryDataGenerator.GetValidCategory()
                };
                var genre = new Genre(
                    id,
                    name,
                    index % 2 == 0,
                    createdAt,
                    categories
                );
                return GenreModel.FromEntity(genre);
            }).ToList();
    }

    public SaveGenreInput GetValidSaveGenreInput()
    {
        var genre = GetValidGenre();
        return new SaveGenreInput(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name))
        );
    }

    public SaveGenreInput GetInvalidSaveGenreInput()
    {
        var genre = GetValidGenre();
        return new SaveGenreInput(
            genre.Id,
            null!,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name))
        );
    }


    public List<Genre> GetGenreList(int length = 10)
        => Enumerable.Range(0, length).Select(_ => GetValidGenre()).ToList();

    public IList<GenreModel> CloneGenresListOrdered(List<GenreModel> genreList, string orderBy, SearchOrder inputOrder)
    {
        var listClone = new List<GenreModel>(genreList);
        var orderedEnumerable = (orderBy.ToLower(), inputOrder) switch
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