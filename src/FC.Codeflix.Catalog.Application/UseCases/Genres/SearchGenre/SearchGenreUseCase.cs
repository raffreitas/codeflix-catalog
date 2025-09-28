using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Genres.SearchGenre;

public sealed class SearchGenreUseCase(IGenreRepository genreRepository) : ISearchGenreUseCase
{
    public async Task<SearchListOutput<GenreModelOutput>> Handle(SearchGenreInput request,
        CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();

        var categories = await genreRepository.SearchAsync(searchInput, cancellationToken);

        return new SearchListOutput<GenreModelOutput>(
            categories.CurrentPage,
            categories.PerPage,
            categories.Total,
            [.. categories.Items.Select(GenreModelOutput.FromGenre)]
        );
    }
}