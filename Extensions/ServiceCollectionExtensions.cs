using Microsoft.Extensions.DependencyInjection;
using ZawatSys.MicroLib.AI.Licensing;

namespace ZawatSys.MicroLib.AI.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register AI MicroLib services.
/// Simplifies dependency injection setup in host services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all AI MicroLib services with license validation.
    /// REQUIRED: You must provide a valid license key to use this library.
    /// The license must be bound to the ProductId embedded in this library.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="licenseFilePath">Path to the license file</param>
    /// <returns>The service collection for chaining.</returns>
    /// <example>
    /// <code>
    /// // In your Program.cs or Startup.cs:
    /// services.AddAIMicroLibWithLicense("path/to/license.json");
    /// </code>
    /// </example>
    /// <exception cref="LicenseException">Thrown when license is invalid, expired, or ProductId mismatch</exception>
    public static IServiceCollection AddAIMicroLibWithLicense(
        this IServiceCollection services,
        string licenseFilePath)
    {
        // Create LicenseManager instance for validation (fail-fast approach)
        var licenseManager = new LicenseManager();

        // Initialize and validate license using embedded public key
        // This throws LicenseException immediately if invalid (before registering services)
        var license = LicenseInitializer.InitializeFromFile(
            licenseFilePath,
            ZawatSysMetadata.ZawatSysPublicKey.PublicKeyPem,  // ← Embedded in library
            licenseManager);

        // Validate product binding against embedded ProductId (prevents user bypass)
        LicenseValidator.ValidateProductBinding(license, ZawatSysMetadata.ProductId);

        // Mark library as properly licensed
        LicenseEnforcement.MarkAsInitialized();

        return services.AddAIMicroLibInternal(licenseManager);
    }

    /// <summary>
    /// Registers all AI MicroLib services with license from JSON content.
    /// REQUIRED: You must provide a valid license to use this library.
    /// The license must be bound to the ProductId embedded in this library.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="licenseJson">License JSON content</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="LicenseException">Thrown when license is invalid, expired, or ProductId mismatch</exception>
    public static IServiceCollection AddAIMicroLibWithLicenseContent(
        this IServiceCollection services,
        string licenseJson)
    {
        // Create LicenseManager instance for validation (fail-fast approach)
        var licenseManager = new LicenseManager();

        // Initialize and validate license using embedded public key
        // This throws LicenseException immediately if invalid (before registering services)
        var license = LicenseInitializer.InitializeFromContent(
            licenseJson,
            ZawatSysMetadata.ZawatSysPublicKey.PublicKeyPem,  // ← Embedded in library
            licenseManager);

        // Validate product binding against embedded ProductId (prevents user bypass)
        LicenseValidator.ValidateProductBinding(license, ZawatSysMetadata.ProductId);

        // Mark library as properly licensed
        LicenseEnforcement.MarkAsInitialized();

        return services.AddAIMicroLibInternal(licenseManager);
    }

    /// <summary>
    /// Registers AI microlib services after successful license validation.
    /// The AI microlib currently exposes domain contracts and entities, so the
    /// runtime registration surface is the validated <see cref="LicenseManager"/>.
    /// </summary>
    private static IServiceCollection AddAIMicroLibInternal(
        this IServiceCollection services,
        LicenseManager licenseManager)
    {
        services.AddSingleton(licenseManager);

        return services;
    }

    /// <summary>
    /// Allows host applications to append AI-specific registrations after the
    /// microlib license has been validated.
    /// </summary>
    public static IServiceCollection CustomizeAIServices(
        this IServiceCollection services,
        Action<IServiceCollection> customRegistration)
    {
        customRegistration(services);
        return services;
    }
}
