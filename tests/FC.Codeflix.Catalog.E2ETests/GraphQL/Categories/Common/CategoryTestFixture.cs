using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.E2ETests.Base;
using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

public class CategoryTestFixture : CategoryTestFixtureBase
{
    public CatalogClient GraphQlClient { get; }

    public CategoryTestFixture()
    {
        GraphQlClient = WebApplicationFactory.Services.GetRequiredService<CatalogClient>();
    }
}