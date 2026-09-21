namespace StoreFront.Web.Features.AI.Providers;

public interface IAiAgentProvider
{
    string Name { get; }
    Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default);
}
