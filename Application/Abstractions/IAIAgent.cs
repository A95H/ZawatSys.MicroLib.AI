using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIAgent
{
    string AgentId { get; }
    string Role { get; }
    bool SupportsScenario(AIAssistScenario scenario);

    Task<AIAgentResult> RunAsync(AIAgentRunContext context, CancellationToken cancellationToken = default);
}
