using FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;

namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public sealed record CategoryPayloadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public SaveCategoryInput ToSaveCategory() => new(Id, Name, Description, CreatedAt, IsActive);
    public DeleteCategoryInput ToDeleteCategory() => new(Id);
};