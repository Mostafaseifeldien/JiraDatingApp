using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moova.Services.Implementation;
using Moova.Services.Interfaces;
using Movva.Configuration.Implementation;
using Movva.Configuration.Interfaces;

namespace Movva.Integration.BLL;

public static class BusinessLogicDependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();

        services.AddRepositories();

        services.AddConfiguration(configuration);

        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IHttpRequestFactory, HttpRequestFactory>();

        services.AddHttpClient();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
       // services.AddScoped<IAsyncRepository<IntegrationLog>, Repository<IntegrationLog>>();
    }

   

    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IConfiguration>(configuration);
    }
}