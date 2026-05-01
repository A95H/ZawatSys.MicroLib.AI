using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIAgentOrchestrator
{
    Task<AIAssistResponse> RunAsync(
        AIAssistScenario scenario,
        AIAssistContext context,
        CancellationToken cancellationToken = default);
}
