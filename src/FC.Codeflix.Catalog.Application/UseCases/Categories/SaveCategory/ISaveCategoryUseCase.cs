using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
public interface ISaveCategoryUseCase : IRequestHandler<SaveCategoryInput, CategoryModelOutput>
{
}
