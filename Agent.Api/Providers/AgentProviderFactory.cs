namespace Agent.Api.Providers;

public class AgentProviderFactory(IEnumerable<IAgentProvider> providers) : IAgentProviderFactory
{
    public IAgentProvider Create(string provider)
    {
        return providers.FirstOrDefault(p => p.Name.Equals(provider, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Provider '{provider}' not found.", nameof(provider));
    }
}
