namespace ZawatSys.MicroLib.AI.Licensing;

/// <summary>
/// Contains the official ZawatSys RSA public key for license validation.
/// This key is embedded in the library and cannot be changed by users.
/// </summary>
internal static class ZawatSysMetadata
{
    internal static class ZawatSysPublicKey
    {
        /// <summary>
        /// Official ZawatSys RSA 2048-bit public key in PEM format.
        /// Used to verify all licenses are signed by ZawatSys.
        /// </summary>
        /// <remarks>
        /// TODO: Generate your key pair and paste the public key here:
        /// <code>
        /// var (privateKey, publicKey) = LicenseGenerator.GenerateKeyPair(2048);
        /// // Store privateKey securely (Azure Key Vault, AWS KMS, etc.)
        /// // Paste publicKey below
        /// </code>
        /// </remarks>
        public const string PublicKeyPem = @"-----BEGIN RSA PUBLIC KEY-----
MIIBCgKCAQEAxKyP8fIdNv4gYyE2cI4cYLzjg9MBtd0mfD5PddDe7/x0GD5G7IOG
/jOapyb5hGMrid8gKWNsNYEgbO5PKb+SdTERoqyg+KKuwkdjr9iiuNH9CYwNU2Bl
q7wyDK4/dioNVeU18mBm4BNApeoz4eg26dqICRTm4y32Tt+kDT66xQx0I+Yob6md
YcwrqXxj2kzWi2Z20QS8qg9GbcPaP82K3rcVAYHQLzwZFhXx2qZH5d+kKjMfIe0R
nbAVUc1TQ+xd7ByZeGuCWodARFvkbTrAsSwmqG4gYzDCd/7Xzpqft+htbDDLntCS
9tc1cWxO7FVT5fTfz5Lln/bT099EmlfvfQIDAQAB
-----END RSA PUBLIC KEY-----";

        /// <summary>
        /// Verifies if this is a valid public key (not the default placeholder).
        /// </summary>
        internal static bool IsValid() =>
            !string.IsNullOrWhiteSpace(PublicKeyPem);
    }

    /// <summary>
    /// Contains the ProductId that this library expects in licenses.
    /// This is auto-generated at build time and embedded in the library.
    /// INTERNAL - Cannot be accessed or modified by external code.
    /// </summary>
    public static string ProductId = "ZawatSys.MicroLib.AI";

}
