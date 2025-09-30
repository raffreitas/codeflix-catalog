using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Application.UseCases.Genres.GetGenresByIds;
using FC.Codeflix.Catalog.IntegrationTests.Genres.Common;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.GetGenreByIds;

[Trait("Integration", "[UseCase] GetGenreByIds")]
[Collection(nameof(GenreTestFixture))]
public sealed class GetGenreByIdsTest(GenreTestFixture fixture) : IDisposable
{
    [Fact]
    public async Task Handle_WhenReceivesValidInput_ReturnsGenres()
    {
        var elasticClient = fixture.ElasticClient;
        var genres = fixture.GetGenreModelList();
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        await elasticClient.IndexManyAsync(genres);
        await elasticClient.Indices.RefreshAsync();
        var expectedOutput = new[] { genres[3], genres[5] };
        var ids = expectedOutput.Select(g => g.Id).ToList();
        var input = new GetGenreByIdsInput(ids);

        var output = await mediatr.Send(input);

        output.Should().BeEquivalentTo(expectedOutput);
    }

    public void Dispose()
    {
        fixture.DeleteAll();
    }
}