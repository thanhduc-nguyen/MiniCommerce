using System.Collections.Concurrent;
using Microsoft.Agents.AI;

namespace Agent.Api.Agents;

// In-memory session store. Registered as a singleton so sessions survive
// across requests. Lazy<Task<>> guarantees the factory runs exactly once per
// key, even under concurrent requests for the same user.
public class InMemoryAgentSessionStore : IAgentSessionStore
{
    private readonly ConcurrentDictionary<string, Lazy<Task<AgentSession>>> sessions = new();

    public Task<AgentSession> GetOrCreateAsync(
        string agentName,
        Guid userGuid,
        Func<CancellationToken, Task<AgentSession>> factory,
        CancellationToken cancellationToken = default)
    {
        var key = $"{agentName}:{userGuid}";
        var lazy = sessions.GetOrAdd(key, _ => new Lazy<Task<AgentSession>>(() => factory(cancellationToken)));
        return lazy.Value;
    }
}
