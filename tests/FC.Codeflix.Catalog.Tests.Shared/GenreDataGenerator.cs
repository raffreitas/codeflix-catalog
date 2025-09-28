using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.Domain.Entities;
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
        => Enumerable.Range(1, count).Select(_ =>
        {
            Task.Delay(5).GetAwaiter().GetResult();
            return GenreModel.FromEntity(GetValidGenre());
        });

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
}