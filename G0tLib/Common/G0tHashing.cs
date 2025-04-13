using System.Security.Cryptography;
using System.Text;

namespace G0tLib.Common;
public static class G0tHashing
{
    public static string HashObject(string content)
    {
        using var sha1 = SHA256.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
