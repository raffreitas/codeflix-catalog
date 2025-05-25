using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.SaveCategory;
public class SaveCategoryFixture
    : CategoryUseCaseFixture
{
    public SaveCategoryInput GetValidInput()
        => new(
            Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            DateTime.Now,
            GetRandomBoolean()
        );

    public SaveCategoryInput GetInvalidInput()
        => new(
            Guid.NewGuid(),
            null!,
            GetValidDescription(),
            DateTime.Now,
            GetRandomBoolean()
        );
}

[CollectionDefinition(nameof(SaveCategoryFixture))]
public class SaveCategoryTestFixtureCollection
    : ICollectionFixture<SaveCategoryFixture>
{
}
