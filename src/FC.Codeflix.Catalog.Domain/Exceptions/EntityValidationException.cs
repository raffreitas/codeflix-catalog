namespace FC.Codeflix.Catalog.Domain.Exceptions;

public sealed class EntityValidationException(string? message) 
    : BusinessRuleException(message)
{
}
