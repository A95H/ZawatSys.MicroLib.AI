using ZawatSys.MicroLib.Shared.Common.Models;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public sealed class AIAuditRequest : TenantEntity
{
    public Guid RequestId { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid EncounterId { get; private set; }
    public Guid DoctorId { get; private set; }
    public string WorkflowType { get; private set; } = string.Empty;
    public string RedactedContextSnapshot { get; private set; } = "{}";
    public string PromptVersion { get; private set; } = string.Empty;
    public string Provider { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public long TotalLatencyMs { get; private set; }
    public long? ProviderLatencyMs { get; private set; }
    public long? OrchestratorLatencyMs { get; private set; }
    public string? TraceId { get; private set; }

    public List<AIAuditSuggestion> Suggestions { get; private set; } = [];
    public List<AIAuditFeedback> FeedbackEntries { get; private set; } = [];

    private AIAuditRequest()
    {
    }

    public AIAuditRequest(
        Guid requestId,
        Guid patientId,
        Guid encounterId,
        Guid doctorId,
        string workflowType,
        string redactedContextSnapshot,
        string promptVersion,
        string provider,
        string model,
        long totalLatencyMs,
        long? providerLatencyMs,
        long? orchestratorLatencyMs,
        string? traceId)
    {
        if (requestId == Guid.Empty) throw new ArgumentException("RequestId is required.", nameof(requestId));
        if (patientId == Guid.Empty) throw new ArgumentException("PatientId is required.", nameof(patientId));
        if (encounterId == Guid.Empty) throw new ArgumentException("EncounterId is required.", nameof(encounterId));
        if (doctorId == Guid.Empty) throw new ArgumentException("DoctorId is required.", nameof(doctorId));
        if (string.IsNullOrWhiteSpace(workflowType)) throw new ArgumentException("WorkflowType is required.", nameof(workflowType));
        if (string.IsNullOrWhiteSpace(redactedContextSnapshot)) throw new ArgumentException("RedactedContextSnapshot is required.", nameof(redactedContextSnapshot));
        if (string.IsNullOrWhiteSpace(promptVersion)) throw new ArgumentException("PromptVersion is required.", nameof(promptVersion));
        if (string.IsNullOrWhiteSpace(provider)) throw new ArgumentException("Provider is required.", nameof(provider));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model is required.", nameof(model));
        if (totalLatencyMs < 0) throw new ArgumentOutOfRangeException(nameof(totalLatencyMs), "TotalLatencyMs cannot be negative.");

        RequestId = requestId;
        PatientId = patientId;
        EncounterId = encounterId;
        DoctorId = doctorId;
        WorkflowType = workflowType.Trim();
        RedactedContextSnapshot = redactedContextSnapshot;
        PromptVersion = promptVersion.Trim();
        Provider = provider.Trim();
        Model = model.Trim();
        TotalLatencyMs = totalLatencyMs;
        ProviderLatencyMs = providerLatencyMs;
        OrchestratorLatencyMs = orchestratorLatencyMs;
        TraceId = string.IsNullOrWhiteSpace(traceId) ? null : traceId.Trim();
    }
}
