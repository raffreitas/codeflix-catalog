namespace FC.Codeflix.Catalog.Domain.Exceptions;

public abstract class BusinessRuleException(string? message) : Exception(message);