namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal interface IConnectionTestService
{
    Task<ConnectionTestResult> TestAsync(
        TestConnectionRequest request,
        CancellationToken cancellationToken);
}
