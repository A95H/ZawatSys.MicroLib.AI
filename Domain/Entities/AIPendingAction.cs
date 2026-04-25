using ZawatSys.MicroLib.Shared.Common.Models;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public static class AIPendingActionStatuses
{
    public const string Pending = "PENDING";
    public const string Executed = "EXECUTED";
    public const string Cancelled = "CANCELLED";
    public const string Expired = "EXPIRED";
    public const string Rejected = "REJECTED";
}

public static class AIPendingActionInitiatorTypes
{
    public const string User = "USER";
    public const string Human = "HUMAN";
    public const string AI = "AI";
    public const string System = "SYSTEM";
    public const string Policy = "POLICY";
}

public sealed class AIPendingAction : TenantEntity
{
    public Guid PendingActionId { get; private set; }
    public Guid ConversationId { get; private set; }
    public Guid SessionId { get; private set; }
    public Guid? ConversationMessageId { get; private set; }
    public Guid? AIAgentRunId { get; private set; }
    public Guid? RunId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string PayloadHash { get; private set; } = string.Empty;
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string InitiatorType { get; private set; } = string.Empty;
    public Guid? InitiatorActorId { get; private set; }
    public Guid? InitiatorContactId { get; private set; }
    public DateTimeOffset? ExecutedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
    public DateTimeOffset? ExpiredAt { get; private set; }
    public DateTimeOffset? RejectedAt { get; private set; }

    public AIAgentRun? InitiatingRun { get; private set; }

    private AIPendingAction()
    {
    }

    public AIPendingAction(
        Guid pendingActionId,
        Guid conversationId,
        Guid sessionId,
        string type,
        string payloadHash,
        DateTimeOffset requestedAt,
        DateTimeOffset? expiresAt,
        string initiatorType)
    {
        if (pendingActionId == Guid.Empty) throw new ArgumentException("PendingActionId is required.", nameof(pendingActionId));
        if (conversationId == Guid.Empty) throw new ArgumentException("ConversationId is required.", nameof(conversationId));
        if (sessionId == Guid.Empty) throw new ArgumentException("SessionId is required.", nameof(sessionId));
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type is required.", nameof(type));
        if (string.IsNullOrWhiteSpace(payloadHash)) throw new ArgumentException("PayloadHash is required.", nameof(payloadHash));
        if (string.IsNullOrWhiteSpace(initiatorType)) throw new ArgumentException("InitiatorType is required.", nameof(initiatorType));
        if (expiresAt.HasValue && expiresAt.Value < requestedAt) throw new ArgumentOutOfRangeException(nameof(expiresAt), "ExpiresAt cannot be earlier than RequestedAt.");

        PendingActionId = pendingActionId;
        ConversationId = conversationId;
        SessionId = sessionId;
        Type = type.Trim();
        PayloadHash = payloadHash.Trim();
        RequestedAt = requestedAt;
        ExpiresAt = expiresAt;
        Status = AIPendingActionStatuses.Pending;
        InitiatorType = initiatorType.Trim();
    }

    public bool IsActive => string.Equals(Status, AIPendingActionStatuses.Pending, StringComparison.Ordinal);

    public bool IsExpired(DateTimeOffset asOf)
        => ExpiresAt.HasValue && ExpiresAt.Value <= asOf;

    public void SetRunContext(Guid? aiAgentRunId, Guid? runId, Guid? conversationMessageId)
    {
        AIAgentRunId = aiAgentRunId;
        RunId = runId;
        ConversationMessageId = conversationMessageId;
    }

    public void SetInitiatorMetadata(Guid? initiatorActorId, Guid? initiatorContactId)
    {
        InitiatorActorId = initiatorActorId;
        InitiatorContactId = initiatorContactId;
    }

    public void SetInitiatingRun(AIAgentRun? initiatingRun)
    {
        InitiatingRun = initiatingRun;
    }

    public void MarkExecuted(DateTimeOffset executedAt)
    {
        EnsurePendingTransition(executedAt, nameof(executedAt));
        Status = AIPendingActionStatuses.Executed;
        ExecutedAt = executedAt;
    }

    public void MarkCancelled(DateTimeOffset cancelledAt)
    {
        EnsurePendingTransition(cancelledAt, nameof(cancelledAt));
        Status = AIPendingActionStatuses.Cancelled;
        CancelledAt = cancelledAt;
    }

    public void MarkExpired(DateTimeOffset expiredAt)
    {
        EnsurePendingTransition(expiredAt, nameof(expiredAt));

        if (ExpiresAt.HasValue && expiredAt < ExpiresAt.Value)
            throw new ArgumentOutOfRangeException(nameof(expiredAt), "ExpiredAt cannot be earlier than ExpiresAt.");

        Status = AIPendingActionStatuses.Expired;
        ExpiredAt = expiredAt;
    }

    public void MarkRejected(DateTimeOffset rejectedAt)
    {
        EnsurePendingTransition(rejectedAt, nameof(rejectedAt));
        Status = AIPendingActionStatuses.Rejected;
        RejectedAt = rejectedAt;
    }

    private void EnsurePendingTransition(DateTimeOffset changedAt, string paramName)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Pending action is already finalized with status '{Status}'.");

        if (changedAt < RequestedAt)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be earlier than RequestedAt.");
    }
}
