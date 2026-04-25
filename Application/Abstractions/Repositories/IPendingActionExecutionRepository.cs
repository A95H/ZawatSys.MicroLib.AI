using ZawatSys.MicroLib.AI.Domain.Entities;

namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

public interface IPendingActionExecutionRepository
{
    Task<AIPendingAction?> GetPendingActionAsync(Guid tenantId, Guid pendingActionId, CancellationToken cancellationToken = default);

    Task<AIPendingAction?> GetActivePendingActionAsync(Guid tenantId, Guid conversationId, CancellationToken cancellationToken = default);

    Task<AIAgentRun?> GetRunByOperationAsync(
        Guid tenantId,
        string operationType,
        Guid operationId,
        CancellationToken cancellationToken = default);

    Task<AIAgentRunStep?> GetLatestRunStepAsync(Guid tenantId, Guid aiAgentRunId, CancellationToken cancellationToken = default);

    Task<int> GetNextRunStepSequenceAsync(Guid tenantId, Guid aiAgentRunId, CancellationToken cancellationToken = default);

    Task AddRunAsync(AIAgentRun run, CancellationToken cancellationToken = default);

    Task AddPendingActionAsync(AIPendingAction pendingAction, CancellationToken cancellationToken = default);

    Task AddRunStepAsync(AIAgentRunStep step, CancellationToken cancellationToken = default);
}
