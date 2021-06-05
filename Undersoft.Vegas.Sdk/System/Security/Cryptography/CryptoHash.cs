/*************************************************
   Copyright (c) 2021 Undersoft

   System.CryptoHash.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="CryptoHashKey" />.
    /// </summary>
    public static class CryptoHashKey
    {
        #region Fields

        /// <summary>
        /// Defines the alghoritm.
        /// </summary>
        public static string alghoritm = "HMACSHA256";

        #endregion

        #region Methods

        /// <summary>
        /// The Encrypt.
        /// </summary>
        /// <param name="pass">The pass<see cref="string"/>.</param>
        /// <param name="format">The format<see cref="int"/>.</param>
        /// <param name="salt">The salt<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Encrypt(string pass, int format, string salt)
        {
            if (format == 0)
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            if (format == 1)
            {
                HMACSHA512 hm = new HMACSHA512();

                if (hm is KeyedHashAlgorithm)
                {
                    KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                    if (kha.Key.Length == bSalt.Length)
                    {
                        kha.Key = bSalt;
                    }
                    else if (kha.Key.Length < bSalt.Length)
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                        kha.Key = bKey;
                    }
                    else
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        for (int iter = 0; iter < bKey.Length;)
                        {
                            int len = Math.Min(bSalt.Length, bKey.Length - iter);
                            Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                            iter += len;
                        }
                        kha.Key = bKey;
                    }
                    bRet = kha.ComputeHash(bIn);
                }
                else
                {
                    byte[] bAll = new byte[bSalt.Length + bIn.Length];
                    Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                    Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                    bRet = hm.ComputeHash(bAll);
                }
            }
            return Convert.ToBase64String(bRet);
        }

        /// <summary>
        /// The Salt.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Salt()
        {
            return Convert.ToBase64String(DateTime.Now.ToLongDateString().ToBytes(CharEncoding.ASCII));
        }

        /// <summary>
        /// The Verify.
        /// </summary>
        /// <param name="hashedPassword">The hashedPassword<see cref="string"/>.</param>
        /// <param name="hashedSalt">The hashedSalt<see cref="string"/>.</param>
        /// <param name="providedPassword">The providedPassword<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool Verify(string hashedPassword, string hashedSalt, string providedPassword)
        {
            string passwordHash = hashedPassword;
            const int format = 1;
            string salt = hashedSalt;
            if (String.Equals(Encrypt(providedPassword, format, salt), passwordHash, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        #endregion
    }
}
