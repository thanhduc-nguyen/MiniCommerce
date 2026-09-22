using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniCommerce.Web.Models.Agent;
using MiniCommerce.Web.Services.Agent;

namespace MiniCommerce.Web.Pages;

public class IndexModel(IAgentService agentService) : PageModel
{
    [BindProperty]
    public AgentPromptModel AiAgentModel { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSendAsync([FromBody] AgentPromptModel request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Prompt) || string.IsNullOrWhiteSpace(request?.Provider))
        {
            return BadRequest("Provider and prompt are required.");
        }

        var result = await agentService.SendAsync(request, cancellationToken);

        return new JsonResult(new { provider = result.Provider, response = result.Response });
    }
}
