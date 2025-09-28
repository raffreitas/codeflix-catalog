namespace FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

public sealed record GenreModelOutput
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<GenreModelOutputCategory> Categories { get; private set; }

    public GenreModelOutput(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt,
        IEnumerable<GenreModelOutputCategory> categories
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories.ToList().AsReadOnly();
    }

    public static GenreModelOutput FromGenre(Domain.Entities.Genre genre) => new(
        genre.Id,
        genre.Name,
        genre.IsActive,
        genre.CreatedAt,
        genre.Categories.Select(GenreModelOutputCategory.FromCategory)
    );
}

public class GenreModelOutputCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public static GenreModelOutputCategory FromCategory(Domain.Entities.Category category) => new()
    {
        Id = category.Id, Name = category.Name
    };
}