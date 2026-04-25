using ZawatSys.MicroLib.Shared.Common.Models;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public sealed class AIAuditFeedback : TenantEntity
{
    public Guid AIAuditRequestId { get; private set; }
    public Guid SuggestionId { get; private set; }
    public Guid DoctorId { get; private set; }
    public string DoctorAction { get; private set; } = string.Empty;
    public string? ModificationNotes { get; private set; }
    public DateTimeOffset RecordedAt { get; private set; }

    public AIAuditRequest? Request { get; private set; }

    private AIAuditFeedback()
    {
    }

    public AIAuditFeedback(
        Guid aiAuditRequestId,
        Guid suggestionId,
        Guid doctorId,
        string doctorAction,
        string? modificationNotes,
        DateTimeOffset recordedAt)
    {
        if (aiAuditRequestId == Guid.Empty) throw new ArgumentException("AIAuditRequestId is required.", nameof(aiAuditRequestId));
        if (suggestionId == Guid.Empty) throw new ArgumentException("SuggestionId is required.", nameof(suggestionId));
        if (doctorId == Guid.Empty) throw new ArgumentException("DoctorId is required.", nameof(doctorId));
        if (string.IsNullOrWhiteSpace(doctorAction)) throw new ArgumentException("DoctorAction is required.", nameof(doctorAction));

        AIAuditRequestId = aiAuditRequestId;
        SuggestionId = suggestionId;
        DoctorId = doctorId;
        DoctorAction = doctorAction.Trim();
        ModificationNotes = string.IsNullOrWhiteSpace(modificationNotes) ? null : modificationNotes;
        RecordedAt = recordedAt;
    }
}
