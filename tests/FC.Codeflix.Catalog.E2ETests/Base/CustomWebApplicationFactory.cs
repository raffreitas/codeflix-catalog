using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public readonly string BaseUrl = "http://localhost:61001/";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        const string environment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        builder.UseEnvironment(environment);
        builder.ConfigureServices(services =>
        {
            services.AddTransient<TestServerHttpMessageHandlerBuilder>(sp =>
                new TestServerHttpMessageHandlerBuilder(Server));

            services.AddCatalogClient()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("http://localhost/graphql");
                });
        });

        base.ConfigureWebHost(builder);
    }
}