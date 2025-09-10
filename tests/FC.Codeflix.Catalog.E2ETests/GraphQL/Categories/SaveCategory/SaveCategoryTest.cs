using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SaveCategory;

[Trait("E2E", "[Category] Save")]
public sealed class SaveCategoryTest(SaveCategoryTestFixture fixture)
    : IClassFixture<SaveCategoryTestFixture>, IDisposable
{
    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsValid_PersistsCategory))]
    public async Task SaveCategory_WhenInputIsValid_PersistsCategory()
    {
        var elasticClient = fixture.ElasticClient;
        var input = fixture.GetValidInput();

        var output = await fixture.GraphQlClient.SaveCategory
            .ExecuteAsync(input);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Date.Should().Be(input.CreatedAt.Date);
        output.Should().NotBeNull();
        output.Data?.SaveCategory.Should().NotBeNull();
        output.Data?.SaveCategory.Id.Should().Be(input.Id);
        output.Data?.SaveCategory.Name.Should().Be(input.Name);
        output.Data?.SaveCategory.Description.Should().Be(input.Description);
        output.Data?.SaveCategory.IsActive.Should().Be(input.IsActive);
        output.Data?.SaveCategory.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsInvalid_ThrowsException))]
    public async Task SaveCategory_WhenInputIsInvalid_ThrowsException()
    {
        var elasticClient = fixture.ElasticClient;
        var input = fixture.GetInvalidInput();
        const string expectedMessage = "Name should not be null or empty.";

        var output = await fixture.GraphQlClient.SaveCategory
            .ExecuteAsync(input);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedMessage);
        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
    {
        fixture.DeleteAll();
    }
}