using Logix.Application.Interfaces.IServices.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        private const string EncryptionPassphrase = "Key@12!"; // Passphrase used to derive the encryption key
        private const string EncryptionKey = "Key@12!";
        private const int KeySizeInBytes = 32; // 256 bits key size

        public byte[] EncryptPassword(string data)
        {
            byte[] salt = GenerateSalt();

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = DeriveKeyFromPassphrase(EncryptionPassphrase, salt);
                aesAlg.GenerateIV();

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] encryptedData;

                    using (var msEncrypt = new System.IO.MemoryStream())
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                        encryptedData = msEncrypt.ToArray();
                    }

                    byte[] result = new byte[aesAlg.IV.Length + encryptedData.Length];
                    Array.Copy(aesAlg.IV, result, aesAlg.IV.Length);
                    Array.Copy(encryptedData, 0, result, aesAlg.IV.Length, encryptedData.Length);

                    return result;
                }
            }
        }

        public string DecryptPassword(byte[] encryptedData)
        {
            using (var aesAlg = Aes.Create())
            {
                byte[] iv = new byte[aesAlg.IV.Length];
                byte[] encryptedBytes = new byte[encryptedData.Length - aesAlg.IV.Length];

                Array.Copy(encryptedData, iv, aesAlg.IV.Length);
                Array.Copy(encryptedData, aesAlg.IV.Length, encryptedBytes, 0, encryptedBytes.Length);

                aesAlg.Key = DeriveKeyFromPassphrase(EncryptionPassphrase, iv);
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private byte[] DeriveKeyFromPassphrase(string passphrase, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(passphrase, salt, 10000))
            {
                return deriveBytes.GetBytes(KeySizeInBytes);
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // Salt size of 16 bytes
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

       
        //public string DecryptPassword(byte[] encryptedData)
        //{
        //    byte[] salt = GenerateSalt();

        //    using (var aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = DeriveKeyFromPassphrase(EncryptionPassphrase, salt);
        //        byte[] iv = new byte[aesAlg.BlockSize / 8];
        //        Array.Copy(encryptedData, iv, iv.Length);
        //        aesAlg.IV = iv;

        //        using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
        //        using (var msDecrypt = new System.IO.MemoryStream())
        //        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
        //        {
        //            byte[] encryptedContent = new byte[encryptedData.Length - iv.Length];
        //            Array.Copy(encryptedData, iv.Length, encryptedContent, 0, encryptedContent.Length);
        //            csDecrypt.Write(encryptedContent, 0, encryptedContent.Length);
        //            csDecrypt.FlushFinalBlock();

        //            byte[] decryptedBytes = msDecrypt.ToArray();
        //            return Encoding.UTF8.GetString(decryptedBytes);
        //        }
        //    }
        //}
    }
}
