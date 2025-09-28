using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Application.UseCases.Genres.DeleteGenre;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Genres.Common;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.DeleteGenre;

[Collection(nameof(GenreTestFixture))]
[Trait("Integration", "[UseCase] DeleteGenre")]
public sealed class DeleteGenreTest(GenreTestFixture fixture) : IDisposable
{
    [Fact(DisplayName = nameof(DeleteGenre_WhenReceivesAnExistingId_DeleteGenre))]
    public async Task DeleteGenre_WhenReceivesAnExistingId_DeleteGenre()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        var genresExample = fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(genresExample);
        var input = new DeleteGenreInput(genresExample[3].Id);

        await mediatr.Send(input);

        var deletedGenre = await elasticClient.GetAsync<GenreModel>(genresExample[3].Id);
        deletedGenre.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteGenre_WhenReceivesANonExistingId_ThrowsException))]
    public async Task DeleteGenre_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = fixture.ElasticClient;
        var genresExample = fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(genresExample);
        var input = new DeleteGenreInput(Guid.NewGuid());

        var action = async () => await mediatr.Send(input);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{input.Id}' not found.");
    }

    public void Dispose()
    {
        fixture.DeleteAll();
    }
}