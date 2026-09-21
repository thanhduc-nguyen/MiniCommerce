namespace StoreFront.Web.Features.AI.Providers;

public class AiAgentProviderFactory(IEnumerable<IAiAgentProvider> providers) : IAiAgentProviderFactory
{
    public IAiAgentProvider Create(string provider)
    {
        return providers.FirstOrDefault(p => p.Name.Equals(provider, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Provider '{provider}' not found.", nameof(provider));
    }
}
