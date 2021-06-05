/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.Hasher.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    /// <summary>
    /// Defines the <see cref="Hasher32" />.
    /// </summary>
    public static class Hasher32
    {
        #region Methods

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static unsafe Byte[] ComputeBytes(byte* ptr, int length, ulong seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(ptr, length, (uint)seed);
            }
            return b;
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b, pa = bytes)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(pa, bytes.Length, (uint)seed);
            }
            return b;
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static unsafe uint ComputeKey(byte* ptr, int length, ulong seed = 0)
        {
            return xxHash32.UnsafeComputeHash(ptr, length, (uint)seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static unsafe uint ComputeKey(byte[] bytes, ulong seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return xxHash32.UnsafeComputeHash(pa, bytes.Length, (uint)seed);
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="Hasher64" />.
    /// </summary>
    public static class Hasher64
    {
        #region Methods

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b)
            {
                *((ulong*)pb) = xxHash64.UnsafeComputeHash(bytes, length, seed);
            }
            return b;
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b, pa = bytes)
            {
                *((ulong*)pb) = xxHash64.UnsafeComputeHash(pa, bytes.Length, seed);
            }
            return b;
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeKey(byte* ptr, int length, ulong seed = 0)
        {
            return xxHash64.UnsafeComputeHash(ptr, length, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeKey(byte[] bytes, ulong seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return xxHash64.UnsafeComputeHash(pa, bytes.Length, seed);
            }
        }

        #endregion
    }
}
