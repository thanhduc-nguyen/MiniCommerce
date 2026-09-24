namespace Agent.Api.Agents;

public interface IAgent
{
    string Name { get; }
    Task<string> SendAsync(Guid userGuid, string prompt, CancellationToken cancellationToken = default);
}
