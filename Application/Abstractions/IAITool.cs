using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAITool
{
    string Name { get; }

    Task<AIToolResult> ExecuteAsync(AIToolContext context, CancellationToken cancellationToken = default);
}
