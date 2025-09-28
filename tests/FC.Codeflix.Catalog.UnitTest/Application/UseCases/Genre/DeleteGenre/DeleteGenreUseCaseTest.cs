using FC.Codeflix.Catalog.Application.UseCases.Genres.DeleteGenre;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.DeleteGenre;

[Trait("Application", "[UseCase] DeleteGenre")]
public sealed class DeleteGenreUseCaseTest(GenreUseCaseTestFixture fixture) : IClassFixture<GenreUseCaseTestFixture>
{
    [Fact(DisplayName = nameof(DeleteGenre))]
    public async Task DeleteGenre()
    {
        var repository = fixture.GetMockRepository();
        var useCase = new DeleteGenreUseCase(repository);
        var input = new DeleteGenreInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        await repository
            .Received(1)
            .DeleteAsync(input.Id, Arg.Any<CancellationToken>());
    }
}