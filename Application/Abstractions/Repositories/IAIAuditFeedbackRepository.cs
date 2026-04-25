using ZawatSys.MicroLib.AI.Domain.Entities;

namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for AIAuditFeedback.
/// Defines data access operations following Repository pattern.
/// </summary>
public interface IAIAuditFeedbackRepository
{
    /// <summary>
    /// Adds a new AI audit feedback entry.
    /// </summary>
    Task AddAsync(AIAuditFeedback feedback, CancellationToken cancellationToken = default);
}
