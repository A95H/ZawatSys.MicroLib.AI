using ZawatSys.MicroLib.Shared.Contracts.Common;
using ZawatSys.MicroLib.AI.Domain.Commands;

namespace ZawatSys.MicroLib.AI.Domain.IntegrationEvents;

public static class CancelPendingActionIntegrationStatuses
{
    public const string Cancelled = "CANCELLED";
    public const string AlreadyClosed = "ALREADY_CLOSED";
    public const string Expired = "EXPIRED";
    public const string Conflict = "CONFLICT";
}

public sealed class CancelPendingActionIntegrationEvent : IDomainIntegrationEvent
{
    public string ContractVersion { get; set; } = CancelPendingActionIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = CancelPendingActionIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required Guid PendingActionId { get; set; }
    public required long ExpectedControlVersion { get; set; }

    // Deterministic cancellation status (idempotent-safe).
    public required string CancellationStatus { get; set; }

    // Safe user-facing explanation suitable for direct display.
    public required string UserMessage { get; set; }

    // Placeholders for AI-104 runtime metadata without changing v1 wire shape later.
    public CancelPendingActionResultMetadataIntegrationEvent? ResultMetadata { get; set; }
    public CancelPendingActionAuditMetadataIntegrationEvent? AuditMetadata { get; set; }
}

public sealed class CancelPendingActionResultMetadataIntegrationEvent
{
    public string? ResultCode { get; set; }
    public string? ResultReason { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public sealed class CancelPendingActionAuditMetadataIntegrationEvent
{
    public string? AuditTrailId { get; set; }
    public Guid? RunId { get; set; }
    public Guid? RunStepId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}
