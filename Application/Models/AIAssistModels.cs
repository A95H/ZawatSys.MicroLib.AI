using System.Text.Json;

namespace ZawatSys.MicroLib.AI.Application.Models;

public enum AIAssistScenario
{
    Diagnosis = 1,
    Treatment = 2
}

public enum AISuggestionType
{
    Unknown = 0,
    Diagnosis = 1,
    Treatment = 2,
    Warning = 3
}

public enum AIRuleSource
{
    Unknown = 0,
    Code = 1,
    Database = 2,
    Ai = 3
}

public sealed class AIReasoning
{
    public string Summary { get; set; } = string.Empty;
    public List<string> Signals { get; set; } = [];

    public object ToPromptModel() => new
    {
        summary = Summary,
        signals = Signals
    };
}

public sealed class AIRuleTrace
{
    public string RuleId { get; set; } = string.Empty;
    public string RuleVersion { get; set; } = string.Empty;
    public AIRuleSource Source { get; set; } = AIRuleSource.Unknown;
    public int Priority { get; set; }
    public List<string> Signals { get; set; } = [];

    public object ToPromptModel() => new
    {
        rule = RuleId,
        version = RuleVersion,
        source = Source.ToString(),
        priority = Priority,
        signals = Signals
    };
}

public sealed class AIAssistRequest
{
    public Guid RequestId { get; set; }
    public Guid TenantId { get; set; }
    public string? TraceId { get; set; }
    public Guid PatientId { get; set; }
    public Guid? EncounterId { get; set; }
    public Guid RequestingDoctorId { get; set; }
    public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? Locale { get; set; }

    public IReadOnlyCollection<AIObservationInput> Observations { get; set; } = [];
    public IReadOnlyCollection<AIDiagnosisInput> Diagnoses { get; set; } = [];
    public IReadOnlyCollection<AIFactorInput> Factors { get; set; } = [];
    public IReadOnlyCollection<AIOperationInput> Operations { get; set; } = [];
    public IReadOnlyCollection<AIHistoricalOutcomeInput> HistoricalOutcomes { get; set; } = [];

    public AIPatientContext? PatientContext { get; set; }
    public AIEncounterContext? EncounterContext { get; set; }
    public OverrideContext? OverrideContext { get; set; }

    public object ToPromptModel()
    {
        var observations = Observations ?? Array.Empty<AIObservationInput>();
        var diagnoses = Diagnoses ?? Array.Empty<AIDiagnosisInput>();
        var factors = Factors ?? Array.Empty<AIFactorInput>();
        var operations = Operations ?? Array.Empty<AIOperationInput>();
        var historicalOutcomes = HistoricalOutcomes ?? Array.Empty<AIHistoricalOutcomeInput>();
        var patientContext = PatientContext?.ToPromptModel();
        var encounterContext = EncounterContext?.ToPromptModel();

        var promptObservations = observations.Select(static item => item.ToPromptModel()).ToArray();
        var promptDiagnoses = diagnoses.Select(static item => item.ToPromptModel()).ToArray();
        var promptFactors = factors.Select(static item => item.ToPromptModel()).ToArray();
        var promptOperations = operations.Select(static item => item.ToPromptModel()).ToArray();
        var promptHistoricalOutcomes = historicalOutcomes.Select(static item => item.ToPromptModel()).ToArray();

        return new
        {
            //trace = TraceId,
            requestedAt = RequestedAt,
            locale = Locale,
            observations = promptObservations,
            diagnoses = promptDiagnoses,
            factors = promptFactors,
            operations = promptOperations,
            historicalOutcomes = promptHistoricalOutcomes,
            patientContext,
            encounterContext,
            overrideContext = OverrideContext?.ToPromptModel()
        };
    }
}

/// <summary>
/// Flat override context — canonical contract accepted by the EXP layer and forwarded unchanged to the AI service.
/// All ID lists are pre-deduplicated by the EXP layer before forwarding.
/// </summary>
public sealed class OverrideContext
{
    /// <summary>Observation IDs to forcibly include in the AI context.</summary>
    public IReadOnlyCollection<Guid> IncludeObservationIds { get; set; } = [];

    /// <summary>Observation IDs to forcibly exclude from the AI context.</summary>
    public IReadOnlyCollection<Guid> ExcludeObservationIds { get; set; } = [];

    /// <summary>Inline observation data to override or supplement existing observations.</summary>
    public IReadOnlyCollection<ObservationOverrideInput> OverrideObservations { get; set; } = [];

    /// <summary>Diagnosis IDs to forcibly exclude from the AI context.</summary>
    public IReadOnlyCollection<Guid> ExcludeDiagnosisIds { get; set; } = [];

    /// <summary>Inline diagnosis data to override or supplement existing diagnoses.</summary>
    public IReadOnlyCollection<DiagnosisOverrideInput> OverrideDiagnoses { get; set; } = [];

    public object ToPromptModel()
    {
        var includeObservationIds = IncludeObservationIds ?? Array.Empty<Guid>();
        var excludeObservationIds = ExcludeObservationIds ?? Array.Empty<Guid>();
        var excludeDiagnosisIds = ExcludeDiagnosisIds ?? Array.Empty<Guid>();
        var overrideObservations = OverrideObservations ?? Array.Empty<ObservationOverrideInput>();
        var overrideDiagnoses = OverrideDiagnoses ?? Array.Empty<DiagnosisOverrideInput>();

        return new
        {
            includeObservationCount = includeObservationIds.Count,
            excludeObservationCount = excludeObservationIds.Count,
            excludeDiagnosisCount = excludeDiagnosisIds.Count,
            overrideObservations = overrideObservations.Select(static item => item.ToPromptModel()).ToArray(),
            overrideDiagnoses = overrideDiagnoses.Select(static item => item.ToPromptModel()).ToArray()
        };
    }
}

public sealed class ObservationOverrideInput
{
    public Guid ObservationId { get; set; }
    public string? ObservationType { get; set; }
    public string? Code { get; set; }
    public string? Value { get; set; }
    public string? Severity { get; set; }
    public Guid? AffectedLocationId { get; set; }
    public DateTimeOffset? ObservedAt { get; set; }
    public string? Notes { get; set; }

    public object ToPromptModel() => new
    {
        observationType = ObservationType,
        code = Code,
        value = Value,
        severity = Severity,
        observedAt = ObservedAt,
        notes = Notes
    };
}

public sealed class DiagnosisOverrideInput
{
    public Guid PatientDiagnosisId { get; set; }
    public Guid? DiagnosisDefinitionId { get; set; }
    public string? Status { get; set; }
    public string? Severity { get; set; }
    public string? Confidence { get; set; }
    public string? Source { get; set; }
    public DateTimeOffset? DiagnosedAt { get; set; }
    public Guid? AffectedLocationId { get; set; }
    public string? Notes { get; set; }

    public object ToPromptModel() => new
    {
        status = Status,
        severity = Severity,
        confidence = Confidence,
        source = Source,
        diagnosedAt = DiagnosedAt,
        notes = Notes
    };
}

public sealed class AIPatientContext
{
    public Guid PatientId { get; set; }
    public int? Age { get; set; }
    public string? Sex { get; set; }
    public IReadOnlyCollection<string> KnownAllergies { get; set; } = [];
    public IReadOnlyCollection<string> ChronicConditions { get; set; } = [];
    public IReadOnlyCollection<string> Medications { get; set; } = [];
    public string? Specialty { get; set; }

    public object ToPromptModel() => new
    {
        age = Age,
        sex = Sex,
        knownAllergies = KnownAllergies ?? Array.Empty<string>(),
        chronicConditions = ChronicConditions ?? Array.Empty<string>(),
        medications = Medications ?? Array.Empty<string>(),
        specialty = Specialty
    };
}

public sealed class AIEncounterContext
{
    public Guid EncounterId { get; set; }
    public string? EncounterType { get; set; }
    public DateTimeOffset? EncounterStartedAt { get; set; }
    public string? CareSetting { get; set; }
    public Guid DoctorId { get; set; }
    public string? Department { get; set; }

    public object ToPromptModel() => new
    {
        encounterType = EncounterType,
        encounterStartedAt = EncounterStartedAt,
        careSetting = CareSetting,
        department = Department
    };
}

public sealed class AIObservationInput
{
    public Guid ObservationId { get; set; }
    public string ObservationType { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public Guid? AffectedLocationId { get; set; }
    public AIAnatomicalLocationInput? AffectedLocation { get; set; }
    public DateTimeOffset? ObservedAt { get; set; }
    public string? Notes { get; set; }

    public object ToPromptModel() => new
    {
        observationType = ObservationType,
        code = Code,
        value = Value,
        status = Status,
        severity = Severity,
        affectedLocation = AffectedLocation?.ToPromptModel(),
        observedAt = ObservedAt,
        notes = Notes
    };
}

public sealed class AIDiagnosisInput
{
    public Guid PatientDiagnosisId { get; set; }
    public Guid DiagnosisDefinitionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Confidence { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTimeOffset? DiagnosedAt { get; set; }
    public Guid? AffectedLocationId { get; set; }
    public AIAnatomicalLocationInput? AffectedLocation { get; set; }
    public string? Notes { get; set; }

    public object ToPromptModel() => new
    {
        status = Status,
        severity = Severity,
        confidence = Confidence,
        source = Source,
        affectedLocation = AffectedLocation?.ToPromptModel(),
        diagnosedAt = DiagnosedAt,
        notes = Notes
    };
}

public sealed class AIAnatomicalLocationInput
{
    public Guid LocationId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? BodySystem { get; set; }
    public Guid? ParentId { get; set; }
    public string? HierarchyPath { get; set; }

    public object ToPromptModel() => new
    {
        code = Code,
        name = Name,
        type = Type,
        bodySystem = BodySystem,
        hierarchyPath = HierarchyPath
    };
}

public sealed class AIOperationInput
{
    public Guid OperationId { get; set; }
    public Guid EncounterId { get; set; }
    public Guid ProcedureCatalogId { get; set; }
    public string? ProcedureCatalogCode { get; set; }
    public string? ProcedureCatalogName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Priority { get; set; }
    public DateTimeOffset? PlannedDate { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public bool? IsSuccessful { get; set; }
    public string? Notes { get; set; }
    public IReadOnlyCollection<AIAnatomicalLocationInput> AnatomicalLocations { get; set; } = [];

    public object ToPromptModel() => new
    {
        procedureCatalogId = ProcedureCatalogId,
        procedureCatalogCode = ProcedureCatalogCode,
        procedureCatalogName = ProcedureCatalogName,
        status = Status,
        priority = Priority,
        plannedDate = PlannedDate,
        startedAt = StartedAt,
        completedAt = CompletedAt,
        isSuccessful = IsSuccessful,
        notes = Notes,
        anatomicalLocations = (AnatomicalLocations ?? []).Select(static item => item.ToPromptModel()).ToArray()
    };
}

public sealed class AIFactorInput
{
    public Guid PatientFactorId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Value { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? RecordedAt { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Notes { get; set; }

    public object ToPromptModel() => new
    {
        type = Type,
        code = Code,
        value = Value,
        isActive = IsActive,
        recordedAt = RecordedAt,
        startDate = StartDate,
        endDate = EndDate,
        notes = Notes
    };
}

public sealed class AIHistoricalOutcomeInput
{
    public Guid OutcomeId { get; set; }
    public Guid RelatedDiagnosisDefinitionId { get; set; }
    public string PreviousIntervention { get; set; } = string.Empty;
    public string OutcomeSummary { get; set; } = string.Empty;
    public DateTimeOffset? MeasuredAt { get; set; }
    public string Response { get; set; } = string.Empty;

    public object ToPromptModel() => new
    {
        previousIntervention = PreviousIntervention,
        outcomeSummary = OutcomeSummary,
        measuredAt = MeasuredAt,
        response = Response
    };
}

public sealed class AIContextBuildResult
{
    public AIAssistRequest Context { get; set; } = new();
    public object RedactedContextSnapshot { get; set; } = new();

    public object ToPromptModel() => new
    {
        context = Context.ToPromptModel(),
        redactedContextSnapshot = AIPromptSanitizer.RemoveIdentifierKeys(RedactedContextSnapshot)
    };
}

public sealed class AIAssistResponse
{
    public Guid RequestId { get; set; }
    public Guid SuggestionId { get; set; }
    public List<AISuggestion> Suggestions { get; set; } = [];
    public decimal Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public AIReasoning? StructuredReasoning { get; set; }
    public List<AIRuleTrace> RuleTrace { get; set; } = [];
    public List<string> Warnings { get; set; } = [];
    public string? PromptVersion { get; set; }
    public AIProviderMetadata ProviderMetadata { get; set; } = new();
    public DateTimeOffset GeneratedAt { get; set; } = DateTimeOffset.UtcNow;

    public object ToPromptModel()
    {
        var suggestions = Suggestions ?? [];
        var ruleTrace = RuleTrace ?? [];
        var warnings = Warnings ?? [];

        return new
        {
            suggestions = suggestions.Select(static item => item.ToPromptModel()).ToArray(),
            confidence = Confidence,
            reasoning = Reasoning,
            structuredReasoning = StructuredReasoning?.ToPromptModel(),
            ruleTrace = ruleTrace.Select(static item => item.ToPromptModel()).ToArray(),
            warnings,
            promptVersion = PromptVersion,
            providerMetadata = ProviderMetadata.ToPromptModel(),
            generatedAt = GeneratedAt
        };
    }
}

public sealed class AISuggestion
{
    public Guid SuggestionId { get; set; }
    public string? ProviderSuggestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public AISuggestionType SuggestionType { get; set; } = AISuggestionType.Unknown;
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public AIReasoning? StructuredReasoning { get; set; }
    public string? RuleId { get; set; }
    public string? RuleVersion { get; set; }
    public AIRuleSource RuleSource { get; set; } = AIRuleSource.Unknown;
    public int? RulePriority { get; set; }

    public object ToPromptModel() => new
    {
        title = Title,
        suggestionType = SuggestionType.ToString(),
        type = Type,
        content = Content,
        confidence = Confidence,
        reasoning = Reasoning,
        structuredReasoning = StructuredReasoning?.ToPromptModel(),
        rule = RuleId,
        ruleVersion = RuleVersion,
        ruleSource = RuleSource.ToString(),
        rulePriority = RulePriority
    };
}

public sealed class AIProviderMetadata
{
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public long LatencyMs { get; set; }

    public object ToPromptModel() => new
    {
        provider = Provider,
        model = Model,
        latencyMs = LatencyMs
    };
}

public static class AIPromptSanitizer
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static object? RemoveIdentifierKeys(object? value)
    {
        if (value is null)
            return null;

        var jsonElement = JsonSerializer.SerializeToElement(value, SerializerOptions);
        return RemoveIdentifierKeys(jsonElement);
    }

    private static object? RemoveIdentifierKeys(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => RemoveIdentifierKeysFromObject(element),
            JsonValueKind.Array => element.EnumerateArray().Select(RemoveIdentifierKeys).ToArray(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => ReadNumber(element),
            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            _ => element.ToString()
        };
    }

    private static IReadOnlyDictionary<string, object?> RemoveIdentifierKeysFromObject(JsonElement element)
    {
        var dictionary = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var property in element.EnumerateObject())
        {
            if (IsIdentifierKey(property.Name))
                continue;

            dictionary[property.Name] = RemoveIdentifierKeys(property.Value);
        }

        return dictionary;
    }

    private static object ReadNumber(JsonElement element)
    {
        if (element.TryGetInt64(out var longValue))
            return longValue;

        if (element.TryGetDecimal(out var decimalValue))
            return decimalValue;

        return element.GetDouble();
    }

    private static bool IsIdentifierKey(string key)
        => key.EndsWith("id", StringComparison.OrdinalIgnoreCase)
           || key.EndsWith("ids", StringComparison.OrdinalIgnoreCase);
}
