using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreFront.Web.Features.AI.Models;
using StoreFront.Web.Features.AI.Providers;

namespace StoreFront.Web.Pages;

public class IndexModel(IAiAgentProviderFactory factory) : PageModel
{
    [BindProperty]
    public AiAgentModel AiAgentModel { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSendAsync([FromBody] AiAgentModel request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Prompt) || string.IsNullOrWhiteSpace(request?.Provider))
        {
            return BadRequest("Provider and prompt are required.");
        }

        var provider = factory.Create(request.Provider);
        var response = await provider.SendAsync(request.Prompt, cancellationToken);

        return new JsonResult(new { provider = provider.Name, response });
    }
}
