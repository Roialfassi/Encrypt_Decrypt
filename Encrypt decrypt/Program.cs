using System;
using System.Security.Cryptography;


namespace Encrypt_decrypt
{
    class Program
    {
        public static (string publicKey, string privateKey) GenerateKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096))
            {
                return (rsa.ToXmlString(false), rsa.ToXmlString(true));
            }
        }

        public static byte[] Encrypt(string publicKey, string data)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] bytesData = System.Text.Encoding.UTF8.GetBytes(data);
                return rsa.Encrypt(bytesData, false);
            }
        }
        public static string Decrypt(string privateKey, byte[] encryptedData)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] bytesDecrypted = rsa.Decrypt(encryptedData, false);
                return System.Text.Encoding.UTF8.GetString(bytesDecrypted);
            }
        }
        static void Main(string[] args)
        {
            var (publicKey, privateKey) = GenerateKeys();

            string originalMessage =  "{\r\n  \"Date\": \"2023-08-15T13:32:32.8686962+03:00\",\r\n  \"Content\": {\r\n    \"Type\": \"Alarm\",\r\n    \"Active\": true}}";
            Console.WriteLine($"Original: {originalMessage}");

            byte[] encryptedMessage = Encrypt(publicKey, originalMessage);
            Console.WriteLine($"Encrypted: {Convert.ToBase64String(encryptedMessage)}");

            string decryptedMessage = Decrypt(privateKey, encryptedMessage);
            Console.WriteLine($"Decrypted: {decryptedMessage}");
        }

    }
}
