using Bogus;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validations;

using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Lorem.Word();

        var action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        var fieldName = Faker.Lorem.Word();

        var action = () => DomainValidation.NotNull(value, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null.");
    }

    [Theory(DisplayName = nameof(NotNulOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void NotNulOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Lorem.Word();

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null or empty.");
    }

    [Fact(DisplayName = nameof(NotNulOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNulOrEmptyOk()
    {
        var target = Faker.Lorem.Word();
        var fieldName = Faker.Lorem.Word();

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(GuidNotNulOrEmptyThrowWhenNullOfEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    public void GuidNotNulOrEmptyThrowWhenNullOfEmpty(string? target)
    {
        Guid? value = target == null ? null : Guid.Empty;
        string fieldName = "Id";

        var action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null or empty.");
    }

    [Fact(DisplayName = nameof(GuidNotNulOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void GuidNotNulOrEmptyOk()
    {
        var target = Guid.NewGuid();
        var fieldName = "Id";

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }
}