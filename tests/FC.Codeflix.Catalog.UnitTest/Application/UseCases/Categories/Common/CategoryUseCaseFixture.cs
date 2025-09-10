using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Tests.Shared;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.Common;

public class CategoryUseCaseFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();

    public ICategoryRepository GetMockRepository()
        => Substitute.For<ICategoryRepository>();

    public Category GetValidCategory() => DataGenerator.GetValidCategory();
}

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseFixtureCollection : ICollectionFixture<CategoryUseCaseFixture>
{
}