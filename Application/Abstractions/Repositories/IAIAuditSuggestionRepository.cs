using ZawatSys.MicroLib.AI.Domain.Entities;

namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for AIAuditSuggestion.
/// Defines data access operations following Repository pattern.
/// </summary>
public interface IAIAuditSuggestionRepository
{
    /// <summary>
    /// Adds a range of AI audit suggestions.
    /// </summary>
    Task AddRangeAsync(IEnumerable<AIAuditSuggestion> suggestions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a suggestion exists for the specified audit request and tenant.
    /// suggestionId is the internal AIAuditSuggestion.Id value.
    /// </summary>
    Task<bool> ExistsAsync(Guid requestEntityId, Guid suggestionId, Guid tenantId, CancellationToken cancellationToken = default);
}
