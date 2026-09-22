using Agent.Api.Models;
using Agent.Api.Providers;
using Agent.Api.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAgentProvider, MyOwnAgentProvider>();
builder.Services.AddScoped<IAgentProviderFactory, AgentProviderFactory>();

builder.Services.AddHttpClient<MiniCommerceClient>(client =>
{
    var baseUrl = builder.Configuration["MiniCommerceWeb:BaseUrl"]
        ?? throw new InvalidOperationException("MiniCommerceWeb:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddScoped<SearchProductTool>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/agent", async (
    AiAgentModel request,
    IAgentProviderFactory factory,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request?.Prompt) || string.IsNullOrWhiteSpace(request?.Provider))
    {
        return Results.BadRequest("Provider and prompt are required.");
    }

    var provider = factory.Create(request.Provider);
    var response = await provider.SendAsync(request.Prompt, cancellationToken);

    return Results.Ok(new { provider = provider.Name, response });
});

app.MapHealthChecks("/health");
app.Run();
