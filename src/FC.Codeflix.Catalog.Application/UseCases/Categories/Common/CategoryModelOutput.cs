using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.Common;

public class CategoryModelOutput(
    Guid id,
    string? name,
    string? description,
    DateTime createdAt,
    bool isActive
)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name!;
    public string Description { get; set; } = description!;
    public DateTime CreatedAt { get; set; } = createdAt;
    public bool IsActive { get; set; } = isActive;

    public static CategoryModelOutput FromCategory(Category category) => new(
        category.Id,
        category.Name,
        category.Description,
        category.CreatedAt,
        category.IsActive
    );
}