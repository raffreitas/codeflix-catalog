using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.E2ETests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public const string BaseUrl = "http://localhost:61001/";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        const string environment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        builder.UseEnvironment(environment);
        builder.ConfigureServices(services =>
        {
            services.AddCatalogClient();

            services.AddHttpClient("CatalogClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost/graphql");
            }).ConfigurePrimaryHttpMessageHandler(() => Server.CreateHandler());
        });

        base.ConfigureWebHost(builder);
    }
}