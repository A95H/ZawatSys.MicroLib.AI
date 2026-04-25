namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Manages license state for the application.
/// Should be registered as a singleton in DI container.
/// </summary>
public sealed class LicenseManager
{
    private LicenseInfo? _currentLicense;
    private readonly object _lock = new();

    /// <summary>
    /// Gets the currently validated license.
    /// </summary>
    public LicenseInfo CurrentLicense
    {
        get
        {
            lock (_lock)
            {
                return _currentLicense
                    ?? throw new LicenseException("License has not been validated. Call Initialize() first.");
            }
        }
    }

    /// <summary>
    /// Indicates whether a license has been successfully validated.
    /// </summary>
    public bool IsInitialized
    {
        get
        {
            lock (_lock)
            {
                return _currentLicense != null;
            }
        }
    }

    /// <summary>
    /// Initializes the license manager with a validated license.
    /// </summary>
    public void Initialize(LicenseInfo license)
    {
        if (license == null)
            throw new ArgumentNullException(nameof(license));

        lock (_lock)
        {
            _currentLicense = license;
        }
    }

    /// <summary>
    /// Checks if license allows a specific tier.
    /// </summary>
    public bool IsTier(string tier)
    {
        return CurrentLicense.Tier?.Equals(tier, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    /// <summary>
    /// Gets remaining days until license expires.
    /// </summary>
    public int GetRemainingDays()
    {
        var remaining = CurrentLicense.ExpiryDate - DateTime.UtcNow;
        return Math.Max(0, (int)remaining.TotalDays);
    }

    /// <summary>
    /// Checks if license is about to expire (within specified days).
    /// </summary>
    public bool IsExpiringWithin(int days)
    {
        return GetRemainingDays() <= days;
    }
}
