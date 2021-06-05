/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.xxHash64.Array.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="xxHash64" />.
    /// </summary>
    public static partial class xxHash64
    {
        #region Methods

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeHash(byte[] data, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &data[0])
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeHash(byte[] data, int offset, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(offset < data.Length);
            Debug.Assert(length <= data.Length - offset);

            fixed (byte* pData = &data[0 + offset])
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="data">The data<see cref="System.ArraySegment{byte}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeHash(System.ArraySegment<byte> data, ulong seed = 0)
        {
            Debug.Assert(data != null);

            return ComputeHash(data.Array, data.Offset, data.Count, seed);
        }

        #endregion
    }
}
