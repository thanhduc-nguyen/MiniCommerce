using Google.GenAI;

namespace StoreFront.Web.Features.AI.Providers;

public class MyOwnAiAgentProvider : IAiAgentProvider
{
    public string Name => "My Own AI Agent";

    public async Task<string> SendAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable("Gemini__ApiKey");
        var client = new Client(apiKey: apiKey);

        var response = await client.Models.GenerateContentAsync(
            model: "gemini-3.5-flash-lite", contents: prompt,
            cancellationToken: cancellationToken
        );

        return $"{response.Candidates[0].Content.Parts[0].Text}";
    }
}
