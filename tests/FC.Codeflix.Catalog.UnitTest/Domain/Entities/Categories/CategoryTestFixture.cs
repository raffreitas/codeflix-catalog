using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Categories;

public sealed class CategoryTestFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();

    public Category GetValidCategory() => DataGenerator.GetValidCategory();
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{
}