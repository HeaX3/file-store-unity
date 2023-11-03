using System;
using System.IO;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace FileStore
{
    public static class FileEncryption
    {
        private static byte[] Key;
        private static byte[] IV;

        /// <summary>
        /// Use the key generator at Tools > FileStore > Generate... in the unity editor to create these values
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static void Initialize(string key, string iv)
        {
            Key = Convert.FromBase64String(key);
            IV = Convert.FromBase64String(iv);
        }

        public static byte[] Encrypt([NotNull] string input) =>
            EncryptStringToBytes(input, Key, IV);

        public static byte[] Encrypt([NotNull] byte[] input) =>
            EncryptStringToBytes(Convert.ToBase64String(input), Key, IV);

        public static string DecryptText([NotNull] byte[] input) =>
            DecryptStringFromBytes(input, Key, IV);

        public static byte[] DecryptBytes([NotNull] byte[] input) =>
            Convert.FromBase64String(DecryptStringFromBytes(input, Key, IV));

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            if (Key == null || IV == null)
            {
                throw new Exception(
                    "Initialize the FileEncryption before using it by calling FileEncryption.Initialize(key:string, iv:string)"
                );
            }

            if (plainText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(plainText));
            if (Key is not { Length: > 0 })
                throw new ArgumentNullException(nameof(Key));
            if (IV is not { Length: > 0 })
                throw new ArgumentNullException(nameof(IV));

            using var rijAlg = new RijndaelManaged()
            {
                Key = Key,
                IV = IV
            };

            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return msEncrypt.ToArray();
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(cipherText));
            if (Key is not { Length: > 0 })
                throw new ArgumentNullException(nameof(Key));
            if (IV is not { Length: > 0 })
                throw new ArgumentNullException(nameof(IV));

            using var rijAlg = new RijndaelManaged();
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}