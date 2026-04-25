using ZawatSys.MicroLib.Shared.Common.Models;
using ZawatSys.MicroLib.AI.Domain.Services;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public static class AIAgentRunStepCategories
{
    public const string Prompt = "Prompt";
    public const string Tool = "Tool";
    public const string Validation = "Validation";
    public const string Decision = "Decision";
    public const string Execution = "Execution";
}

public sealed class AIAgentRunStep : TenantEntity
{
    public Guid AIAgentRunId { get; private set; }
    public Guid RunId { get; private set; }
    public int StepSequence { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? InputSnapshotJson { get; private set; }
    public string? OutputSnapshotJson { get; private set; }

    public AIAgentRun AIAgentRun { get; private set; } = null!;

    private AIAgentRunStep()
    {
    }

    public AIAgentRunStep(
        Guid aiAgentRunId,
        Guid runId,
        int stepSequence,
        string category,
        string name,
        DateTimeOffset startedAt)
    {
        if (aiAgentRunId == Guid.Empty) throw new ArgumentException("AIAgentRunId is required.", nameof(aiAgentRunId));
        if (runId == Guid.Empty) throw new ArgumentException("RunId is required.", nameof(runId));
        if (stepSequence <= 0) throw new ArgumentOutOfRangeException(nameof(stepSequence), "StepSequence must be greater than zero.");
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required.", nameof(category));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));

        AIAgentRunId = aiAgentRunId;
        RunId = runId;
        StepSequence = stepSequence;
        Category = category.Trim();
        Name = name.Trim();
        StartedAt = startedAt;
    }

    public void Complete(DateTimeOffset completedAt)
    {
        if (completedAt < StartedAt) throw new ArgumentOutOfRangeException(nameof(completedAt), "CompletedAt cannot be earlier than StartedAt.");

        CompletedAt = completedAt;
    }

    public void SetSnapshots(object? inputSnapshot, object? outputSnapshot)
    {
        InputSnapshotJson = AIAuditSnapshotSanitizer.Serialize(inputSnapshot);
        OutputSnapshotJson = AIAuditSnapshotSanitizer.Serialize(outputSnapshot);
    }
}
