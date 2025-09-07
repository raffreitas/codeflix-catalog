using FC.Codeflix.Catalog.Domain.Validations;

namespace FC.Codeflix.Catalog.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(
        Guid id,
        string? name,
        string? description,
        DateTime createdAt,
        bool isActive = true)
    {
        Id = id;
        Name = name!;
        Description = description!;
        IsActive = isActive;
        CreatedAt = createdAt;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, fieldName: nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, fieldName: nameof(Name));
        DomainValidation.NotNull(Description, fieldName: nameof(Description));
    }
}