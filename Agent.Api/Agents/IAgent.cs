namespace Agent.Api.Agents;

public interface IAgent
{
    string Name { get; }
    Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default);
}
