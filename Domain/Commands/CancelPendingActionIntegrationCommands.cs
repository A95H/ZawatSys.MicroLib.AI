using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.Commands;

public static class CancelPendingActionIntegrationContractVersions
{
    public const string ContractVersionV1 = "v1";
    public const string PayloadVersionV1 = "v1";
}

public sealed class CancelPendingActionIntegrationCmd : IDomainIntegrationCommand
{
    public string ContractVersion { get; set; } = CancelPendingActionIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = CancelPendingActionIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required Guid PendingActionId { get; set; }
    public required long ExpectedControlVersion { get; set; }

    public required CancelPendingActionActorIntegrationCmd Actor { get; set; }
    public required string CancellationReason { get; set; }
    public string? CancellationReasonCode { get; set; }
    public required DateTimeOffset CancelledAt { get; set; }

    // Forward-compatibility placeholder metadata for producer-side context.
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public sealed class CancelPendingActionActorIntegrationCmd
{
    public required string TriggeredByType { get; set; }
    public required string ActorId { get; set; }
    public string? ActorDisplayName { get; set; }
    public string? ActorRole { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}
