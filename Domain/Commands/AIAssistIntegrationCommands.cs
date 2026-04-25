using ZawatSys.MicroLib.AI.Application.Models;
using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.Commands;

public sealed class AssistDiagnosisIntegrationCmd : IDomainIntegrationCommand
{
    public Guid RequestId { get; set; }
    public Guid TenantId { get; set; }
    public string TraceId { get; set; } = string.Empty;
    public Guid PatientId { get; set; }
    public Guid? EncounterId { get; set; }
    public Guid RequestingDoctorId { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public string Locale { get; set; } = "en";
    public OverrideContext? OverrideContext { get; set; }
}

public sealed class AssistTreatmentIntegrationCmd : IDomainIntegrationCommand
{
    public Guid RequestId { get; set; }
    public Guid TenantId { get; set; }
    public string TraceId { get; set; } = string.Empty;
    public Guid PatientId { get; set; }
    public Guid? EncounterId { get; set; }
    public Guid RequestingDoctorId { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public string Locale { get; set; } = "en";
    public OverrideContext? OverrideContext { get; set; }
}

public sealed class AIFeedbackRecordedIntegrationCmd : IDomainIntegrationCommand
{
    public Guid RequestId { get; set; }
    public Guid SuggestionId { get; set; }
    public string DoctorAction { get; set; } = string.Empty;
    public string? ModificationNotes { get; set; }
    public Guid DoctorId { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public Guid TenantId { get; set; }
    public string TraceId { get; set; } = string.Empty;
}
