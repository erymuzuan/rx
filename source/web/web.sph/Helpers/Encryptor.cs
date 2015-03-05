using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebSph.Helpers
{

    public class Encryptor
    {
        public string DecryptFile(string fileName)
        {
            var cipherText = File.ReadAllText(fileName);
            var cipherBytes = Convert.FromBase64String(cipherText);
            var pdb = new Rfc2898DeriveBytes(Secret, Salt);
            var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            var text = Encoding.Unicode.GetString(decryptedData);
            return text.Replace("utf-16", "utf-8");
        }

        public string Decrypt(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            var pdb = new Rfc2898DeriveBytes(Secret, Salt);
            var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            var text = Encoding.Unicode.GetString(decryptedData);
            return text.Replace("utf-16", "utf-8");
        }

        public byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();
                alg.Key = key;
                alg.IV = iv;
                using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherData, 0, cipherData.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public string Encrypt(string clearText)
        {
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            var pdb = new Rfc2898DeriveBytes(Secret, Salt);
            var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }


        public byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();
                alg.Key = key;
                alg.IV = iv;
                using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearData, 0, clearData.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }


        private static string Secret
        {
            get
            {
                return ConfigurationManager.AppSettings["sph:TokenSecretKey"] ?? "9D168FB5-6B58-4D43-8C52-11D6909E9638";
            }
        }
        public static readonly byte[] Salt = { 0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64, 0x03, 0x76 };
    }
}