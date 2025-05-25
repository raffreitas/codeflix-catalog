using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.Common;
public class CategoryModelOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public CategoryModelOutput(
        Guid id,
        string? name,
        string? description,
        DateTime createdAt,
        bool isActive)
    {
        Id = id;
        Name = name!;
        Description = description!;
        CreatedAt = createdAt;
        IsActive = isActive;
    }

    public static CategoryModelOutput FromCategory(Category category)
    {
        return new(
            category.Id,
            category.Name,
            category.Description,
            category.CreatedAt,
            category.IsActive
        );
    }
}