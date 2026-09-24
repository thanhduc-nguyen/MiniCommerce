using Microsoft.Agents.AI;

namespace Agent.Api.Agents;

// The ChatClientAgent is shared (singleton); only the session is per user.
public class MicrosoftAgent(ChatClientAgent agent, IAgentSessionStore sessionStore) : IAgent
{
    public string Name => "Microsoft AI Agent";

    public async Task<string> SendAsync(Guid userGuid, string prompt, CancellationToken cancellationToken = default)
    {
        AgentSession session = await sessionStore.GetOrCreateAsync(
            Name,
            userGuid,
            async ct => await agent.CreateSessionAsync(ct),
            cancellationToken);

        AgentResponse response = await agent.RunAsync(prompt, session, cancellationToken: cancellationToken);
        return response.ToString();
    }
}
