﻿/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.xxHash32.Array.cs
   
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
    /// Defines the <see cref="xxHash32" />.
    /// </summary>
    public static partial class xxHash32
    {
        #region Methods

        /// <summary>
        /// Compute xxHash for the data byte array.
        /// </summary>
        /// <param name="data">The source of data.</param>
        /// <param name="seed">The seed number.</param>
        /// <returns>hash.</returns>
        public static unsafe ulong ComputeHash(ArraySegment<byte> data, uint seed = 0)
        {
            Debug.Assert(data != null);

            return ComputeHash(data.Array, data.Offset, data.Count, seed);
        }

        /// <summary>
        /// Compute xxHash for the data byte array.
        /// </summary>
        /// <param name="data">The source of data.</param>
        /// <param name="length">The length of the data for hashing.</param>
        /// <param name="seed">The seed number.</param>
        /// <returns>hash.</returns>
        public static unsafe uint ComputeHash(byte[] data, int length, uint seed = 0)
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
        /// Compute xxHash for the data byte array.
        /// </summary>
        /// <param name="data">The source of data.</param>
        /// <param name="offset">The offset of the data for hashing.</param>
        /// <param name="length">The length of the data for hashing.</param>
        /// <param name="seed">The seed number.</param>
        /// <returns>hash.</returns>
        public static unsafe uint ComputeHash(byte[] data, int offset, int length, uint seed = 0)
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

        #endregion
    }
}
