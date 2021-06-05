/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.xxHash64.Span.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="xxHash64" />.
    /// </summary>
    public static partial class xxHash64
    {
        #region Methods

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="data">The data<see cref="ReadOnlySpan{byte}"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeHash(ReadOnlySpan<byte> data, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="data">The data<see cref="Span{byte}"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static unsafe ulong ComputeHash(Span<byte> data, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }

        #endregion
    }
}
