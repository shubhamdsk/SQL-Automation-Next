var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Service = "SQL Automation Next Connection Service",
    Status = "Running",
}));

app.MapHealthChecks("/health");

app.Run();
