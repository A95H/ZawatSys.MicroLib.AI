using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIPromptTemplate
{
    string Name { get; }
    string Version { get; }
    AIAssistScenario Scenario { get; }

    string BuildUserPrompt(AIAssistContext context);
}
