using ZawatSys.MicroLib.Shared.Contracts.Common;

namespace ZawatSys.MicroLib.AI.Domain.IntegrationEvents;

public sealed class AssistDiagnosisIntegrationEvent : IDomainIntegrationEvent
{
    public Guid RequestId { get; set; }
    public Guid SuggestionId { get; set; }
    public List<AIAssistSuggestionIntegrationEventItem> Suggestions { get; set; } = [];
    public decimal Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public List<string> Warnings { get; set; } = [];
    public string? PromptVersion { get; set; }
    public AIAssistProviderMetadataIntegrationEvent? ProviderMetadata { get; set; }
    public DateTimeOffset GeneratedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AssistTreatmentIntegrationEvent : IDomainIntegrationEvent
{
    public Guid RequestId { get; set; }
    public Guid SuggestionId { get; set; }
    public List<AIAssistSuggestionIntegrationEventItem> Suggestions { get; set; } = [];
    public decimal Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public List<string> Warnings { get; set; } = [];
    public string? PromptVersion { get; set; }
    public AIAssistProviderMetadataIntegrationEvent? ProviderMetadata { get; set; }
    public DateTimeOffset GeneratedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AIAssistSuggestionIntegrationEventItem
{
    public Guid SuggestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
}

public sealed class AIAssistProviderMetadataIntegrationEvent
{
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public long LatencyMs { get; set; }
}
