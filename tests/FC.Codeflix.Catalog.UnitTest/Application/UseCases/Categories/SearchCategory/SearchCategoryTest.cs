using FC.Codeflix.Catalog.Application.UseCases.Categories.SearchCategory;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

using FluentAssertions;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.SearchCategory;

[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest(SearchCategoryTestFixture fixture)
{
    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchCategory")]
    public async Task ReturnsSearchResult()
    {
        var repository = fixture.GetMockRepository();
        var categories = fixture.GetCategoriesList();
        var input = fixture.GetSearchInput();
        var expectedQueryResult = new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            categories.Count,
            categories
        );
        repository.SearchAsync(
                Arg.Any<SearchInput>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedQueryResult));
        var useCase = new SearchCategoryUseCase(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalentTo(categories);
        await repository
            .Received(1)
            .SearchAsync(Arg.Is<SearchInput>(search =>
                    search.Page == input.Page &&
                    search.PerPage == input.PerPage &&
                    search.Search == input.Search &&
                    search.Order == input.Order &&
                    search.OrderBy == input.OrderBy),
                Arg.Any<CancellationToken>());
    }
}