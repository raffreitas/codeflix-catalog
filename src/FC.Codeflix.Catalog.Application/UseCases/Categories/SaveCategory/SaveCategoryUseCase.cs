using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.SaveCategory;
public class SaveCategoryUseCase : ISaveCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public SaveCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(SaveCategoryInput request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Id, request.Name, request.Description, request.CreatedAt, request.IsActive);

        await _categoryRepository.SaveAsync(category, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
