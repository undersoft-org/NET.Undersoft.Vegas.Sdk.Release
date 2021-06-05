/*************************************************
   Copyright (c) 2021 Undersoft

   System.CharsEncodingExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Text;

    #region Enums

    public enum CharEncoding
    {
        ASCII,
        UTF8,
        Unicode
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="CharsEncodingExtension" />.
    /// </summary>
    public static class CharsEncodingExtension
    {
        #region Methods

        /// <summary>
        /// The ToBytes.
        /// </summary>
        /// <param name="ca">The ca<see cref="Char"/>.</param>
        /// <param name="tf">The tf<see cref="CharEncoding"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] ToBytes(this Char ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(new char[] { ca });
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(new char[] { ca });
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(new char[] { ca });
                default:
                    return Encoding.ASCII.GetBytes(new char[] { ca });
            }
        }

        /// <summary>
        /// The ToBytes.
        /// </summary>
        /// <param name="ca">The ca<see cref="Char[]"/>.</param>
        /// <param name="tf">The tf<see cref="CharEncoding"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] ToBytes(this Char[] ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(ca);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(ca);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(ca);
                default:
                    return Encoding.ASCII.GetBytes(ca);
            }
        }

        /// <summary>
        /// The ToBytes.
        /// </summary>
        /// <param name="ca">The ca<see cref="String"/>.</param>
        /// <param name="tf">The tf<see cref="CharEncoding"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] ToBytes(this String ca, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(ca);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(ca);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(ca);
                default:
                    return Encoding.ASCII.GetBytes(ca);
            }
        }

        /// <summary>
        /// The ToChars.
        /// </summary>
        /// <param name="ba">The ba<see cref="Byte"/>.</param>
        /// <param name="tf">The tf<see cref="CharEncoding"/>.</param>
        /// <returns>The <see cref="Char[]"/>.</returns>
        public static Char[] ToChars(this Byte ba, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetChars(new byte[] { ba });
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetChars(new byte[] { ba });
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetChars(new byte[] { ba });
                default:
                    return Encoding.ASCII.GetChars(new byte[] { ba });
            }
        }

        /// <summary>
        /// The ToChars.
        /// </summary>
        /// <param name="ba">The ba<see cref="Byte[]"/>.</param>
        /// <param name="tf">The tf<see cref="CharEncoding"/>.</param>
        /// <returns>The <see cref="Char[]"/>.</returns>
        public static Char[] ToChars(this Byte[] ba, CharEncoding tf = CharEncoding.ASCII)
        {
            switch (tf)
            {
                case CharEncoding.ASCII:
                    return Encoding.ASCII.GetChars(ba);
                case CharEncoding.UTF8:
                    return Encoding.UTF8.GetChars(ba);
                case CharEncoding.Unicode:
                    return Encoding.Unicode.GetChars(ba);
                default:
                    return Encoding.ASCII.GetChars(ba);
            }
        }

        #endregion
    }
}
