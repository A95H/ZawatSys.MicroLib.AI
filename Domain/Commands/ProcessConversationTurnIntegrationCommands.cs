using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.Commands;

public static class ProcessConversationTurnIntegrationContractVersions
{
    public const string ContractVersionV1 = "v1";
    public const string PayloadVersionV1 = "v1";
}

public sealed class ProcessConversationTurnIntegrationCmd : IDomainIntegrationCommand
{
    public string ContractVersion { get; set; } = ProcessConversationTurnIntegrationContractVersions.ContractVersionV1;
    public string PayloadVersion { get; set; } = ProcessConversationTurnIntegrationContractVersions.PayloadVersionV1;

    public required Guid TenantId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid SessionId { get; set; }
    public required Guid ConversationMessageId { get; set; }
    public required long ExpectedControlVersion { get; set; }
    public required ProcessConversationTurnUserMessageIntegrationCmd UserMessage { get; set; }

    // Forward-compatibility placeholders for AI-002/AI-003 and future handlers.
    public ProcessConversationTurnContextSnapshotIntegrationCmd? ContextSnapshot { get; set; }
}

public sealed class ProcessConversationTurnUserMessageIntegrationCmd
{
    public required string MessageType { get; set; }
    public required string Content { get; set; }
    public required DateTimeOffset SentAt { get; set; }
    public required Dictionary<string, string> Metadata { get; set; }
}

public sealed class ProcessConversationTurnContextSnapshotIntegrationCmd
{
    public ProcessConversationTurnScenarioSnapshotIntegrationCmd? Scenario { get; set; }
    public ProcessConversationTurnControlSnapshotIntegrationCmd? Control { get; set; }
    public ProcessConversationTurnHistorySnapshotIntegrationCmd? History { get; set; }
}

public sealed class ProcessConversationTurnScenarioSnapshotIntegrationCmd
{
    public string? ScenarioName { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}

public sealed class ProcessConversationTurnControlSnapshotIntegrationCmd
{
    public string? Mode { get; set; }
    public string? State { get; set; }
    public string? Reason { get; set; }
    public long? ControlVersion { get; set; }
}

public sealed class ProcessConversationTurnHistorySnapshotIntegrationCmd
{
    public List<ProcessConversationTurnHistoryMessageIntegrationCmd> Messages { get; set; } = [];
}

public sealed class ProcessConversationTurnHistoryMessageIntegrationCmd
{
    public Guid? ConversationMessageId { get; set; }
    public string? Direction { get; set; }
    public string? MessageType { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}
