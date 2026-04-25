using ZawatSys.MicroLib.Shared.Contracts.Common;
using ZawatSys.MicroLib.AI.Domain.Commands;

namespace ZawatSys.MicroLib.AI.Domain.IntegrationEvents;

public static class ConfirmPendingActionExecutionStatuses
{
    public const string Executed = "EXECUTED";
    public const string Rejected = "REJECTED";
    public const string Expired = "EXPIRED";
    public const string Conflict = "CONFLICT";
    public const string HumanRequired = "HUMAN_REQUIRED";
    public const string Failed = "FAILED";
}

public sealed class ConfirmPendingActionIntegrationEvent : IDomainIntegrationEvent
{
    public string ContractVersion { get; set; } = ConfirmPendingActionIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = ConfirmPendingActionIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required Guid PendingActionId { get; set; }
    public required long ExpectedControlVersion { get; set; }
    public required string ExecutionStatus { get; set; }

    // Safe, user-visible message for Communication/EXP surfaces.
    public required string UserVisibleMessage { get; set; }

    // Audit/result reference placeholders for downstream correlation.
    public ConfirmPendingActionResultReferenceIntegrationEvent? ResultReference { get; set; }

    public ConfirmPendingActionAuditMetadataIntegrationEvent? AuditMetadata { get; set; }

    public List<ConfirmPendingActionIssueIntegrationEvent> Warnings { get; set; } = [];
    public List<ConfirmPendingActionIssueIntegrationEvent> Errors { get; set; } = [];

    public required DateTimeOffset RequestedAt { get; set; }
    public required DateTimeOffset ConfirmedAt { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
}

public sealed class ConfirmPendingActionResultReferenceIntegrationEvent
{
    public string? Service { get; set; }
    public string? Type { get; set; }
    public string? Id { get; set; }
}

public sealed class ConfirmPendingActionAuditMetadataIntegrationEvent
{
    public string? AuditTrailId { get; set; }
    public Guid? RunId { get; set; }
    public Guid? RunStepId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public sealed class ConfirmPendingActionIssueIntegrationEvent
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}
