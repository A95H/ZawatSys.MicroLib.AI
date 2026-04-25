using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIProvider
{
    Task<AIAssistResponse> GenerateAssistAsync(
        AIAssistScenario scenario,
        AIAssistContext context,
        CancellationToken cancellationToken = default);
}
