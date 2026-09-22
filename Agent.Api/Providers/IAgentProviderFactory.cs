namespace Agent.Api.Providers;

public interface IAgentProviderFactory
{
    IAgentProvider Create(string provider);
}
