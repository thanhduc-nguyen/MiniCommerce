using System.Text;
using Agent.Api.Tools;
using Google.GenAI;

namespace Agent.Api.Agents;

// A tool-calling agent that LOOPS: ask the model, run any tool it asks for,
// feed the result back, and repeat until the model gives a final answer.
public class MyOwnAgent(IEnumerable<ITool> tools) : IAgent
{
    public string Name => "My Own AI Agent";

    // A real agent loops "forever"; this cap stops a confused model looping endlessly.
    private const int MaxSteps = 5;

    public async Task<string> SendAsync(string prompt, CancellationToken ct = default)
    {
        var client = new Client(apiKey: Environment.GetEnvironmentVariable("Gemini__ApiKey"));
        var toolsByName = tools.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        // The whole story the model can see. Tool results get appended each turn,
        // so the next turn can build on what earlier tools returned.
        var conversation = new StringBuilder();
        conversation.AppendLine(BuildRules(toolsByName.Values));
        conversation.AppendLine($"User: {prompt}");

        for (var step = 0; step < MaxSteps; step++)
        {
            // 1. Ask the model what to do next, given everything so far.
            var reply = await Ask(client, conversation.ToString(), ct);

            // 2. Does it want a tool? Format: TOOL <name>: <input>
            if (TryParseToolCall(reply, out var toolName, out var input)
                && toolsByName.TryGetValue(toolName, out var tool))
            {
                var result = await tool.InvokeAsync(input, ct);

                // 3. Record the call + result, then loop — the model may call another tool.
                conversation.AppendLine($"Assistant: {reply}");
                conversation.AppendLine($"ToolResult ({tool.Name}): {result}");
                continue;
            }

            // 4. No tool call -> the model answered. We're done.
            return StripAnswerPrefix(reply);
        }

        return "Sorry, I couldn't finish within the step limit.";
    }

    private static string BuildRules(IEnumerable<ITool> tools)
    {
        var toolList = string.Join(Environment.NewLine, tools.Select(t => $"- {t.Name}: {t.Description}"));

        return $"""
            You are a helpful shopping assistant for MiniCommerce.
            Work one step at a time: you may call a tool, see its result, then decide again.

            Tools you can use:
            {toolList}

            To call a tool, reply with exactly one line:
            TOOL <tool name>: <input>

            When you are ready to answer the user, reply with:
            ANSWER: <your answer>
            """;
    }

    private static bool TryParseToolCall(string reply, out string toolName, out string input)
    {
        toolName = "";
        input = "";

        // Find a line like: TOOL search_products: red shoes
        var line = reply.Split('\n')
            .Select(l => l.Trim())
            .FirstOrDefault(l => l.StartsWith("TOOL ", StringComparison.OrdinalIgnoreCase));
        if (line is null)
        {
            return false;
        }

        var rest = line["TOOL ".Length..];
        var colon = rest.IndexOf(':');
        if (colon < 0)
        {
            return false;
        }

        toolName = rest[..colon].Trim();
        input = rest[(colon + 1)..].Trim();
        return toolName.Length > 0;
    }

    private static string StripAnswerPrefix(string reply)
    {
        const string prefix = "ANSWER:";
        var idx = reply.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
        return idx >= 0 ? reply[(idx + prefix.Length)..].Trim() : reply.Trim();
    }

    private static async Task<string> Ask(Client client, string text, CancellationToken ct)
    {
        var response = await client.Models.GenerateContentAsync("gemini-3.5-flash-lite", text, cancellationToken: ct);
        return response.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text?.Trim() ?? "";
    }
}
