using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.SaveCategory;

[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest(SaveCategoryTestFixture fixture) : IDisposable
{
    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsValid_PersistsCategory))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCategory_WhenInputIsValid_PersistsCategory()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
        var input = fixture.GetValidInput();

        var output = await mediatr.Send(input);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveCategory_WhenInputIsInvalid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCategory_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
        var input = fixture.GetValidInput();
        var expectedMessage = "Name should not be null or empty.";

        var action = async () => await mediatr.Send(input);

        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var persisted = await elasticClient
            .GetAsync<CategoryModel>(input.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => fixture.DeleteAll();
}
