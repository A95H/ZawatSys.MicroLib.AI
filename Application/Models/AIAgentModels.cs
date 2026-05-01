namespace ZawatSys.MicroLib.AI.Application.Models;

public sealed class AIAgentRunContext
{
    public AIAssistScenario Scenario { get; set; }
    public AIAssistContext AssistContext { get; set; } = new();
}

public sealed class AIAgentResult
{
    public string AgentId { get; set; } = string.Empty;
    public bool Success { get; set; }
    public List<AISuggestion> Suggestions { get; set; } = [];
    public string Reasoning { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public List<string> Warnings { get; set; } = [];
    public string? Error { get; set; }
}
