namespace Agent.Api.Tools;

// A tool the agent can call. Text in, text out — because the LLM only speaks text.
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<string> InvokeAsync(string input, CancellationToken cancellationToken = default);
}
