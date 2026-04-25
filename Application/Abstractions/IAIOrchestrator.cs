using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIOrchestrator
{
    Task<AIAssistResponse> AssistDiagnosisAsync(AIAssistRequest request, CancellationToken cancellationToken = default);

    Task<AIAssistResponse> AssistTreatmentAsync(AIAssistRequest request, CancellationToken cancellationToken = default);
}
