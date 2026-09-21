namespace StoreFront.Web.Features.AI.Providers;

public interface IAiAgentProviderFactory
{
    IAiAgentProvider Create(string provider);
}
