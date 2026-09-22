namespace Agent.Api.Agents;

public class MicrosoftAgent : IAgent
{
    public string Name => "Microsoft AI Agent";

    public Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
