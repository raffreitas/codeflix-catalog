using FC.Codeflix.Catalog.Application.UseCases.Categories.SearchCategory;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

using FluentAssertions;

using Moq;

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
        repository.Setup(x => x
            .SearchAsync(
                It.IsAny<SearchInput>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedQueryResult);
        var useCase = new SearchCategoryUseCase(repository.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalentTo(categories);
        repository.Verify(x => x.SearchAsync(
            It.Is<SearchInput>(search =>
                search.Page == input.Page &&
                search.PerPage == input.PerPage &&
                search.Search == input.Search &&
                search.Order == input.Order &&
                search.OrderBy == input.OrderBy),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
