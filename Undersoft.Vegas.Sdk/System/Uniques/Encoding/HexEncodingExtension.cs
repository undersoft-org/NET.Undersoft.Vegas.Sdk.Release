/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.HexEncodingExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Text;

    /// <summary>
    /// Defines the <see cref="HexEncodingExtension" />.
    /// </summary>
    public static class HexEncodingExtension
    {
        #region Methods

        /// <summary>
        /// The FromHex.
        /// </summary>
        /// <param name="hex">The hex<see cref="String"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] FromHex(this String hex)
        {
            return hexToByte(hex);
        }

        /// <summary>
        /// The ToHex.
        /// </summary>
        /// <param name="ba">The ba<see cref="Byte[]"/>.</param>
        /// <param name="trim">The trim<see cref="bool"/>.</param>
        /// <returns>The <see cref="String"/>.</returns>
        public static String ToHex(this Byte[] ba, bool trim = false)
        {
            return byteToHex(ba, trim);
        }

        /// <summary>
        /// The byteToHex.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <param name="trim">The trim<see cref="bool"/>.</param>
        /// <returns>The <see cref="String"/>.</returns>
        private static String byteToHex(Byte[] bytes, bool trim = false)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            if (trim)
            {
                foreach (byte b in bytes)
                    if (b == 0)
                        length--;
                    else break;
            }
            for (int i = 0; i < length; i++)
                s.Append(bytes[i].ToString("x2").ToUpper());
            return s.ToString();
        }

        /// <summary>
        /// The getHex.
        /// </summary>
        /// <param name="x">The x<see cref="Char"/>.</param>
        /// <returns>The <see cref="Byte"/>.</returns>
        private static Byte getHex(Char x)
        {
            if (x <= '9' && x >= '0')
            {
                return (byte)(x - '0');
            }
            else if (x <= 'z' && x >= 'a')
            {
                return (byte)(x - 'a' + 10);
            }
            else if (x <= 'Z' && x >= 'A')
            {
                return (byte)(x - 'A' + 10);
            }
            return 0;
        }

        /// <summary>
        /// The hexToByte.
        /// </summary>
        /// <param name="hex">The hex<see cref="String"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        private static Byte[] hexToByte(String hex, int length = -1)
        {
            if (length < 0)
                length = hex.Length;
            byte[] r = new byte[length / 2];
            for (int i = 0; i < length - 1; i += 2)
            {
                byte a = getHex(hex[i]);
                byte b = getHex(hex[i + 1]);
                r[i / 2] = (byte)(a * 16 + b);
            }
            return r;
        }

        #endregion
    }
}
