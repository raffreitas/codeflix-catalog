using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.Common;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.GetGenresByIds;

[Collection(nameof(GenreTestFixture))]
[Trait("E2E/GraphQL", "[Genre] GetByIds")]
public sealed class GetGenresByIdsTest(GenreTestFixture fixture) : IDisposable
{
    [Fact(DisplayName = nameof(GetGenreByIds_WhenReceivesValidIds_ReturnGenres))]
    public async Task GetGenreByIds_WhenReceivesValidIds_ReturnGenres()
    {
        var elasticClient = fixture.ElasticClient;
        var examples = fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync();
        var genre1 = examples[3];
        var genre2 = examples[5];

        var output = await fixture.GraphQlClient.GetGenresByIds.ExecuteAsync(genre1.Id, genre2.Id);

        output.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Data.Genre1.Name.Should().Be(genre1.Name);
        output.Data.Genre1.CreatedAt.Date.Should().Be(genre1.CreatedAt.Date);
        output.Data.Genre1.IsActive.Should().Be(genre1.IsActive);
        output.Data.Genre1.Categories.Should().BeEquivalentTo(genre1.Categories);
        output.Data.Genre2.Name.Should().Be(genre2.Name);
    }

    [Fact(DisplayName = nameof(GetGenreByIds_WhenNotFindIds_ReturnNull))]
    public async Task GetGenreByIds_WhenNotFindIds_ReturnNull()
    {
        var elasticClient = fixture.ElasticClient;
        var examples = fixture.GetGenreModelList();
        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync();
        var genre1 = examples[3];

        var output = await fixture.GraphQlClient.GetGenresByIds.ExecuteAsync(genre1.Id, Guid.NewGuid());

        output.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Data.Genre1.Name.Should().Be(genre1.Name);
        output.Data.Genre1.CreatedAt.Date.Should().Be(genre1.CreatedAt.Date);
        output.Data.Genre1.IsActive.Should().Be(genre1.IsActive);
        output.Data.Genre1.Categories.Should().BeEquivalentTo(genre1.Categories);
        output.Data.Genre2.Should().BeNull();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }
}