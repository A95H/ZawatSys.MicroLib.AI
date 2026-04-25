using System.Text.Json;
using System.Text.RegularExpressions;

namespace ZawatSys.MicroLib.AI.Domain.Services;

public static partial class AIAuditSnapshotSanitizer
{
    private const string RedactedText = "[REDACTED]";
    private static readonly object Excluded = new();
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static string? Serialize(object? snapshot)
    {
        if (snapshot is null)
            return null;

        var element = JsonSerializer.SerializeToElement(snapshot, SerializerOptions);
        var sanitized = SanitizeElement(element);
        return JsonSerializer.Serialize(sanitized, SerializerOptions);
    }

    public static string RedactText(string? value, string fallback = RedactedText)
    {
        if (string.IsNullOrWhiteSpace(value))
            return fallback;

        var sanitized = value.Trim();
        sanitized = EmailRegex().Replace(sanitized, "[REDACTED_EMAIL]");
        sanitized = PhoneRegex().Replace(sanitized, "[REDACTED_PHONE]");
        sanitized = IdNumberRegex().Replace(sanitized, "[REDACTED_ID]");
        return sanitized;
    }

    public static Dictionary<string, string> SanitizeStringDictionary(IEnumerable<KeyValuePair<string, string>>? values)
    {
        var sanitized = new Dictionary<string, string>(StringComparer.Ordinal);
        if (values is null)
            return sanitized;

        foreach (var pair in values)
        {
            if (string.IsNullOrWhiteSpace(pair.Key) || ShouldExcludeKey(pair.Key))
                continue;

            sanitized[pair.Key] = SanitizeStringValue(pair.Key, pair.Value);
        }

        return sanitized;
    }

    private static object? SanitizeElement(JsonElement element, string? propertyName = null)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => SanitizeObject(element),
            JsonValueKind.Array => element.EnumerateArray().Select(item => SanitizeElement(item, propertyName)).Where(item => !ReferenceEquals(item, Excluded)).ToArray(),
            JsonValueKind.String => SanitizeStringValue(propertyName, element.GetString()),
            JsonValueKind.Number => ReadNumber(element),
            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            _ => element.ToString()
        };
    }

    private static IReadOnlyDictionary<string, object?> SanitizeObject(JsonElement element)
    {
        var dictionary = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var property in element.EnumerateObject())
        {
            if (ShouldExcludeKey(property.Name))
                continue;

            var sanitizedValue = SanitizeElement(property.Value, property.Name);
            if (ReferenceEquals(sanitizedValue, Excluded))
                continue;

            dictionary[property.Name] = sanitizedValue;
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

    private static bool ShouldExcludeKey(string key)
    {
        var normalized = NormalizeKey(key);
        return normalized.EndsWith("id", StringComparison.Ordinal)
               || normalized.EndsWith("ids", StringComparison.Ordinal)
               || normalized.EndsWith("token", StringComparison.Ordinal)
               || normalized.EndsWith("tokens", StringComparison.Ordinal)
               || normalized.Contains("apikey", StringComparison.Ordinal)
               || normalized.Contains("secret", StringComparison.Ordinal)
               || normalized.Contains("password", StringComparison.Ordinal);
    }

    private static string SanitizeStringValue(string? key, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var normalized = NormalizeKey(key);
        return RequiresFullRedaction(normalized)
            ? RedactedText
            : RedactText(value, RedactedText);
    }

    private static bool RequiresFullRedaction(string normalizedKey)
        => normalizedKey.Contains("content", StringComparison.Ordinal)
           || normalizedKey.Contains("prompt", StringComparison.Ordinal)
           || normalizedKey.Contains("cancellationreason", StringComparison.Ordinal)
           || normalizedKey.Contains("confirmationprompt", StringComparison.Ordinal)
           || normalizedKey.Contains("confirmationsummary", StringComparison.Ordinal)
           || normalizedKey.Contains("actordisplayname", StringComparison.Ordinal)
           || normalizedKey.Contains("displayname", StringComparison.Ordinal)
           || normalizedKey.Contains("fullname", StringComparison.Ordinal)
           || normalizedKey.Equals("name", StringComparison.Ordinal)
           || normalizedKey.EndsWith("name", StringComparison.Ordinal)
           || normalizedKey.Contains("email", StringComparison.Ordinal)
           || normalizedKey.Contains("phone", StringComparison.Ordinal)
           || normalizedKey.Contains("contact", StringComparison.Ordinal)
           || normalizedKey.Contains("note", StringComparison.Ordinal)
           || normalizedKey.Contains("notes", StringComparison.Ordinal)
           || normalizedKey.Contains("description", StringComparison.Ordinal)
           || normalizedKey.Contains("metadata", StringComparison.Ordinal);

    private static string NormalizeKey(string? key)
        => string.IsNullOrWhiteSpace(key)
            ? string.Empty
            : new string(key.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

    [GeneratedRegex(@"\b[\w.%+-]+@[\w.-]+\.[A-Za-z]{2,}\b", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"\+?\d[\d\s().-]{6,}\d")]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"\b\d{8,16}\b")]
    private static partial Regex IdNumberRegex();
}
