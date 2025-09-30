using FC.Codeflix.Catalog.Infra.Messaging.Extensions;

namespace FC.Codeflix.Catalog.Infra.Messaging.Configuration;

public sealed record KafkaConfiguration
{
    public required KafkaConsumerConfiguration CategoryConsumer { get; init; }
}

public sealed record KafkaConsumerConfiguration
{
    public string BootstrapServers { get; init; } = null!;
    public string GroupId { get; init; } = null!;
    public string Topic { get; init; } = null!;
    public string? RetryTopic { get; set; }
    public string? DlqTopic { get; set; }
    public int ConsumeDelaySeconds { get; private set; } = 0;
    public bool HasRetry => !string.IsNullOrWhiteSpace(RetryTopic);

    public KafkaConsumerConfiguration CreateRetryConfiguration(int retryIndex, bool hasNextRetry) => new()
    {
        BootstrapServers = BootstrapServers,
        GroupId = GroupId,
        Topic = Topic.ToRetryTopic(retryIndex),
        RetryTopic = !hasNextRetry ? null : Topic.ToRetryTopic(retryIndex + 1),
        DlqTopic = DlqTopic,
        ConsumeDelaySeconds = (int)Math.Pow(2, retryIndex)
    };
}