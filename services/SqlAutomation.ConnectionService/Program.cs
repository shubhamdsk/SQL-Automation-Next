using SqlAutomation.ConnectionService.Features.ConnectionTesting;
using SqlAutomation.ConnectionService.Features.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDatabaseProviderServices();
builder.Services.AddConnectionTestServices();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Service = "SQL Automation Next Connection Service",
    Status = "Running",
}));

app.MapHealthChecks("/health");
app.MapProviderEndpoints();
app.MapConnectionTestEndpoints();

app.Run();
