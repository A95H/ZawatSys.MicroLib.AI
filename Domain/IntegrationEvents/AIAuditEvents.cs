using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.IntegrationEvents;

public sealed class AIAssistCompletedEvent : IDomainIntegrationEvent
{
    public Guid AggregateId { get; set; }
    public Guid RequestId { get; set; }
    public Guid TenantId { get; set; }
    public Guid PatientId { get; set; }
    public Guid EncounterId { get; set; }
    public Guid DoctorId { get; set; }
    public string WorkflowType { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public long TotalLatencyMs { get; set; }
    public long? ProviderLatencyMs { get; set; }
    public long? OrchestratorLatencyMs { get; set; }
    public int SuggestionCount { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CreatedByUid { get; set; }
    public Guid UpdatedByUid { get; set; }
}

public sealed class AIFeedbackRecordedEvent : IDomainIntegrationEvent
{
    public Guid AggregateId { get; set; }
    public Guid RequestId { get; set; }
    public Guid SuggestionId { get; set; }
    public Guid TenantId { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorAction { get; set; } = string.Empty;
    public bool HasModificationNotes { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CreatedByUid { get; set; }
    public Guid UpdatedByUid { get; set; }
}
