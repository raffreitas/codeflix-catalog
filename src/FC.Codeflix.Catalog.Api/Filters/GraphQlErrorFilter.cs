namespace FC.Codeflix.Catalog.Api.Filters;

public sealed class GraphQlErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.WithMessage(error.Exception?.Message ?? "Unexpected error.");
    }
}