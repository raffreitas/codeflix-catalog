using FC.Codeflix.Catalog.Domain.Repositories.DTOs;

namespace FC.Codeflix.Catalog.Application.Common;
public record SearchListInput
{
    public int Page { get; init; }
    public int PerPage { get; init; }
    public string Search { get; init; }
    public string OrderBy { get; init; }
    public SearchOrder Order { get; init; }

    public SearchListInput(int page, int perPage, string search, string orderBy, SearchOrder order)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
    }

    public SearchInput ToSearchInput()
        => new(Page, PerPage, Search, OrderBy, Order);
}
