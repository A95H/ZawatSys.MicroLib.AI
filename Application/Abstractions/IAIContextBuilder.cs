using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIContextBuilder
{
    Task<AIContextBuildResult> BuildDiagnosisContextAsync(
        AIAssistRequest request,
        CancellationToken cancellationToken = default);

    Task<AIContextBuildResult> BuildTreatmentContextAsync(
        AIAssistRequest request,
        CancellationToken cancellationToken = default);
}
