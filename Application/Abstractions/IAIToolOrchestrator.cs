using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIToolOrchestrator
{
    Task<IReadOnlyList<AIToolResult>> ExecuteAsync(
        AIToolContext context,
        CancellationToken cancellationToken = default);
}
