namespace Agent.Api.Agents;

public class AgentFactory(IEnumerable<IAgent> agents) : IAgentFactory
{
    public IAgent Create(string name)
    {
        return agents.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Agent '{name}' not found.", nameof(name));
    }
}
