using System.Text.Json;
using System.Text.Json.Serialization;

namespace SqlAutomation.ConnectionService.Features.Providers;

internal static class ProviderServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseProviderServices(
        this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter<DatabaseProviderAvailability>(
                    JsonNamingPolicy.CamelCase));
        });

        services.AddSingleton<
            IDatabaseProviderService,
            DatabaseProviderServiceImpl>();

        return services;
    }
}
