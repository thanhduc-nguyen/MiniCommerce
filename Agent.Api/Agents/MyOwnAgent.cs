using Agent.Api.Tools.Catalog;
using Google.GenAI;

namespace Agent.Api.Agents;

public class MyOwnAgent(SearchProductTool searchProduct) : IAgent
{
    public string Name => "My Own AI Agent";

    public async Task<string> SendAsync(string prompt, CancellationToken ct = default)
    {
        var client = new Client(apiKey: Environment.GetEnvironmentVariable("Gemini__ApiKey"));

        // 1. Tell the model about its ONE tool and how to ask for it.
        var rules = """
            You can search products. If you need to, reply with exactly:
            SEARCH: <search term>
            Otherwise, just answer the user.
            """;

        // 2. First question to the model.
        var reply = await Ask(client, $"{rules}\n\nUser: {prompt}", ct);

        // 3. Did the model ask for the tool?
        if (reply.StartsWith("SEARCH:"))
        {
            var term = reply["SEARCH:".Length..].Trim();

            // 4. YOUR code runs the tool.
            var found = await searchProduct.InvokeAsync(term, ct);

            // 5. Give the result back and let the model write the final answer.
            reply = await Ask(client, $"User asked: {prompt}\nSearch results: {found}\nNow answer them.", ct);
        }

        return reply;
    }

    private static async Task<string> Ask(Client client, string text, CancellationToken ct)
    {
        var response = await client.Models.GenerateContentAsync("gemini-3.5-flash-lite", text, cancellationToken: ct);
        return response.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text?.Trim() ?? "";
    }
}
