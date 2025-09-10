using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
using FC.Codeflix.Catalog.IntegrationTests.Categories.Common;
using FC.Codeflix.Catalog.Tests.Shared;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.SaveCategory;

public class SaveCategoryTestFixture : CategoryTestFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();

    public SaveCategoryInput GetValidInput()
        => new(
            Guid.NewGuid(),
            DataGenerator.GetValidCategoryName(),
            DataGenerator.GetValidCategoryDescription(),
            DateTime.Now,
            DataGenerator.GetRandomBoolean()
        );

    public SaveCategoryInput GetInvalidInput()
        => new(
            Guid.NewGuid(),
            null!,
            DataGenerator.GetValidCategoryDescription(),
            DateTime.Now,
            DataGenerator.GetRandomBoolean()
        );
}