using FC.Codeflix.Catalog.E2ETests.Base.Fixture;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Genres.Common;

public class GenreTestFixture : GenreTestFixtureBase
{
    public CatalogClient GraphQlClient { get; }

    public GenreTestFixture()
    {
        GraphQlClient = WebApplicationFactory.Services.GetRequiredService<CatalogClient>();
    }
}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{
}