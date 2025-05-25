namespace FC.Codeflix.Catalog.Application.Common;
public record SearchListOutput<T>
{
    public int CurrentPage { get; init; }
    public int PerPage { get; init; }
    public int Total { get; init; }
    public IReadOnlyList<T> Items { get; init; }

    public SearchListOutput(
        int currentPage,
        int perPage,
        int total,
        IReadOnlyList<T> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
