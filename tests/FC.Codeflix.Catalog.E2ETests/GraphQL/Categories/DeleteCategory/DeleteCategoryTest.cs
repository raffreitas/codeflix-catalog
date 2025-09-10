using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.DeleteCategory;

[Trait("E2E/GraphQL", "[Category] Delete")]
public sealed class DeleteCategoryTest(CategoryTestFixture fixture) : IClassFixture<CategoryTestFixture>, IDisposable
{
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesAnExistingId_DeletesCategory))]
    public async Task DeleteCategory_WhenReceivesAnExistingId_DeletesCategory()
    {
        var elasticClient = fixture.ElasticClient;
        var categoriesExample = fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var id = categoriesExample[3].Id;

        var output = await fixture.GraphQlClient.DeleteCategory
            .ExecuteAsync(id, CancellationToken.None);

        output.Data!.Should().NotBeNull();
        output.Data!.DeleteCategory.Should().BeTrue();
        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(id);
        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesANonExistingId_ReturnsErrors))]
    public async Task DeleteCategory_WhenReceivesANonExistingId_ReturnsErrors()
    {
        var elasticClient = fixture.ElasticClient;
        var categoriesExample = fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var id = Guid.NewGuid();
        var expectedErrorMessage = $"Category '{id}' not found.";

        var output = await fixture.GraphQlClient.DeleteCategory
            .ExecuteAsync(id, CancellationToken.None);

        output.Data.Should().BeNull();
        output.Errors.Should().NotBeEmpty();
        output.Errors.Single().Message.Should().Be(expectedErrorMessage);
    }

    public void Dispose() => fixture.DeleteAll();
}