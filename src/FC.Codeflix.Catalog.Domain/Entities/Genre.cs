using FC.Codeflix.Catalog.Domain.Validations;

namespace FC.Codeflix.Catalog.Domain.Entities;

public sealed class Genre
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<Category> _categories = [];
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public Genre(Guid id, string name) : this(id, name, true, DateTime.Now)
    {
    }

    public Genre(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt,
        IEnumerable<Category>? categories = null)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        if (categories != null)
            _categories.AddRange(categories);

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
    }
}