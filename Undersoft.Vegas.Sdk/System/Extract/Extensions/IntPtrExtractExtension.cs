/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.IntPtrExtractExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    /// <summary>
    /// Defines the <see cref="IntPtrExtractExtenstion" />.
    /// </summary>
    public static class IntPtrExtractExtenstion
    {
        #region Methods

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, uint count)
        {
            Extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="offset">The offset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, uint offset, uint count)
        {
            Extractor.CopyBlock(dest, offset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, ulong count)
        {
            Extractor.CopyBlock(dest, 0, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, IntPtr dest, ulong offset, ulong count)
        {
            Extractor.CopyBlock(dest, offset, src, 0, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, uint srcOffset, IntPtr dest, uint destOffset, uint count)
        {
            Extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="src">The src<see cref="IntPtr"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="IntPtr"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(this IntPtr src, ulong srcOffset, IntPtr dest, ulong destOffset, ulong count)
        {
            Extractor.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The FromStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        public unsafe static void FromStructure(this IntPtr binary, object structure)
        {
            Extractor.StructureToPointer(structure, binary);
        }

        /// <summary>
        /// The NewStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="Type"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object NewStructure(this IntPtr binary, Type structure, int offset)
        {
            return Extractor.PointerToStructure(binary, structure, offset);
        }

        /// <summary>
        /// The ToStructure.
        /// </summary>
        /// <param name="binary">The binary<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object ToStructure(this IntPtr binary, object structure)
        {
            return Extractor.PointerToStructure(binary, structure);
        }

        #endregion
    }
}
