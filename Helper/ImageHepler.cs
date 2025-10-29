using System.Security.Cryptography;

namespace TaskManagment.Helper;

public static class ImageHepler
{
    public static string ComputeHash(byte[] imageData)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(imageData);
        return Convert.ToBase64String(hash);
    }
}
