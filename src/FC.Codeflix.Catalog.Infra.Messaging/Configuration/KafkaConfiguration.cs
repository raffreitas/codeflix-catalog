namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public sealed record KafkaConfiguration
{
    public required string BootstrapServers { get; init; }
    public required KafkaConsumerConfiguration CategoryConsumer { get; init; }
}

public sealed record KafkaConsumerConfiguration
{
    public required string GroupId { get; init; }
    public required string Topic { get; init; }
}