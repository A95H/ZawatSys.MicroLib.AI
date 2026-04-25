namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Represents license information for a MicroLib AI.
/// Contains client details, version restrictions, expiry, and enabled features.
/// </summary>
public sealed class LicenseInfo
{
    /// <summary>
    /// Client/Company name that owns this license.
    /// </summary>
    public string Client { get; init; } = default!;

    /// <summary>
    /// Product name (e.g., "MicroLib.AI").
    /// </summary>
    public string Product { get; init; } = default!;

    /// <summary>
    /// Product ID that uniquely identifies the host system using this library.
    /// This binds the license to a specific application/service.
    /// Example: "Wasftkom.ProductService", "MyCompany.UserManagement"
    /// </summary>
    public string ProductId { get; init; } = default!;

    /// <summary>
    /// Allowed version range (e.g., "1.x" or "1.2.x" or "2").
    /// </summary>
    public string AllowedVersion { get; init; } = default!;

    /// <summary>
    /// License expiration date (UTC).
    /// </summary>
    public DateTimeOffset ExpiryDate { get; init; }

    /// <summary>
    /// License tier: "Lite", "Professional", "Enterprise"
    /// </summary>
    public string? Tier { get; init; }

    /// <summary>
    /// RSA/ECDSA signature of the license content.
    /// This prevents tampering.
    /// </summary>
    public string Signature { get; init; } = default!;

    /// <summary>
    /// Issue date of the license.
    /// </summary>
    public DateTimeOffset IssuedDate { get; init; }

    /// <summary>
    /// Optional: Machine/Tenant identifier for binding.
    /// </summary>
    public string? MachineId { get; init; }
}
