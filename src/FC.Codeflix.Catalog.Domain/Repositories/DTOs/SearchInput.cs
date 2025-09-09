namespace FC.Codeflix.Catalog.Domain.Repositories.DTOs;

public record SearchInput(int Page, int PerPage, string Search, string OrderBy, SearchOrder Order)
{
    public int Page { get; set; } = Page;
    public int PerPage { get; set; } = PerPage;
    public string Search { get; set; } = Search;
    public string OrderBy { get; set; } = OrderBy;
    public SearchOrder Order { get; set; } = Order;
    public int From => (Page - 1) * PerPage;
}