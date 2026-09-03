namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal sealed record ConnectionTestResult(
    bool Success,
    string ProviderId,
    string Message,
    long DurationMilliseconds,
    string? ServerVersion,
    string? Database,
    string? ErrorCode);
