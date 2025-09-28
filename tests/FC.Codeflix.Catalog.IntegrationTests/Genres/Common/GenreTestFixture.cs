using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Genres.Common;

public class GenreTestFixture : BaseFixture, IDisposable
{
    public GenreDataGenerator DataGenerator { get; } = new();
    public readonly ElasticsearchClient ElasticClient;

    public GenreTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();
        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult();
    }

    public Genre GetValidGenre() => DataGenerator.GetValidGenre();
    //
    // public IList<GenreModel> GetGenreModelList(int length = 10)
    //     => DataGenerator.GetGenreModelList(length);


    public void DeleteAll() => ElasticClient.DeleteDocuments<GenreModel>();

    public void Dispose() => ElasticClient.DeleteGenreIndex();
}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{
}