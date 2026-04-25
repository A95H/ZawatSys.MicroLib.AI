using ZawatSys.MicroLib.Medical.Application.Integration;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IMedicalAIContextIntegrationClient
{
    Task<GetPatientContextForAIDiagnosisIntegrationQueryResult> GetDiagnosisContextAsync(
        GetPatientContextForAIDiagnosisIntegrationQuery query,
        CancellationToken cancellationToken = default);

    Task<GetPatientContextForAITreatmentIntegrationQueryResult> GetTreatmentContextAsync(
        GetPatientContextForAITreatmentIntegrationQuery query,
        CancellationToken cancellationToken = default);
}
