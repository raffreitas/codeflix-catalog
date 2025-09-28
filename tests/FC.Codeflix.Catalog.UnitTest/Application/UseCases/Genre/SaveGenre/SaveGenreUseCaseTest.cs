using FC.Codeflix.Catalog.Application.UseCases.Genres.SaveGenre;
using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

[Trait("Application", "[UseCase] SaveGenre")]
public sealed class SaveGenreUseCaseTest : IClassFixture<SaveGenreUseCaseTestFixture>
{
    private readonly SaveGenreUseCaseTestFixture _fixture;

    public SaveGenreUseCaseTest(SaveGenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveValidGenre))]
    public async Task SaveValidGenre()
    {
        var repository = _fixture.GetMockRepository();
        var input = _fixture.GetValidInput();
        var useCase = new SaveGenreUseCase(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        await repository
            .Received(1)
            .SaveAsync(Arg.Any<Catalog.Domain.Entities.Genre>(), Arg.Any<CancellationToken>());

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Categories.Should().BeEquivalentTo(input.Categories);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveInvalidGenre))]
    public async Task SaveInvalidGenre()
    {
        var repository = _fixture.GetMockRepository();
        var input = _fixture.GetInalidInput();
        var useCase = new SaveGenreUseCase(repository);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await repository
            .DidNotReceive()
            .SaveAsync(Arg.Any<Catalog.Domain.Entities.Genre>(), Arg.Any<CancellationToken>());

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
}