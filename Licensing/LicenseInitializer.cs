using System.Reflection;

namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Helper class for initializing and validating licenses on application startup.
/// </summary>
public static class LicenseInitializer
{
    /// <summary>
    /// Initializes and validates license from file path.
    /// </summary>
    /// <param name="licenseFilePath">Path to license.json file</param>
    /// <param name="publicKeyPem">RSA public key in PEM format</param>
    /// <param name="licenseManager">License manager instance</param>
    /// <returns>Validated license info</returns>
    public static LicenseInfo InitializeFromFile(
        string licenseFilePath,
        string publicKeyPem,
        LicenseManager licenseManager)
    {
        if (!File.Exists(licenseFilePath))
            throw new LicenseException($"License file not found at: {licenseFilePath}");

        var licenseJson = File.ReadAllText(licenseFilePath);
        return InitializeFromContent(licenseJson, publicKeyPem, licenseManager);
    }

    /// <summary>
    /// Initializes and validates license from JSON content.
    /// </summary>
    public static LicenseInfo InitializeFromContent(
        string licenseJson,
        string publicKeyPem,
        LicenseManager licenseManager)
    {
        // Get library version
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version
            ?? throw new LicenseException("Unable to determine library version.");

        // Validate license with version check
        var license = LicenseValidator.ValidateWithVersion(licenseJson, publicKeyPem, version);

        // Initialize license manager
        licenseManager.Initialize(license);

        return license;
    }

    /// <summary>
    /// Initializes license from embedded resource.
    /// Useful for development/testing scenarios.
    /// </summary>
    public static LicenseInfo InitializeFromEmbeddedResource(
        string resourceName,
        string publicKeyPem,
        LicenseManager licenseManager)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new LicenseException($"Embedded license resource '{resourceName}' not found.");

        using var reader = new StreamReader(stream);
        var licenseJson = reader.ReadToEnd();

        return InitializeFromContent(licenseJson, publicKeyPem, licenseManager);
    }

    /// <summary>
    /// Gets the current library version.
    /// </summary>
    public static Version GetLibraryVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetName().Version ?? new Version(1, 0, 0);
    }

    /// <summary>
    /// Gets library version as string.
    /// </summary>
    public static string GetLibraryVersionString()
    {
        var version = GetLibraryVersion();
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }
}
