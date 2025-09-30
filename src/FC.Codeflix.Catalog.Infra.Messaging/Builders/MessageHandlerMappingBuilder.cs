using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging.Builders;

internal sealed class MessageHandlerMappingBuilder<TMessage>(KafkaConsumerBuilder<TMessage> kafkaConsumerBuilder)
    where TMessage : class
{
    private Type? _handlerType;
    private Predicate<MessageModelPayload<TMessage>>? _predicate;

    public KafkaConsumerBuilder<TMessage> And => kafkaConsumerBuilder;

    public MessageHandlerMappingBuilder<TMessage> With<THandler>() where THandler : IMessageHandler<TMessage>
    {
        _handlerType = typeof(THandler);
        return this;
    }

    public MessageHandlerMappingBuilder<TMessage> When(Predicate<MessageModelPayload<TMessage>> predicate)
    {
        _predicate = predicate;
        return this;
    }

    public (Predicate<MessageModelPayload<TMessage>> Predicate, Type Type) Build()
    {
        if (_predicate is null) throw new InvalidOperationException("Predicate is required");
        if (_handlerType is null) throw new InvalidOperationException("Handler type is required");
        return (_predicate, _handlerType);
    }

    public IServiceCollection Register() => kafkaConsumerBuilder.Register();
}