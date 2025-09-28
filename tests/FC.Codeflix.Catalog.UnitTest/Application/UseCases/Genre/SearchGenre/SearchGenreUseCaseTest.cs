using FC.Codeflix.Catalog.Application.UseCases.Genres.SearchGenre;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

using FluentAssertions;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SearchGenre;

public sealed class SearchGenreUseCaseTest(SearchGenreUseCaseTestFixture fixture)
    : IClassFixture<SearchGenreUseCaseTestFixture>
{
    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchGenres")]
    public async Task ReturnsSearchResult()
    {
        var repository = fixture.GetMockRepository();
        var genres = fixture.GetGenreList();
        var input = fixture.GetSearchGenreInput();
        var expectedOutput = genres.Select(genre => new
        {
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            Categories = genre.Categories.Select(category => new { category.Id, category.Name })
        });
        var expectedQueryResult = new SearchOutput<Catalog.Domain.Entities.Genre>(
            input.Page,
            input.PerPage,
            genres.Count,
            genres
        );
        repository.SearchAsync(
                Arg.Any<SearchInput>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedQueryResult));
        var useCase = new SearchGenreUseCase(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalentTo(expectedOutput);
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