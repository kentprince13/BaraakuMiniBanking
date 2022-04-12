using System.Security.Cryptography;
using System.Text;

namespace MiniBanking.Domain.Utilities;

public static class CommonHelper
{
    public static T ParseEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    
    public static string Encrypt(string source, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();

        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Encoding.UTF8.GetBytes(source);

        var encoded =
            Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return encoded;
    }
    public static string Decrypt(string encodedText, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();

        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Convert.FromBase64String(encodedText);

        var plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return plaintext;
    }
}