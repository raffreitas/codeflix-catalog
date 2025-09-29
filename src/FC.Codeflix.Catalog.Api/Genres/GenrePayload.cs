using FC.Codeflix.Catalog.Application.UseCases.Genres.Common;

namespace FC.Codeflix.Catalog.Api.Genres;

public sealed record GenrePayload(
    Guid Id,
    string? Name,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<GenreCategoryPayload>? Categories)
{
    public static GenrePayload FromGenreModelOutput(GenreModelOutput genreModelOutput)
        => new(
            genreModelOutput.Id,
            genreModelOutput.Name,
            genreModelOutput.IsActive,
            genreModelOutput.CreatedAt,
            genreModelOutput.Categories.Select(category => new GenreCategoryPayload(category.Id, category.Name)));
};

public sealed record GenreCategoryPayload(Guid Id, string Name);