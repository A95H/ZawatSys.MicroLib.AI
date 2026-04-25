using ZawatSys.MicroLib.Shared.Contracts.Common;
using ZawatSys.MicroLib.AI.Domain.Commands;

namespace ZawatSys.MicroLib.AI.Domain.IntegrationEvents;

public static class ProcessConversationTurnIntegrationOutcomes
{
    public const string DirectReply = "DIRECT_REPLY";
    public const string PendingActionCreated = "PENDING_ACTION_CREATED";
}

public sealed class ProcessConversationTurnIntegrationEvent : IDomainIntegrationEvent
{
    public string ContractVersion { get; set; } = ProcessConversationTurnIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = ProcessConversationTurnIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required long ExpectedControlVersion { get; set; }
    public required string Outcome { get; set; }

    public ProcessConversationTurnAuditMetadataIntegrationEvent? AuditMetadata { get; set; }

    // DIRECT_REPLY path.
    public ProcessConversationTurnDirectReplyIntegrationEvent? DirectReply { get; set; }

    // PENDING_ACTION_CREATED path.
    public ProcessConversationTurnPendingActionCreatedIntegrationEvent? PendingAction { get; set; }
}

public sealed class ProcessConversationTurnAuditMetadataIntegrationEvent
{
    public string? AuditTrailId { get; set; }
    public Guid? RunId { get; set; }
    public Guid? RunStepId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public sealed class ProcessConversationTurnDirectReplyIntegrationEvent
{
    public required string MessageType { get; set; }
    public required string Content { get; set; }
    public DateTimeOffset? GeneratedAt { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public sealed class ProcessConversationTurnPendingActionCreatedIntegrationEvent
{
    public required Guid PendingActionId { get; set; }
    public required string PendingActionType { get; set; }
    public required string ConfirmationPrompt { get; set; }
    public string? ConfirmationSummary { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public ProcessConversationTurnPendingActionHandleIntegrationEvent? Handle { get; set; }
}

public sealed class ProcessConversationTurnPendingActionHandleIntegrationEvent
{
    public string? Hash { get; set; }
    public string? Token { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}
