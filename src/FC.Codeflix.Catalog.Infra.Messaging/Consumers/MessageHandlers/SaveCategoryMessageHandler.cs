using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;

using MediatR;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;

internal sealed class SaveCategoryMessageHandler(IMediator mediator) : IMessageHandler<CategoryPayloadModel>
{
    public async Task HandleMessageAsync(MessageModelPayload<CategoryPayloadModel> messageModel,
        CancellationToken cancellationToken)
    {
        var saveInput = messageModel.After!.ToSaveCategory();
        await mediator.Send(saveInput, cancellationToken);
    }
}