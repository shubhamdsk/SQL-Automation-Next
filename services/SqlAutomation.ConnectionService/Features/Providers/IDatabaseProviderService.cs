namespace SqlAutomation.ConnectionService.Features.Providers;

internal interface IDatabaseProviderService
{
    IReadOnlyList<DatabaseProviderDescriptor> Providers { get; }
}
