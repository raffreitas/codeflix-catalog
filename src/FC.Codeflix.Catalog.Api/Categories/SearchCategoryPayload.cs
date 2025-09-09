using FC.Codeflix.Catalog.Api.Common;
using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;

namespace FC.Codeflix.Catalog.Api.Categories;

public sealed record SearchCategoryPayload : SearchPayload<CategoryPayload>
{
    public static SearchCategoryPayload FromSearchListOutput(SearchListOutput<CategoryModelOutput> output)
    {
        return new SearchCategoryPayload
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(CategoryPayload.FromCategoryModelOutput).ToList()
        };
    }
};