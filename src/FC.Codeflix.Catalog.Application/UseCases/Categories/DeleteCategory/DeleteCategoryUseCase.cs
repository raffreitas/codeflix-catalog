using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryUseCase(ICategoryRepository repository) : IDeleteCategoryUseCase
{
    public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, cancellationToken);
    }
}
