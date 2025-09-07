using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
using FC.Codeflix.Catalog.IntegrationTests.Categories.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.SaveCategory;

public class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
        => new(
            Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean()
        );

    public SaveCategoryInput GetInvalidInput()
        => new(
            Guid.NewGuid(),
            null!,
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean()
        );
}