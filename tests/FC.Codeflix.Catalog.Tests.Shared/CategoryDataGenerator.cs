using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public sealed class CategoryDataGenerator : DataGeneratorBase
{
    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription =
            Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription =
                categoryDescription[..10_000];
        return categoryDescription;
    }

    public Category GetValidCategory()
        => new(
            Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean()
        );

    public IList<CategoryModel> GetCategoryModelList(int length = 10)
    {
        var baseDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Enumerable.Range(0, length)
            .Select(index =>
            {
                var createdAt = baseDateTime.AddDays(index);
                var id = new Guid($"00000000-0000-0000-0001-{index:D12}");
                var category = new Category(
                    id,
                    $"Category {index:D3}",
                    $"Description for category {index}",
                    createdAt,
                    index % 2 == 0);
                return CategoryModel.FromEntity(category);
            }).ToList();
    }

    public IList<CategoryModel> GetCategoryModelList(IEnumerable<string> categoryNames)
    {
        var baseDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return categoryNames
            .Select((name, index) =>
            {
                var createdAt = baseDateTime.AddDays(index);
                var id = new Guid($"00000000-0000-0000-0001-{index:D12}");
                var category = new Category(
                    id,
                    name,
                    $"Description for category {index}",
                    createdAt,
                    index % 2 == 0);
                return CategoryModel.FromEntity(category);
            }).ToList();
    }

    public IList<CategoryModel> CloneCategoriesListOrdered(
        IList<CategoryModel> categoriesList,
        string orderBy,
        SearchOrder direction)
    {
        var listClone = new List<CategoryModel>(categoriesList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}