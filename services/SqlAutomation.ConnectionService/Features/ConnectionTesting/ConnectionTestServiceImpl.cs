using System.Diagnostics;

using Microsoft.Data.SqlClient;

namespace SqlAutomation.ConnectionService.Features.ConnectionTesting;

internal sealed class ConnectionTestServiceImpl : IConnectionTestService
{
    private const string ProviderId = "sql-server";
    private const int DefaultSqlServerPort = 1433;
    private const string DefaultDatabase = "master";

    public async Task<ConnectionTestResult> TestAsync(
        TestConnectionRequest request,
        CancellationToken cancellationToken)
    {
        var startedAt = Stopwatch.GetTimestamp();

        try
        {
            var connectionString = BuildConnectionString(request);

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            return new ConnectionTestResult(
                Success: true,
                ProviderId: ProviderId,
                Message: "Connection successful.",
                DurationMilliseconds: GetElapsedMilliseconds(startedAt),
                ServerVersion: connection.ServerVersion,
                Database: connection.Database,
                ErrorCode: null);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (SqlException exception)
        {
            var failure = MapSqlFailure(exception.Number);

            return CreateFailure(
                startedAt,
                failure.ErrorCode,
                failure.Message);
        }
        catch (ArgumentException)
        {
            return CreateFailure(
                startedAt,
                "invalid_configuration",
                "The connection settings are invalid.");
        }
        catch (InvalidOperationException)
        {
            return CreateFailure(
                startedAt,
                "invalid_configuration",
                "The connection settings are invalid.");
        }
    }

    private static string BuildConnectionString(
        TestConnectionRequest request)
    {
        var port = request.Port ?? DefaultSqlServerPort;

        var database = string.IsNullOrWhiteSpace(request.Database)
            ? DefaultDatabase
            : request.Database.Trim();

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = $"tcp:{request.Host!.Trim()},{port}",
            InitialCatalog = database,
            UserID = request.Username!.Trim(),
            Password = request.Password!,
            IntegratedSecurity = false,
            Encrypt = request.Encrypt
                ? SqlConnectionEncryptOption.Mandatory
                : SqlConnectionEncryptOption.Optional,
            TrustServerCertificate = request.TrustServerCertificate,
            ConnectTimeout = request.TimeoutSeconds,
            ConnectRetryCount = 0,
            Pooling = false,
            PersistSecurityInfo = false,
            ApplicationName = "SQL Automation Next",
        };

        return builder.ConnectionString;
    }

    private static ConnectionTestResult CreateFailure(
        long startedAt,
        string errorCode,
        string message)
    {
        return new ConnectionTestResult(
            Success: false,
            ProviderId: ProviderId,
            Message: message,
            DurationMilliseconds: GetElapsedMilliseconds(startedAt),
            ServerVersion: null,
            Database: null,
            ErrorCode: errorCode);
    }

    private static (string ErrorCode, string Message) MapSqlFailure(
        int sqlErrorNumber)
    {
        return sqlErrorNumber switch
        {
            -2 => (
                "connection_timeout",
                "The SQL Server connection timed out."),
            18456 => (
                "authentication_failed",
                "SQL Server authentication failed."),
            4060 => (
                "database_unavailable",
                "The requested database could not be opened."),
            _ => (
                "connection_failed",
                "Unable to connect to SQL Server."),
        };
    }

    private static long GetElapsedMilliseconds(long startedAt)
    {
        return (long)Stopwatch
            .GetElapsedTime(startedAt)
            .TotalMilliseconds;
    }
}
