﻿using Bogus;

using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Common;
public abstract class BaseFixture
{
    public IServiceProvider ServiceProvider { get; }

    public Faker Faker { get; set; }

    protected BaseFixture()
    {
        Faker = new Faker(locale: "pt_BR");
        ServiceProvider = BuildServiceProvider();
    }

    public bool GetRandomBoolean() => Faker.Random.Bool();

    private ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>()
        {
            {"ConnectionStrings:ElasticSearch","https://localhost:9200" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        services
            .AddUseCases()
            .AddElasticSearch(configuration)
            .AddRepositories();

        return services.BuildServiceProvider();
    }
}
