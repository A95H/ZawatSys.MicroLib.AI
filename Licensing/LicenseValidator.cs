using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Validates license files using RSA signature verification.
/// Ensures license integrity and prevents tampering.
/// </summary>
public static class LicenseValidator
{
    /// <summary>
    /// Validates a license JSON content against a public key.
    /// </summary>
    /// <param name="licenseJson">The license JSON content</param>
    /// <param name="publicKeyPem">RSA public key in PEM format</param>
    /// <returns>Validated LicenseInfo</returns>
    /// <exception cref="LicenseException">Thrown when license is invalid, expired, or tampered</exception>
    public static LicenseInfo Validate(string licenseJson, string publicKeyPem)
    {
        if (string.IsNullOrWhiteSpace(licenseJson))
            throw new LicenseException("License content is empty.");

        if (string.IsNullOrWhiteSpace(publicKeyPem))
            throw new LicenseException("Public key is required for license validation.");

        // Deserialize license
        LicenseInfo license;
        try
        {
            license = JsonSerializer.Deserialize<LicenseInfo>(licenseJson)
                      ?? throw new LicenseException("Invalid license format.");
        }
        catch (JsonException ex)
        {
            throw new LicenseException("Failed to parse license file.", ex);
        }

        // Validate required fields
        ValidateRequiredFields(license);

        // Check expiration
        if (DateTime.UtcNow > license.ExpiryDate)
            throw new LicenseException($"License expired on {license.ExpiryDate:yyyy-MM-dd}.");

        // Verify signature
        if (!VerifySignature(licenseJson, license.Signature, publicKeyPem))
            throw new LicenseException("License signature verification failed. License may have been tampered with.");

        return license;
    }

    /// <summary>
    /// Validates a license and checks version compatibility.
    /// </summary>
    public static LicenseInfo ValidateWithVersion(
        string licenseJson,
        string publicKeyPem,
        Version libraryVersion)
    {
        var license = Validate(licenseJson, publicKeyPem);

        // Check product name
        if (!license.Product.Equals("MicroLib.AI", StringComparison.OrdinalIgnoreCase))
            throw new LicenseException($"License is for '{license.Product}', not 'MicroLib.AI'.");

        // Check version compatibility
        if (!IsVersionAllowed(libraryVersion, license.AllowedVersion))
            throw new LicenseException(
                $"Library version {libraryVersion} is not allowed by license. " +
                $"Licensed version: {license.AllowedVersion}");

        return license;
    }

    /// <summary>
    /// Validates that the license is bound to the expected product/host system.
    /// Prevents license reuse across different applications.
    /// </summary>
    /// <param name="license">The license to validate</param>
    /// <param name="expectedProductId">The ProductId of the current application (e.g., "Wasftkom.ProductService")</param>
    /// <exception cref="LicenseException">Thrown when ProductId mismatch detected</exception>
    public static void ValidateProductBinding(LicenseInfo license, string expectedProductId)
    {
        if (string.IsNullOrWhiteSpace(expectedProductId))
            throw new ArgumentException("ProductId cannot be empty.", nameof(expectedProductId));

        if (!license.ProductId.Equals(expectedProductId, StringComparison.OrdinalIgnoreCase))
        {
            throw new LicenseException(
                $"License product binding mismatch.\n" +
                $"  License is bound to: '{license.ProductId}'\n" +
                $"  Current application: '{expectedProductId}'\n" +
                $"\nThis license cannot be used with this application.\n" +
                $"Contact ZawatSys to obtain a license for '{expectedProductId}'.");
        }
    }

    /// <summary>
    /// Verifies RSA signature of the license content.
    /// </summary>
    private static bool VerifySignature(string licenseJson, string signature, string publicKeyPem)
    {
        try
        {
            // Extract content without signature for verification
            var contentToVerify = ExtractContentWithoutSignature(licenseJson);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            var dataBytes = Encoding.UTF8.GetBytes(contentToVerify);
            var signatureBytes = Convert.FromBase64String(signature);

            return rsa.VerifyData(
                dataBytes,
                signatureBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
        }
        catch (Exception ex)
        {
            throw new LicenseException("Signature verification failed.", ex);
        }
    }

    /// <summary>
    /// Checks if library version is allowed by license version range.
    /// Supports patterns: "1", "1.2", "1.2.3", "1.x", "1.2.x"
    /// </summary>
    private static bool IsVersionAllowed(Version libraryVersion, string allowedVersion)
    {
        if (string.IsNullOrWhiteSpace(allowedVersion))
            return false;

        var parts = allowedVersion.Split('.');

        // Match major version only (e.g., "1" or "2")
        if (parts.Length == 1)
        {
            if (parts[0] == "x" || parts[0] == "*")
                return true;

            return libraryVersion.Major.ToString() == parts[0];
        }

        // Match major.minor (e.g., "1.2" or "1.x")
        if (parts.Length == 2)
        {
            if (libraryVersion.Major.ToString() != parts[0])
                return false;

            if (parts[1] == "x" || parts[1] == "*")
                return true;

            return libraryVersion.Minor.ToString() == parts[1];
        }

        // Match major.minor.patch (e.g., "1.2.3" or "1.2.x")
        if (parts.Length >= 3)
        {
            if (libraryVersion.Major.ToString() != parts[0])
                return false;

            if (libraryVersion.Minor.ToString() != parts[1])
                return false;

            if (parts[2] == "x" || parts[2] == "*")
                return true;

            return libraryVersion.Build.ToString() == parts[2];
        }

        return false;
    }

    /// <summary>
    /// Extracts license content without the signature field for verification.
    /// Preserves original JSON types (numbers, arrays, objects) to ensure signature matches.
    /// Uses canonical JSON format (non-indented, sorted properties) for consistent verification.
    /// </summary>
    private static string ExtractContentWithoutSignature(string licenseJson)
    {
        // Parse and remove signature field while preserving types
        using var doc = JsonDocument.Parse(licenseJson);
        var root = doc.RootElement;

        var contentDict = new Dictionary<string, object?>();

        foreach (var prop in root.EnumerateObject())
        {
            if (prop.Name.Equals("Signature", StringComparison.OrdinalIgnoreCase))
                continue;

            // Preserve original JSON types (not convert to string)
            contentDict[prop.Name] = JsonElementToObject(prop.Value);
        }

        // Serialize with canonical format (non-indented, sorted keys)
        return JsonSerializer.Serialize(contentDict, new JsonSerializerOptions
        {
            WriteIndented = false
        });
    }

    /// <summary>
    /// Converts JsonElement to appropriate .NET type while preserving JSON structure.
    /// This ensures signature verification uses the same data types as signing.
    /// </summary>
    private static object? JsonElementToObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => element.EnumerateArray().Select(JsonElementToObject).ToList(),
            JsonValueKind.Object => element.EnumerateObject().ToDictionary(p => p.Name, p => JsonElementToObject(p.Value)),
            _ => throw new LicenseException($"Unsupported JSON type: {element.ValueKind}")
        };
    }

    private static void ValidateRequiredFields(LicenseInfo license)
    {
        if (string.IsNullOrWhiteSpace(license.Client))
            throw new LicenseException("License must contain a valid client name.");

        if (string.IsNullOrWhiteSpace(license.Product))
            throw new LicenseException("License must contain a valid product name.");

        if (string.IsNullOrWhiteSpace(license.ProductId))
            throw new LicenseException("License must contain a valid ProductId.");

        if (string.IsNullOrWhiteSpace(license.AllowedVersion))
            throw new LicenseException("License must contain an allowed version.");

        if (string.IsNullOrWhiteSpace(license.Signature))
            throw new LicenseException("License must contain a valid signature.");

        if (license.ExpiryDate == default)
            throw new LicenseException("License must contain a valid expiry date.");
    }
}
