using FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.Common;

using NSubstitute;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Categories.DeleteCategory;

[Collection(nameof(CategoryUseCaseFixture))]
public class DeleteCategoryTest(CategoryUseCaseFixture fixture)
{
    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "[Use Cases] DeleteCategory")]
    public async Task DeleteCategory()
    {
        var repository = fixture.GetMockRepository();
        var input = new DeleteCategoryInput(Guid.NewGuid());
        var useCase = new DeleteCategoryUseCase(repository);

        await useCase.Handle(input, CancellationToken.None);

        await repository
            .Received(1)
            .DeleteAsync(input.Id, Arg.Any<CancellationToken>());
    }
}