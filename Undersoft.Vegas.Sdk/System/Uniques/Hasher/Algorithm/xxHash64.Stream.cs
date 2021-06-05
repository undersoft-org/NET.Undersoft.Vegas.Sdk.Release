/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.xxHash64.Stream.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Buffers;
    using System.Diagnostics;
    using System.Extract;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="xxHash64" />.
    /// </summary>
    public static partial class xxHash64
    {
        #region Methods

        /// <summary>
        /// The ComputeHash.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="bufferSize">The bufferSize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 32);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 32);

            int readBytes;
            int offset = 0;
            long length = 0;

            ulong v1 = seed + p1 + p2;
            ulong v2 = seed + p2;
            ulong v3 = seed + 0;
            ulong v4 = seed - p1;

            try
            {
                while ((readBytes = stream.Read(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 32) continue;

                    int r = offset % 32;
                    int l = offset - r;

                    UnsafeAlign(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    Extractor.CopyBlock(buffer, 0, buffer, l, r);
                    offset = r;
                }

                ulong h64 = UnsafeFinal(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h64;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        #endregion
    }
}
