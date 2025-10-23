using System.Security.Cryptography;
using System.Text;

namespace Logix.MVC.Helpers
{
    public interface IEncryptionHelper
    {
        string Encrypt<T>(T value);
        T Decrypt<T>(string encryptedText);
    }

public static class EncryptionHelper
    {
       // private static readonly byte[] key = Encoding.UTF8.GetBytes("0123456789abndgftejbdvcghtkflmvht"); // Replace with your own encryption key
        private static readonly int KeySize = 256; // Specify the desired key size (128, 192, or 256)
        private static readonly byte[] key = GenerateKey();

        private static byte[] GenerateKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.GenerateKey();
                return aes.Key;
            }
        }
        public static string Encrypt<T>(T value)
        {
            string plainText = value.ToString();
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] encryptedBytes = null;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                byte[] combinedBytes = new byte[aes.IV.Length + encryptedBytes.Length];
                Array.Copy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
                Array.Copy(encryptedBytes, 0, combinedBytes, aes.IV.Length, encryptedBytes.Length);

                return Convert.ToBase64String(combinedBytes);
            }
        }

        public static T Decrypt<T>(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                byte[] iv = new byte[aes.IV.Length];
                byte[] encryptedData = new byte[encryptedBytes.Length - iv.Length];

                Array.Copy(encryptedBytes, iv, iv.Length);
                Array.Copy(encryptedBytes, iv.Length, encryptedData, 0, encryptedData.Length);

                aes.IV = iv;

                string decryptedText = null;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            decryptedText = reader.ReadToEnd();
                        }
                    }
                }

                return (T)Convert.ChangeType(decryptedText, typeof(T));
            }
        }
    }
}
