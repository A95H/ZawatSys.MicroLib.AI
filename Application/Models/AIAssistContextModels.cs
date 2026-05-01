namespace ZawatSys.MicroLib.AI.Application.Models;

public sealed class AIAssistContext
{
    public AIAssistRequest Request { get; set; } = new();
    public List<AIToolResult> ToolResults { get; set; } = [];

    public object ToPromptModel()
    {
        var toolResults = ToolResults ?? [];

        return new
        {
            request = Request.ToPromptModel(),
            toolResults = toolResults.Select(static result => result.ToPromptModel()).ToArray()
        };
    }
}

public sealed class AIToolContext
{
    public AIAssistScenario Scenario { get; set; }
    public AIAssistRequest Request { get; set; } = new();

    public object ToPromptModel() => new
    {
        scenario = Scenario.ToString(),
        request = Request.ToPromptModel()
    };
}

public sealed class AIToolResult
{
    public string Name { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }

    public object ToPromptModel() => new
    {
        name = Name,
        success = Success,
        message = Message,
        data = AIPromptSanitizer.RemoveIdentifierKeys(Data)
    };
}

public sealed class AIPromptPayload
{
    public string PromptVersion { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;

    public object ToPromptModel() => new
    {
        promptVersion = PromptVersion,
        userPrompt = UserPrompt
    };
}
