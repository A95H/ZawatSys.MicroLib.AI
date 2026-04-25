using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.Commands;

public static class ConfirmPendingActionIntegrationContractVersions
{
    public const string ContractVersionV1 = "v1";
    public const string PayloadVersionV1 = "v1";
}

public sealed class ConfirmPendingActionIntegrationCmd : IDomainIntegrationCommand
{
    public string ContractVersion { get; set; } = ConfirmPendingActionIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = ConfirmPendingActionIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required Guid PendingActionId { get; set; }
    public required long ExpectedControlVersion { get; set; }

    // Actor metadata fields are optional by contract policy.
    public required ConfirmPendingActionActorMetadataIntegrationCmd Actor { get; set; }

    // Integrity guard for hash/arguments verification.
    public required ConfirmPendingActionIntegrityIntegrationCmd Integrity { get; set; }

    public required DateTimeOffset RequestedAt { get; set; }
    public required DateTimeOffset ConfirmedAt { get; set; }
}

public sealed class ConfirmPendingActionActorMetadataIntegrationCmd
{
    public string? TriggeredBy { get; set; }
    public Guid? ActorId { get; set; }
    public Guid? ContactId { get; set; }
}

public sealed class ConfirmPendingActionIntegrityIntegrationCmd
{
    public required string Hash { get; set; }
    public string? ArgumentsHash { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}
