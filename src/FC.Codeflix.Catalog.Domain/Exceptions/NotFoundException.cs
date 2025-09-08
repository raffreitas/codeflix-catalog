namespace FC.Codeflix.Catalog.Domain.Exceptions;

public sealed class NotFoundException(string? message) : Exception(message)
{
}