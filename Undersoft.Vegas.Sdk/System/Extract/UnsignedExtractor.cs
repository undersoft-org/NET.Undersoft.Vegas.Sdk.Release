/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.UnsignedExtractor.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    /// <summary>
    /// Defines the <see cref="Extractor" />.
    /// </summary>
    public static partial class Extractor
    {
        #region Methods

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte* dest, byte* src, uint count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte* dest, byte* src, uint destOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte* dest, byte* src, ulong count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte* dest, byte* src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte* dest, uint destOffset, byte* src, uint srcOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte* dest, ulong destOffset, byte* src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, byte[] src, ulong count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, byte[] src, uint count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, byte[] src, uint destOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, byte[] src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, uint destOffset, byte[] src, uint srcOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, ulong destOffset, byte[] src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, uint count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, uint destOffset, uint count)
        {
            extractor.CopyBlock((byte*)src.ToPointer(), destOffset, (byte*)dest.ToPointer(), 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, uint destOffset, IntPtr src, uint srcOffset, uint count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(IntPtr dest, ulong destOffset, IntPtr src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(void* dest, uint destOffset, void* src, uint srcOffset, uint count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(void* dest, ulong destOffset, void* src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(void* dest, void* src, uint count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(void* dest, void* src, uint destOffset, uint count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(void* dest, void* src, ulong count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(void* dest, void* src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte* dest, byte* src, uint count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte* dest, byte* src, uint destOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte* dest, byte* src, ulong count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte* dest, byte* src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte* dest, uint destOffset, byte* src, uint srcOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte* dest, ulong destOffset, byte* src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte[] dest, byte[] src, uint count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte[] dest, byte[] src, uint destOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte[] dest, byte[] src, ulong count)
        {
            extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte[] dest, byte[] src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(byte[] dest, uint destOffset, byte[] src, uint srcOffset, uint count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(byte[] dest, ulong destOffset, byte[] src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, IntPtr src, uint count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, IntPtr src, uint destOffset, uint count)
        {
            extractor.CopyBlock((byte*)src.ToPointer(), destOffset, (byte*)dest.ToPointer(), 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, IntPtr src, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, IntPtr src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, uint destOffset, IntPtr src, uint srcOffset, uint count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(IntPtr dest, ulong destOffset, IntPtr src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), destOffset, (byte*)(src.ToPointer()), srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(void* dest, uint destOffset, void* src, uint srcOffset, uint count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(void* dest, ulong destOffset, void* src, ulong srcOffset, ulong count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, srcOffset, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(void* dest, void* src, uint count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void Cpblk(void* dest, void* src, uint destOffset, uint count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(void* dest, void* src, ulong count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, count);
        }

        /// <summary>
        /// The Cpblk.
        /// </summary>
        /// <param name="dest">The dest<see cref="void*"/>.</param>
        /// <param name="src">The src<see cref="void*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void Cpblk(void* dest, void* src, ulong destOffset, ulong count)
        {
            extractor.CopyBlock((byte*)dest, destOffset, (byte*)src, 0, count);
        }

        #endregion
    }
}
