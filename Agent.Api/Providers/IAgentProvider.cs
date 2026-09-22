namespace Agent.Api.Providers;

public interface IAgentProvider
{
    string Name { get; }
    Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default);
}
