using ZawatSys.MicroLib.AI.Domain.Entities;

namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for AIAuditRequest.
/// Defines data access operations following Repository pattern.
/// </summary>
public interface IAIAuditRequestRepository
{
    /// <summary>
    /// Adds a new AI audit request.
    /// </summary>
    Task AddAsync(AIAuditRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an audit request by logical request ID and tenant ID.
    /// </summary>
    Task<AIAuditRequest?> GetByRequestIdAsync(Guid requestId, Guid tenantId, CancellationToken cancellationToken = default);
}
