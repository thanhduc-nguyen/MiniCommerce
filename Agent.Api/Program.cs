using Agent.Api.Agents;
using Agent.Api.Models;
using Agent.Api.Tools;
using Agent.Api.Tools.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAgent, MyOwnAgent>();
builder.Services.AddScoped<IAgentFactory, AgentFactory>();

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
    IAgentFactory factory,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request?.Prompt) || string.IsNullOrWhiteSpace(request?.Provider))
    {
        return Results.BadRequest("Provider and prompt are required.");
    }

    var agent = factory.Create(request.Provider);
    var response = await agent.SendAsync(request.Prompt, cancellationToken);

    return Results.Ok(new { provider = agent.Name, response });
});

app.MapHealthChecks("/health");
app.Run();
