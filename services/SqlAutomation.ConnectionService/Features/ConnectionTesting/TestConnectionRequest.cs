namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal sealed class TestConnectionRequest
{
    public string? ProviderId { get; init; }

    public string? Host { get; init; }

    public int? Port { get; init; }

    public string? Database { get; init; }

    public string? Username { get; init; }

    public string? Password { get; init; }

    public bool Encrypt { get; init; } = true;

    public bool TrustServerCertificate { get; init; }

    public int TimeoutSeconds { get; init; } = 5;
}
