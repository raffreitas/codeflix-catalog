using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.UnitTests.Common;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.Common;

public class CategoryUseCaseFixture : BaseFixture
{
    public ICategoryRepository GetMockRepository()
        => Substitute.For<ICategoryRepository>();

    public string GetValidName() => Faker.Commerce.Categories(1)[0];
    public string GetValidDescription() => Faker.Commerce.ProductDescription();

    public Category GetValidCategory()
        => new(
            id: Guid.NewGuid(),
            name: GetValidName(),
            description: GetValidDescription(),
            createdAt: DateTime.Now,
            isActive: GetRandomBoolean()
        );
}

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseFixtureCollection : ICollectionFixture<CategoryUseCaseFixture>
{
}