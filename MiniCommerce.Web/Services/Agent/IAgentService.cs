using MiniCommerce.Web.Models.Agent;

namespace MiniCommerce.Web.Services.Agent;

public interface IAgentService
{
    Task<AgentResponse> SendAsync(AgentPromptModel request, CancellationToken cancellationToken);
}
