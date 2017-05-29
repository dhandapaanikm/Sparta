using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Interfaces;

namespace Sparta.Infrastructure.Encryption
{
    public class EncryptionClient : IEncryptionClient
    {
        private const string KEY = "Gabriel's Secure key";
        private const int SALT_SIZE = 32;

        #region Public Methods
        /// <summary>
        /// Decrypt an encrypted string.
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                throw new ArgumentNullException("ciphertext");
            }

            var allTheBytes = Convert.FromBase64String(ciphertext);
            var saltBytes = allTheBytes.Take(SALT_SIZE).ToArray();
            var ciphertextBytes = allTheBytes.Skip(SALT_SIZE).Take(allTheBytes.Length - SALT_SIZE).ToArray();

            using (var keyDerivationFunction = new Rfc2898DeriveBytes(KEY, saltBytes))
            {
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);

                return DecryptWithAES(ciphertextBytes, keyBytes, ivBytes);
            }
        }

        /// <summary>
        /// Encrypt and plain text string.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            using (var keyDerivationFunction = new Rfc2898DeriveBytes(KEY, SALT_SIZE))
            {
                var saltBytes = keyDerivationFunction.Salt;
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);

                using (var aesManaged = new AesManaged())
                {
                    aesManaged.KeySize = 256;

                    using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
                    {
                        MemoryStream memoryStream = null;
                        CryptoStream cryptoStream = null;

                        return WriteMemoryStream(plainText, ref saltBytes, encryptor, ref memoryStream, ref cryptoStream);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private static string WriteMemoryStream(string plainText, ref byte[] saltBytes, ICryptoTransform encryptor,
                                                ref MemoryStream memoryStream, ref CryptoStream cryptoStream)
        {
            try
            {
                memoryStream = new MemoryStream();

                try
                {
                    cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }
                finally
                {
                    if (cryptoStream != null)
                    {
                        cryptoStream.Dispose();
                    }
                }

                var cipherTextBytes = memoryStream.ToArray();
                Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
                Array.Copy(cipherTextBytes, 0, saltBytes, SALT_SIZE, cipherTextBytes.Length);

                return Convert.ToBase64String(saltBytes);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }
        }

        private static string DecryptWithAES(byte[] ciphertextBytes, byte[] keyBytes, byte[] ivBytes)
        {
            var decryptedString = string.Empty;

            using (var aesManaged = new AesManaged())
            {
                using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (var memoryStream = new MemoryStream(ciphertextBytes))
                    {
                        StreamReader streamReader;

                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            streamReader = new StreamReader(cryptoStream);

                            decryptedString = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return decryptedString;
        }
        #endregion
    }
}