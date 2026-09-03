namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal static class ConnectionTestRequestValidator
{
    public static Dictionary<string, string[]> Validate(
        TestConnectionRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);

        ValidateProvider(request.ProviderId, errors);
        ValidateHost(request.Host, errors);
        ValidatePort(request.Port, errors);
        ValidateDatabase(request.Database, errors);
        ValidateUsername(request.Username, errors);
        ValidatePassword(request.Password, errors);
        ValidateTimeout(request.TimeoutSeconds, errors);

        return errors;
    }

    private static void ValidateProvider(
        string? providerId,
        Dictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(providerId))
        {
            errors["providerId"] = ["Provider is required."];
            return;
        }

        if (!string.Equals(
                providerId.Trim(),
                "sql-server",
                StringComparison.OrdinalIgnoreCase))
        {
            errors["providerId"] =
                ["Only the sql-server provider is currently supported."];
        }
    }

    private static void ValidateHost(
        string? host,
        Dictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            errors["host"] = ["Host is required."];
            return;
        }

        if (host.Length > 255)
        {
            errors["host"] = ["Host cannot exceed 255 characters."];
            return;
        }

        if (host.IndexOfAny([';', ',', '\r', '\n']) >= 0)
        {
            errors["host"] =
                ["Host contains invalid characters. Supply the port separately."];
        }
    }

    private static void ValidatePort(
        int? port,
        Dictionary<string, string[]> errors)
    {
        if (port is <= 0 or > 65535)
        {
            errors["port"] = ["Port must be between 1 and 65535."];
        }
    }

    private static void ValidateDatabase(
        string? database,
        Dictionary<string, string[]> errors)
    {
        if (database is { Length: > 128 })
        {
            errors["database"] =
                ["Database cannot exceed 128 characters."];
        }
    }

    private static void ValidateUsername(
        string? username,
        Dictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            errors["username"] = ["Username is required."];
            return;
        }

        if (username.Length > 128)
        {
            errors["username"] =
                ["Username cannot exceed 128 characters."];
        }
    }

    private static void ValidatePassword(
        string? password,
        Dictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            errors["password"] = ["Password is required."];
            return;
        }

        if (password.Length > 256)
        {
            errors["password"] =
                ["Password cannot exceed 256 characters."];
        }
    }

    private static void ValidateTimeout(
        int timeoutSeconds,
        Dictionary<string, string[]> errors)
    {
        if (timeoutSeconds is < 1 or > 15)
        {
            errors["timeoutSeconds"] =
                ["Timeout must be between 1 and 15 seconds."];
        }
    }
}
