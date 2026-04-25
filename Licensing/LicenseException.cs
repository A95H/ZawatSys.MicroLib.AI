namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Exception thrown when license validation fails.
/// </summary>
public sealed class LicenseException : Exception
{
    public LicenseException(string message) : base(message)
    {
    }

    public LicenseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
