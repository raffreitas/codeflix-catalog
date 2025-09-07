using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Categories;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest(CategoryTestFixture categoryTestFixture)
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = categoryTestFixture.GetValidCategory();

        var category = new Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = categoryTestFixture.GetValidCategory();

        var category = new Category(
            validCategory.Id,
            validCategory.Name,
            validCategory.Description,
            validCategory.CreatedAt,
            isActive);

        category.Should().NotBeNull();
        category.Id.Should().Be(validCategory.Id);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = categoryTestFixture.GetValidCategory();

        var action = () => new Category(validCategory.Id, name!, validCategory.Description, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = categoryTestFixture.GetValidCategory();

        var action = () => new Category(validCategory.Id, validCategory.Name, null!, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null.");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenIdIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenIdIsEmpty()
    {
        var validCategory = categoryTestFixture.GetValidCategory();

        var action = () => new Category(Guid.Empty, validCategory.Name, validCategory.Name, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Id should not be null or empty.");
    }
}