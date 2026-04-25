using ZawatSys.MicroLib.AI.Application.Models;

namespace ZawatSys.MicroLib.AI.Application.Abstractions;

public interface IAIPromptBuilder
{
    AIPromptPayload BuildPrompt(AIAssistScenario scenario, AIAssistContext context);
}
