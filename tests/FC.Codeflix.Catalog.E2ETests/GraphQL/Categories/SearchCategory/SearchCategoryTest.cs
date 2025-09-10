using Elastic.Clients.Elasticsearch;

using RepositoriesDto = FC.Codeflix.Catalog.Domain.Repositories.DTOs;

using FC.Codeflix.Catalog.Infra.Data.ES;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SearchCategory;

public class SearchCategoryTest(SearchCategoryTestFixture fixture)
    : IClassFixture<SearchCategoryTestFixture>, IDisposable
{
    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Category] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCategory_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        var elasticClient = fixture.ElasticClient;
        var categoryNamesList = new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };
        var examples = fixture.GetCategoryModelList(categoryNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);

        var output = await fixture.GraphQlClient.SearchCategory
            .ExecuteAsync(page, perPage, search, "", SearchOrder.Asc, CancellationToken.None);

        output.Data!.Categories.Should().NotBeNull();
        output.Data!.Categories.Items.Should().NotBeNull();
        output.Data!.Categories.CurrentPage.Should().Be(page);
        output.Data!.Categories.PerPage.Should().Be(perPage);
        output.Data!.Categories.Total.Should().Be(expectedTotalCount);
        output.Data!.Categories.Items.Should().HaveCount(expectedItemsCount);

        foreach (var outputItem in output.Data!.Categories.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchCategory_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[Category] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchCategory_WhenReceivesValidSearchInput_ReturnOrderedList(
        string orderBy,
        string direction)
    {
        var elasticClient = fixture.ElasticClient;
        var examples = fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);
        const int page = 1;
        var perPage = examples.Count;
        var directionGraphql = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var directionRepository =
            direction == "asc" ? RepositoriesDto.SearchOrder.Asc : RepositoriesDto.SearchOrder.Desc;

        var expectedList = fixture.CloneCategoriesListOrdered(
            examples, orderBy, directionRepository);

        var output = await fixture.GraphQlClient.SearchCategory
            .ExecuteAsync(page, perPage, "", orderBy, directionGraphql);

        output.Data!.Categories.Should().NotBeNull();
        output.Data!.Categories.Items.Should().NotBeNullOrEmpty();
        output.Data!.Categories.CurrentPage.Should().Be(page);
        output.Data!.Categories.PerPage.Should().Be(perPage);
        output.Data!.Categories.Total.Should().Be(examples.Count);
        output.Data!.Categories.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Data!.Categories.Items.Count; i++)
        {
            var outputItem = output.Data!.Categories.Items[i];
            var expected = expectedList[i];
            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
        }
    }

    public void Dispose() => fixture.DeleteAll();
}