using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.SaveCategory;

[Collection(nameof(SaveCategoryFixture))]
public class SaveCategoryTest(SaveCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(SaveValidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveValidCategory()
    {
        var repository = fixture.GetMockRepository();
        var input = fixture.GetValidInput();
        var useCase = new SaveCategoryUseCase(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        await repository
            .Received(1)
            .SaveAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveInvalidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveInvalidCategory()
    {
        var repository = fixture.GetMockRepository();
        var input = fixture.GetInvalidInput();
        var useCase = new SaveCategoryUseCase(repository);

        var action = async () => await useCase.Handle(input, CancellationToken.None);


        await repository
            .DidNotReceive()
            .SaveAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
}