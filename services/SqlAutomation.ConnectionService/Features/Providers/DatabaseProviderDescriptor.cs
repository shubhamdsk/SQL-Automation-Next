namespace SqlAutomation.ConnectionService.Features.Providers;

internal sealed record DatabaseProviderDescriptor(
    string Id,
    string DisplayName,
    int DefaultPort,
    DatabaseProviderAvailability Availability);
