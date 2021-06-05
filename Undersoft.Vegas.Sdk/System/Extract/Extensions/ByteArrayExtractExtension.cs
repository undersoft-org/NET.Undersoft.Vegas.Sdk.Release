/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.ByteArrayExtractExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    /// <summary>
    /// Defines the <see cref="ByteArrayExtractExtenstion" />.
    /// </summary>
    public static class ByteArrayExtractExtenstion
    {
        #region Methods

        /// <summary>
        /// The BlockEqual.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public unsafe static bool BlockEqual(this byte[] source, byte[] dest)
        {
            long sl = source.LongLength;
            if (sl > dest.LongLength)
                return false;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            fixed (byte* psrc = source, pdst = dest)
            {
                long* lsrc = (long*)psrc, ldst = (long*)pdst;
                for (int i = 0; i < sl64; i++)
                    if (*(&lsrc[i]) != (*(&ldst[i])))
                        return false;
                byte* psrc8 = psrc + (sl64 * 8), pdst8 = pdst + (sl64 * 8);
                for (int i = 0; i < sl8; i++)
                    if (*(&psrc8[i]) != (*(&pdst8[i])))
                        return false;
                return true;
            }
        }

        /// <summary>
        /// The BlockEqual.
        /// </summary>
        /// <param name="source">The source<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="long"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="long"/>.</param>
        /// <param name="count">The count<see cref="long"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public unsafe static bool BlockEqual(this IntPtr source, long srcOffset, IntPtr dest, long destOffset, long count)
        {
            long sl = count;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            long* lsrc = (long*)((byte*)source + srcOffset), ldst = (long*)((byte*)dest + destOffset);
            for (int i = 0; i < sl64; i++)
                if (*(&lsrc[i]) != (*(&ldst[i])))
                    return false;
            byte* psrc8 = (byte*)source + (sl64 * 8), pdst8 = (byte*)dest + (sl64 * 8);
            for (int i = 0; i < sl8; i++)
                if (*(&psrc8[i]) != (*(&pdst8[i])))
                    return false;
            return true;
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, uint count)
        {
            Extractor.CopyBlock(src, dest, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, uint offset, uint count)
        {
            Extractor.CopyBlock(src, dest, offset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, ulong count)
        {
            Extractor.CopyBlock(src, dest, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(this byte[] src, byte[] dest, ulong offset, ulong count)
        {
            Extractor.CopyBlock(src, dest, offset, count);
        }

        /// <summary>
        /// The NewStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="Type"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object NewStructure(this byte[] binary, Type structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, structure, offset);
        }

        /// <summary>
        /// The ToInt32.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public unsafe static int ToInt32(this Byte[] bytes)
        {
            int v = 0;
            uint l = (uint)bytes.Length;
            fixed (byte* pbyte = bytes)
            {
                if (l < 4)
                {
                    byte* a = stackalloc byte[4];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((int*)a);

                }
                v = *((int*)pbyte);
            }
            return v;
        }

        /// <summary>
        /// The ToInt64.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public unsafe static long ToInt64(this Byte[] bytes)
        {
            long v = 0;
            uint l = (uint)bytes.Length;
            fixed (byte* pbyte = bytes)
            {
                if (l < 8)
                {
                    byte* a = stackalloc byte[8];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((long*)a);

                }
                v = *((long*)pbyte);
            }
            return v;
        }

        /// <summary>
        /// The ToStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object ToStructure(this byte[] binary, object structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, structure, offset);
        }

        /// <summary>
        /// The ToStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <returns>The <see cref="ValueType"/>.</returns>
        public unsafe static ValueType ToStructure(this byte[] binary, ValueType structure, long offset = 0)
        {
            return Extractor.BytesToStructure(binary, ref structure, offset);
        }

        /// <summary>
        /// The ToUInt32.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public unsafe static uint ToUInt32(this Byte[] bytes)
        {
            uint v = 0;
            uint l = (uint)bytes.Length;
            fixed (byte* pbyte = bytes)
            {
                if (l < 4)
                {
                    byte* a = stackalloc byte[4];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((uint*)a);

                }
                v = *((uint*)pbyte);
            }
            return v;
        }

        /// <summary>
        /// The ToUInt64.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public unsafe static ulong ToUInt64(this Byte[] bytes)
        {
            ulong v = 0;
            uint l = (uint)bytes.Length;
            fixed (byte* pbyte = bytes)
            {
                if (l < 8)
                {
                    byte* a = stackalloc byte[8];
                    Extractor.CopyBlock(a, pbyte, l);
                    v = *((ulong*)a);

                }
                v = *((ulong*)pbyte);
            }
            return v;
        }

        #endregion
    }
}
