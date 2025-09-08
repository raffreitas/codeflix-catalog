using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Infra.Data.ES.Models;

public sealed record CategoryModel
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }

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