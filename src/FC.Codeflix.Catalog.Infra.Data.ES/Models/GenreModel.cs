namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public sealed record GenreModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public IReadOnlyList<GenreCategoryModel> Categories { get; set; } = [];

    public static GenreModel FromEntity(Domain.Entities.Genre entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        IsActive = entity.IsActive,
        CreatedAt = entity.CreatedAt,
        Categories = entity.Categories.Select(category => new GenreCategoryModel(category.Id, category.Name))
            .ToList()
    };
}

public sealed record GenreCategoryModel(Guid Id, string Name);