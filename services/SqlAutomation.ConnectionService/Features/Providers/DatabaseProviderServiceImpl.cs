namespace SqlAutomation.ConnectionService.Features.Providers;

internal sealed class DatabaseProviderServiceImpl : IDatabaseProviderService
{
    private static readonly IReadOnlyList<DatabaseProviderDescriptor> Catalog =
    [
        new(
            Id: "sql-server",
            DisplayName: "SQL Server",
            DefaultPort: 1433,
            Availability: DatabaseProviderAvailability.Available),
        new(
            Id: "postgresql",
            DisplayName: "PostgreSQL",
            DefaultPort: 5432,
            Availability: DatabaseProviderAvailability.Planned),
        new(
            Id: "mysql",
            DisplayName: "MySQL",
            DefaultPort: 3306,
            Availability: DatabaseProviderAvailability.Planned),
    ];

    public IReadOnlyList<DatabaseProviderDescriptor> Providers => Catalog;
}
