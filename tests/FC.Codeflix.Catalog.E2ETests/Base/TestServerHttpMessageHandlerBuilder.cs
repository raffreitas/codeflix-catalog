using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Http;

namespace FC.Codeflix.Catalog.E2ETests.Base;

public sealed class TestServerHttpMessageHandlerBuilder(TestServer testServer) : HttpMessageHandlerBuilder
{
    [DisallowNull] public override string? Name { get; set; }
    public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();
    public override HttpMessageHandler PrimaryHandler { get; set; } = testServer.CreateHandler();

    public override HttpMessageHandler Build()
    {
        ArgumentNullException.ThrowIfNull(PrimaryHandler);
        return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
    }
}