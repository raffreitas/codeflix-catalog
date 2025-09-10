using Bogus;

namespace FC.Codeflix.Catalog.Tests.Shared;

public abstract class DataGeneratorBase
{
    public Faker Faker { get; set; } = new(locale: "pt_BR");

    public bool GetRandomBoolean() => Faker.Random.Bool();
}