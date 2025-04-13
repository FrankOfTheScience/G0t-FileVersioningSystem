using System.Security.Cryptography;
using System.Text;

namespace G0tLib.Common;
public static class G0tHashing
{
    public static string HashObject(string content)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(content));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
