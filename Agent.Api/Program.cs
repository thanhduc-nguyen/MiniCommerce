using Agent.Api;
using Agent.Api.Agents;
using Agent.Api.Tools.MicrosoftAgent;
using Agent.Api.Tools.MyOwnAgent;
using Google.GenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAgentFactory, AgentFactory>();
builder.Services.AddScoped<IAgent, MyOwnAgent>();
builder.Services.AddScoped<IAgent, MicrosoftAgent>();

// The agent is stateless, so build it once and share it across all users.
builder.Services.AddSingleton(_ =>
{
    var apiKey = Environment.GetEnvironmentVariable("Gemini__ApiKey")
        ?? throw new InvalidOperationException("Gemini__ApiKey is not configured.");
    const string model = "gemini-3.5-flash-lite";

    return new ChatClientAgent(
        new Client(vertexAI: false, apiKey: apiKey).AsIChatClient(model),
        name: "Microsoft AI Agent",
        instructions: "You are a good assistant.",
        tools:
        [
            AIFunctionFactory.Create(SearchProductTool.GetWeather),
            AIFunctionFactory.Create(SearchProductTool.GetClimateChange)
        ]);
});

// Sessions are per user and must outlive a single request, so keep them in a singleton store.
builder.Services.AddSingleton<IAgentSessionStore, InMemoryAgentSessionStore>();

builder.Services.AddHttpClient<MiniCommerceClient>(client =>
{
    var baseUrl = builder.Configuration["MiniCommerceWeb:BaseUrl"]
        ?? throw new InvalidOperationException("MiniCommerceWeb:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
});

// Register every tool as ITool; the agent receives them all automatically.
builder.Services.AddScoped<ITool, MyOwnSearchProductTool>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/agent", async (
    AgentModel request,
    IAgentFactory factory,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request?.Prompt) || string.IsNullOrWhiteSpace(request?.Provider))
    {
        return Results.BadRequest("Provider and prompt are required.");
    }

    if (request.UserGuid == Guid.Empty)
    {
        return Results.BadRequest("UserGuid is required.");
    }

    var agent = factory.Create(request.Provider);
    var response = await agent.SendAsync(request.UserGuid, request.Prompt, cancellationToken);

    return Results.Ok(new { provider = agent.Name, response });
});

app.MapHealthChecks("/health");
app.Run();
