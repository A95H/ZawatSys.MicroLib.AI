using ZawatSys.MicroLib.AI.Domain.Entities;

namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

public interface IAIPendingActionRepository
{
    Task<AIPendingAction?> GetByIdAsync(Guid pendingActionId, Guid tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(AIPendingAction pendingAction, CancellationToken cancellationToken = default);
    void Update(AIPendingAction pendingAction);
    Task<IReadOnlyList<AIPendingAction>> GetActiveByConversationAsync(Guid conversationId, Guid tenantId, CancellationToken cancellationToken = default);
}