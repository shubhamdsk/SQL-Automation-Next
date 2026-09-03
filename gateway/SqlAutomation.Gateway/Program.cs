var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Service = "SQL Automation Next Gateway",
    Status = "Running",
}));

app.MapHealthChecks("/health");
app.MapReverseProxy();

app.Run();
