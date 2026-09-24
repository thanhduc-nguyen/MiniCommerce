using Agent.Api;
using Agent.Api.Agents;
using Agent.Api.Tools.MyOwnAgent;
using Agent.Api.Tools.MyOwnAgent.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAgentFactory, AgentFactory>();
builder.Services.AddScoped<IAgent, MyOwnAgent>();
builder.Services.AddScoped<IAgent, MicrosoftAgent>();

builder.Services.AddHttpClient<MiniCommerceClient>(client =>
{
    var baseUrl = builder.Configuration["MiniCommerceWeb:BaseUrl"]
        ?? throw new InvalidOperationException("MiniCommerceWeb:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
});

// Register every tool as ITool; the agent receives them all automatically.
builder.Services.AddScoped<ITool, SearchProductTool>();

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

    var agent = factory.Create(request.Provider);
    var response = await agent.SendAsync(request.Prompt, cancellationToken);

    return Results.Ok(new { provider = agent.Name, response });
});

app.MapHealthChecks("/health");
app.Run();
