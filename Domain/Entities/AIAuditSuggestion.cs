using ZawatSys.MicroLib.Shared.Common.Models;

namespace ZawatSys.MicroLib.AI.Domain.Entities;

public sealed class AIAuditSuggestion : TenantEntity
{
    public Guid AIAuditRequestId { get; private set; }
    public Guid SuggestionId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public decimal Confidence { get; private set; }
    public string Reasoning { get; private set; } = string.Empty;

    public AIAuditRequest? Request { get; private set; }

    private AIAuditSuggestion()
    {
    }

    public AIAuditSuggestion(
        Guid aiAuditRequestId,
        Guid suggestionId,
        string type,
        string title,
        string content,
        decimal confidence,
        string reasoning)
    {
        if (aiAuditRequestId == Guid.Empty) throw new ArgumentException("AIAuditRequestId is required.", nameof(aiAuditRequestId));
        if (suggestionId == Guid.Empty) throw new ArgumentException("SuggestionId is required.", nameof(suggestionId));
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type is required.", nameof(type));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required.", nameof(content));
        if (string.IsNullOrWhiteSpace(reasoning)) throw new ArgumentException("Reasoning is required.", nameof(reasoning));
        if (confidence < 0m || confidence > 1m) throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence must be between 0 and 1.");

        AIAuditRequestId = aiAuditRequestId;
        SuggestionId = suggestionId;
        Type = type.Trim();
        Title = title.Trim();
        Content = content;
        Confidence = confidence;
        Reasoning = reasoning;
    }
}
