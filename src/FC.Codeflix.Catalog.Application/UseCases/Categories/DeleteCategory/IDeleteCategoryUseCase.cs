using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
public interface IDeleteCategoryUseCase : IRequestHandler<DeleteCategoryInput>
{
}
