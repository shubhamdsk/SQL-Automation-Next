namespace SqlAutomation.ConnectionService.Features.Providers;

internal static class ProviderEndpoints
{
    public static IEndpointRouteBuilder MapProviderEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/providers");

        group
            .MapGet(
                "",
                (IDatabaseProviderService providerService) =>
                    TypedResults.Ok(providerService.Providers))
            .WithName("GetDatabaseProviders");

        return endpoints;
    }
}
