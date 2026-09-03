namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal static class ConnectionTestEndpoints
{
    public static IEndpointRouteBuilder MapConnectionTestEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("/test", TestConnectionAsync)
            .WithName("TestDatabaseConnection");

        return endpoints;
    }

    private static async Task<IResult> TestConnectionAsync(
        TestConnectionRequest request,
        IConnectionTestService connectionTestService,
        CancellationToken cancellationToken)
    {
        var validationErrors =
            ConnectionTestRequestValidator.Validate(request);

        if (validationErrors.Count > 0)
        {
            return Results.ValidationProblem(validationErrors);
        }

        var result = await connectionTestService.TestAsync(
            request,
            cancellationToken);

        return Results.Ok(result);
    }
}
