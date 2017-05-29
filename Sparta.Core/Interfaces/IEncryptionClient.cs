using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta.Core.Interfaces
{
    /// <summary>
    /// Interface for defining encryption routines.
    /// </summary>
    public interface IEncryptionClient
    {
        /// <summary>
        /// Decrypt an encrypted string.
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        string Decrypt(string ciphertext);

        /// <summary>
        /// Encrypt and plain text string.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        string Encrypt(string plainText);
    }
}