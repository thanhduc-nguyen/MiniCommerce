namespace Agent.Api.Providers;

public class MicrosoftAgentProvider : IAgentProvider
{
    public string Name => "Microsoft AI Agent";

    public Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
