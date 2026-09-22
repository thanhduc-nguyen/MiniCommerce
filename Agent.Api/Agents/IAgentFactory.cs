namespace Agent.Api.Agents;

public interface IAgentFactory
{
    IAgent Create(string name);
}
