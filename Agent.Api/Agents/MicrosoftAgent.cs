using Agent.Api.Tools.MicrosoftAgent;
using Google.GenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Agent.Api.Agents;

public class MicrosoftAgent : IAgent
{
    public string Name => "Microsoft AI Agent";
    private readonly string apiKey = Environment.GetEnvironmentVariable("Gemini__ApiKey")!;
    private readonly string model = "gemini-3.5-flash-lite";

    public async Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
    {
        ChatClientAgent agentGenAI = new(
            new Client(vertexAI: false, apiKey: apiKey).AsIChatClient(model),
            name: Name,
            instructions: "You are a good assistant.",
            tools: [AIFunctionFactory.Create(SearchProductTool.GetWeather),
                AIFunctionFactory.Create(SearchProductTool.GetClimateChange)]);

        AgentSession session = await agentGenAI.CreateSessionAsync(cancellationToken);

        AgentResponse response = await agentGenAI.RunAsync(prompt, session, cancellationToken: cancellationToken);
        return response.ToString();
    }
}
