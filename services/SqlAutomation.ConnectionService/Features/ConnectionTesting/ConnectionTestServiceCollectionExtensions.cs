namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal static class ConnectionTestServiceCollectionExtensions
{
    public static IServiceCollection AddConnectionTestServices(
        this IServiceCollection services)
    {
        services.AddScoped<
            IConnectionTestService,
            ConnectionTestServiceImpl>();

        return services;
    }
}
