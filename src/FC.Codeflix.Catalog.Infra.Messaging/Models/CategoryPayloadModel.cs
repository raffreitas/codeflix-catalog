using FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;

namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public sealed record CategoryPayloadModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool IsActive { get; init; }

    public SaveCategoryInput ToSaveCategory() => new(Id, Name, Description, CreatedAt, IsActive);
    public DeleteCategoryInput ToDeleteCategory => new(Id);
};