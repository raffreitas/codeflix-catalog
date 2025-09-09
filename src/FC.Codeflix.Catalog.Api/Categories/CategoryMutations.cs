using FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;

using MediatR;

namespace FC.Codeflix.Catalog.Api.Categories;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class CategoryMutations
{
    public async Task<CategoryPayload> SaveCategoryAsync(
        SaveCategoryInput input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var output = await mediator.Send(input, cancellationToken);
        return CategoryPayload.FromCategoryModelOutput(output);
    }

    public async Task<bool> DeleteCategoryAsync(
        Guid id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCategoryInput(id), cancellationToken);
        return true;
    }
}