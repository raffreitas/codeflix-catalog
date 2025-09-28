using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.SaveGenre;

[Collection(nameof(SaveGenreTestFixture))]
public sealed class SaveGenreTest(SaveGenreTestFixture fixture) : IDisposable
{
    [Fact(DisplayName = nameof(SaveGenre_WhenInputIsValid_PersistsGenre))]
    [Trait("Integration", "[UseCase] SaveGenre")]
    public async Task SaveGenre_WhenInputIsValid_PersistsGenre()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        var input = fixture.GetValidInput();

        var output = await mediatr.Send(input);

        var persisted = await elasticClient
            .GetAsync<GenreModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);
        document.Categories.Should().BeEquivalentTo(input.Categories);
        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);
        output.Categories.Should().BeEquivalentTo(input.Categories);
    }

    [Fact(DisplayName = nameof(SaveGenre_WhenInputIsInvalid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveGenre")]
    public async Task SaveGenre_WhenInputIsInvalid_ThrowsException()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        var input = fixture.GetInvalidInput();
        const string expectedMessage = "Name should not be null or empty.";

        var action = async () => await mediatr.Send(input);

        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var persisted = await elasticClient
            .GetAsync<GenreModel>(input.Id);
        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => fixture.DeleteAll();
}