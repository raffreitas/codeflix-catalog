using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public sealed class CategoryModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static CategoryModel FromEntity(Category entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };


    public static Category ToEntity(CategoryModel model)
        => new(model.Id, model.Name, model.Description, model.CreatedAt, model.IsActive);
}