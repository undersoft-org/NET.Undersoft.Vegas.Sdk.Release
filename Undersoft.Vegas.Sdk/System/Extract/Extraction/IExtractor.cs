/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.IExtractor.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    /// <summary>
    /// Defines the <see cref="IExtractor" />.
    /// </summary>
    public interface IExtractor
    {
        #region Methods

        /// <summary>
        /// The BytesToValueStructure.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        unsafe void BytesToValueStructure(byte[] array, ref ValueType structure, ulong offset);

        /// <summary>
        /// The BytesToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        unsafe void BytesToValueStructure(byte[] ptr, ref object structure, ulong offset);

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="source">The source<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        unsafe void CopyBlock(byte* source, uint srcOffset, byte* dest, uint destOffset, uint count);

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="source">The source<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        unsafe void CopyBlock(byte* source, ulong srcOffset, byte* dest, ulong destOffset, ulong count);

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        void CopyBlock(byte[] source, uint srcOffset, byte[] dest, uint destOffset, uint count);

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        void CopyBlock(byte[] source, ulong srcOffset, byte[] dest, ulong destOffset, ulong count);

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        unsafe void PointerToValueStructure(byte* ptr, ref object structure, ulong offset);

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        unsafe void PointerToValueStructure(byte* ptr, ref ValueType structure, ulong offset);

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] ValueStructureToBytes(object structure);

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="ptr">The ptr<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        void ValueStructureToBytes(object structure, ref byte[] ptr, ulong offset);

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] ValueStructureToBytes(ValueType structure);

        /// <summary>
        /// The ValueStructureToPointer.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="byte*"/>.</returns>
        unsafe byte* ValueStructureToPointer(object structure);

        /// <summary>
        /// The ValueStructureToPointer.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        unsafe void ValueStructureToPointer(object structure, byte* ptr, ulong offset);

        #endregion
    }
}
