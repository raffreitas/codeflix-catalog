using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Application;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services
            .AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}