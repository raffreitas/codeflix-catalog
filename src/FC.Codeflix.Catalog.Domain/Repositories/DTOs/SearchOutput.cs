namespace FC.Codeflix.Catalog.Domain.Repositories.DTOs;

public record SearchOutput<T>(
    int CurrentPage,
    int PerPage,
    int Total,
    IReadOnlyList<T> Items)
    where T : class
{
    public int CurrentPage { get; set; } = CurrentPage;
    public int PerPage { get; set; } = PerPage;
    public int Total { get; set; } = Total;
    public IReadOnlyList<T> Items { get; set; } = Items;
}