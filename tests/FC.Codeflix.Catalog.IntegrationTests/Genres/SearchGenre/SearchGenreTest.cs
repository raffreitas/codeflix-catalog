using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Application.UseCases.Genres.SearchGenre;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.SearchGenre;

[Trait("Integration", "[UseCase] SearchGenre")]
[Collection(nameof(SearchGenreTestFixture))]
public sealed class SearchGenreTest(SearchGenreTestFixture fixture) : IDisposable
{
    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList))]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnFilteredList(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount
    )
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        string[] genreNamesList =
        [
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        ];
        var examples = fixture.GetGenreModelList(genreNamesList);
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        var input = new SearchGenreInput(page, perPage, search);

        var output = await mediatr.Send(input);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(page);
        output.PerPage.Should().Be(perPage);
        output.Total.Should().Be(expectedTotalCount);
        output.Items.Should().HaveCount(expectedItemsCount);
        foreach (var item in output.Items)
        {
            var expected = examples.First(x => x.Id == item.Id);
            item.Name.Should().Be(expected.Name);
            item.IsActive.Should().Be(expected.IsActive);
            item.CreatedAt.Should().Be(expected.CreatedAt);
            item.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    [Theory(DisplayName = nameof(SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList))]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchGenre_WhenReceivesValidSearchInput_ReturnOrderedList(string orderBy, string direction)
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        var examples = fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);
        var input = new SearchGenreInput(
            Page: 1,
            PerPage: examples.Count,
            OrderBy: orderBy,
            Order: direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc);
        var expectedList = fixture.CloneGenresListOrdered(
            examples, orderBy, input.Order);

        var output = await mediatr.Send(input);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(examples.Count);
        output.Items.Should().HaveCount(examples.Count);
        for (int i = 0; i < output.Items.Count; i++)
        {
            var outputItem = output.Items[i];
            var expected = expectedList[i];
            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }

    public void Dispose()
    {
        fixture.DeleteAll();
    }
}