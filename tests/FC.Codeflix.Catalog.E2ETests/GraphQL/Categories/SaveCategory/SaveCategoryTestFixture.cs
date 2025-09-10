using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SaveCategory;

public sealed class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
    {
        return new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name = DataGenerator.GetValidCategoryName(),
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.Now.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };
    }

    public SaveCategoryInput GetInvalidInput()
    {
        return new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.Now.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };
    }
}