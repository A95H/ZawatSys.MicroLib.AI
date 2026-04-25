namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Static enforcement point for library licensing.
/// Prevents library usage without valid license initialization.
/// </summary>
internal static class LicenseEnforcement
{
    private static bool _isInitialized = false;
    private static readonly object _lock = new();

    /// <summary>
    /// Marks the license as initialized and validated.
    /// Should only be called by ServiceCollectionExtensions after successful validation.
    /// </summary>
    internal static void MarkAsInitialized()
    {
        lock (_lock)
        {
            _isInitialized = true;
        }
    }

    /// <summary>
    /// Ensures that a valid license has been initialized.
    /// Throws LicenseException if not initialized.
    /// </summary>
    /// <exception cref="LicenseException">Thrown when license has not been initialized</exception>
    internal static void EnsureInitialized()
    {
        lock (_lock)
        {
            if (!_isInitialized)
            {
                var libraryVersion = typeof(LicenseEnforcement).Assembly.GetName().Version?.ToString() ?? "Unknown";
                throw new LicenseException(
                    $"ZawatSys.MicroLib.AI v{libraryVersion} requires a valid license.\n" +
                    "Product: MicroLib.AI\n" +
                    "\nInitialization Required:\n" +
                    "  services.AddAIMicroLibWithLicense(\"license.json\");\n" +
                    "\nDocumentation: See Licensing/README.md\n" +
                    "Support: Contact ZawatSys for license assistance");
            }
        }
    }

    /// <summary>
    /// Checks if license has been initialized.
    /// </summary>
    internal static bool IsInitialized
    {
        get
        {
            lock (_lock)
            {
                return _isInitialized;
            }
        }
    }

    /// <summary>
    /// Resets initialization state (for testing purposes only).
    /// </summary>
    internal static void Reset()
    {
        lock (_lock)
        {
            _isInitialized = false;
        }
    }
}
