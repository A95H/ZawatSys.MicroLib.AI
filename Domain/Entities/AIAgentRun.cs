using ZawatSys.MicroLib.Shared.Common.Models;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public static class AIAgentRunOperationTypes
{
    public const string ConversationTurn = "ConversationTurn";
    public const string ConfirmPendingAction = "ConfirmPendingAction";
    public const string CancelPendingAction = "CancelPendingAction";
}

public static class AIAgentRunStatuses
{
    public const string Started = "Started";
    public const string Completed = "Completed";
    public const string Failed = "Failed";
    public const string Rejected = "Rejected";
    public const string Suppressed = "Suppressed";
}

public sealed class AIAgentRun : TenantEntity
{
    public Guid RunId { get; private set; }
    public string OperationType { get; private set; } = string.Empty;
    public Guid OperationId { get; private set; }

    public List<AIAgentRunStep> Steps { get; private set; } = [];
    public List<AIPendingAction> PendingActions { get; private set; } = [];

    public Guid? ConversationId { get; private set; }
    public Guid? ConversationMessageId { get; private set; }
    public Guid? PendingActionId { get; private set; }
    public string? AssistantProfile { get; private set; }
    public Guid? ScenarioInstanceId { get; private set; }
    public string? ScenarioKey { get; private set; }
    public string? ControlModeAtStart { get; private set; }
    public long? IssuedControlVersion { get; private set; }

    public string Status { get; private set; } = string.Empty;
    public string? OutcomeType { get; private set; }

    public string? UserMessageTextRedacted { get; private set; }
    public string? InputSnapshotJson { get; private set; }
    public string? OutputSnapshotJson { get; private set; }

    public string? PromptVersion { get; private set; }
    public string? Provider { get; private set; }
    public string? Model { get; private set; }
    public decimal? Confidence { get; private set; }
    public string? Reasoning { get; private set; }
    public string? WarningCodesJson { get; private set; }

    public long? TotalLatencyMs { get; private set; }
    public long? ProviderLatencyMs { get; private set; }
    public long? OrchestratorLatencyMs { get; private set; }
    public string? TraceId { get; private set; }

    public string? ErrorCode { get; private set; }
    public string? ErrorMessageRedacted { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    private AIAgentRun()
    {
    }

    public AIAgentRun(
        Guid runId,
        string operationType,
        Guid operationId,
        string status,
        DateTimeOffset startedAt)
    {
        if (runId == Guid.Empty) throw new ArgumentException("RunId is required.", nameof(runId));
        if (operationId == Guid.Empty) throw new ArgumentException("OperationId is required.", nameof(operationId));
        if (string.IsNullOrWhiteSpace(operationType)) throw new ArgumentException("OperationType is required.", nameof(operationType));
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.", nameof(status));

        RunId = runId;
        OperationType = operationType.Trim();
        OperationId = operationId;
        Status = status.Trim();
        StartedAt = startedAt;
    }

    public void SetCorrelation(
        Guid? conversationId,
        Guid? conversationMessageId,
        Guid? pendingActionId,
        string? assistantProfile,
        Guid? scenarioInstanceId,
        string? scenarioKey,
        string? controlModeAtStart,
        long? issuedControlVersion)
    {
        ConversationId = conversationId;
        ConversationMessageId = conversationMessageId;
        PendingActionId = pendingActionId;
        AssistantProfile = string.IsNullOrWhiteSpace(assistantProfile) ? null : assistantProfile.Trim();
        ScenarioInstanceId = scenarioInstanceId;
        ScenarioKey = string.IsNullOrWhiteSpace(scenarioKey) ? null : scenarioKey.Trim();
        ControlModeAtStart = string.IsNullOrWhiteSpace(controlModeAtStart) ? null : controlModeAtStart.Trim();
        IssuedControlVersion = issuedControlVersion;
    }

    public void SetSnapshots(
        string? userMessageTextRedacted,
        string? inputSnapshotJson,
        string? outputSnapshotJson,
        string? promptVersion,
        string? provider,
        string? model,
        decimal? confidence,
        string? reasoning,
        string? warningCodesJson)
    {
        UserMessageTextRedacted = string.IsNullOrWhiteSpace(userMessageTextRedacted) ? null : userMessageTextRedacted;
        InputSnapshotJson = string.IsNullOrWhiteSpace(inputSnapshotJson) ? null : inputSnapshotJson;
        OutputSnapshotJson = string.IsNullOrWhiteSpace(outputSnapshotJson) ? null : outputSnapshotJson;
        PromptVersion = string.IsNullOrWhiteSpace(promptVersion) ? null : promptVersion.Trim();
        Provider = string.IsNullOrWhiteSpace(provider) ? null : provider.Trim();
        Model = string.IsNullOrWhiteSpace(model) ? null : model.Trim();
        Confidence = confidence;
        Reasoning = string.IsNullOrWhiteSpace(reasoning) ? null : reasoning;
        WarningCodesJson = string.IsNullOrWhiteSpace(warningCodesJson) ? null : warningCodesJson;
    }

    public void SetTiming(
        long? totalLatencyMs,
        long? providerLatencyMs,
        long? orchestratorLatencyMs,
        string? traceId,
        DateTimeOffset? completedAt)
    {
        TotalLatencyMs = totalLatencyMs;
        ProviderLatencyMs = providerLatencyMs;
        OrchestratorLatencyMs = orchestratorLatencyMs;
        TraceId = string.IsNullOrWhiteSpace(traceId) ? null : traceId.Trim();
        CompletedAt = completedAt;
    }

    public void SetOutcome(
        string status,
        string? outcomeType,
        string? errorCode,
        string? errorMessageRedacted)
    {
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.", nameof(status));

        Status = status.Trim();
        OutcomeType = string.IsNullOrWhiteSpace(outcomeType) ? null : outcomeType.Trim();
        ErrorCode = string.IsNullOrWhiteSpace(errorCode) ? null : errorCode.Trim();
        ErrorMessageRedacted = string.IsNullOrWhiteSpace(errorMessageRedacted) ? null : errorMessageRedacted;
    }
}
