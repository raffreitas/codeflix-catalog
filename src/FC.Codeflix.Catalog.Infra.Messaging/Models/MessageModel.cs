namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public sealed class MessageModel<T> where T : class
{
    public MessageModelPayload<T> Payload { get; init; }
}

public sealed record MessageModelPayload<T> where T : class
{
    public T Before { get; set; }
    public T After { get; set; }
    public string Op { get; set; }

    public MessageModelOperation? Operation => Op switch
    {
        "c" => MessageModelOperation.Create,
        "u" => MessageModelOperation.Update,
        "d" => MessageModelOperation.Delete,
        "r" => MessageModelOperation.Read,
        _ => null
    };
}

public enum MessageModelOperation
{
    Create = 0,
    Update = 1,
    Delete = 2,
    Read = 3
}