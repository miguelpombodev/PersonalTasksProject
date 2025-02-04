using System.Security.Cryptography;

namespace PersonalTasksProject.Providers;

public static class DataEncryptionProvider
{
    private static readonly int SaltSize = 128 / 8;
    private static readonly int KeySize = 256 / 8;
    private static readonly int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';



    public static string Hash(string toBeEncryptedText)
    {
        return BCrypt.Net.BCrypt.HashPassword(toBeEncryptedText, workFactor: 12);
    }

    public static bool Verify(string encryptedText, string rawText)
    {
        return BCrypt.Net.BCrypt.Verify(rawText,encryptedText);
    }

    // public static string Hash(string toBeEncryptedText)
    // {
    //     var salt = RandomNumberGenerator.GetBytes(SaltSize);
    //     var hash = Rfc2898DeriveBytes.Pbkdf2(toBeEncryptedText, salt, Iterations, HashAlgorithmName, KeySize);
    //
    //     return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    // }
    //
    // public static bool Verify(string encryptedText, string rawText)
    // {
    //     var elements = encryptedText.Split(Delimiter);
    //     var salt = Convert.FromBase64String(elements[0]);
    //     var hash = Convert.FromBase64String(elements[1]);
    //
    //     var hashInput = Rfc2898DeriveBytes.Pbkdf2(rawText, salt, Iterations, HashAlgorithmName, KeySize);
    //
    //     return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    // }
}