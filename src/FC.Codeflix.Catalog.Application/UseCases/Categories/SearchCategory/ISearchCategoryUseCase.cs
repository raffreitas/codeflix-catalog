using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.SearchCategory;

public interface ISearchCategoryUseCase
    : IRequestHandler<SearchCategoryInput, SearchListOutput<CategoryModelOutput>>
{
}
