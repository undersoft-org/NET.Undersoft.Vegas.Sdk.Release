/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.ValueTypeExtractExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="ValueTypeExtractExtenstion" />.
    /// </summary>
    public static class ValueTypeExtractExtenstion
    {
        #region Methods

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="hash">The hash<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public unsafe static byte[] GetBytes(this int hash)
        {
            byte[] q = new byte[4];
            fixed (byte* pbyte = q)
                *((int*)pbyte) = hash;
            return q;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="hash">The hash<see cref="long"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public unsafe static byte[] GetBytes(this long hash)
        {
            byte[] q = new byte[8];
            fixed (byte* pbyte = q)
                *((long*)pbyte) = hash;
            return q;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="hash">The hash<see cref="uint"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public unsafe static byte[] GetBytes(this uint hash)
        {
            byte[] q = new byte[4];
            fixed (byte* pbyte = q)
                *((uint*)pbyte) = hash;
            return q;
        }

        //public unsafe static Byte[] GetBytes(this ValueType objvalue)
        //{
        //    if (objvalue.GetType().BaseType.IsPrimitive)
        //    {
        //        byte[] b = new byte[Marshal.SizeOf(objvalue)];
        //        fixed (byte* pb = b)
        //            Marshal.StructureToPtr(objvalue, new IntPtr(pb), true);
        //        return b;
        //    }
        //    if (objvalue is IUnique)
        //        return ((IUnique)objvalue).GetBytes();
        //    else if (objvalue is DateTime)
        //        return ((DateTime)objvalue).ToBinary().GetStructureBytes();
        //    else if (objvalue is Enum)
        //        return objvalue.ToString().GetBytes();
        //    else
        //        return objvalue.GetStructureBytes();
        //}
        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="hash">The hash<see cref="ulong"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public unsafe static byte[] GetBytes(this ulong hash)
        {
            byte[] q = new byte[8];
            fixed (byte* pbyte = q)
                *((ulong*)pbyte) = hash;
            return q;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="objvalue">The objvalue<see cref="ValueType"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public unsafe static Byte[] GetBytes(this ValueType objvalue)
        {
            return Extractor.GetStructureBytes(objvalue);
        }

        /// <summary>
        /// The GetPrimitiveBytes.
        /// </summary>
        /// <param name="objvalue">The objvalue<see cref="object"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public unsafe static Byte[] GetPrimitiveBytes(this object objvalue)
        {
            byte[] b = new byte[Marshal.SizeOf(objvalue)];
            fixed (byte* pb = b)
                objvalue.StructureTo(pb);
            return b;
        }

        /// <summary>
        /// The GetPrimitiveLong.
        /// </summary>
        /// <param name="objvalue">The objvalue<see cref="object"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public unsafe static long GetPrimitiveLong(this object objvalue)
        {
            byte* ps = stackalloc byte[8];
            Marshal.StructureToPtr(objvalue, (IntPtr)ps, false);
            return *(long*)ps;
        }

        /// <summary>
        /// The StructureFrom.
        /// </summary>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="binary">The binary<see cref="byte*"/>.</param>
        public unsafe static void StructureFrom(this ValueType structure, byte* binary)
        {
            structure = Extractor.PointerToStructure(binary, structure);
        }

        /// <summary>
        /// The StructureFrom.
        /// </summary>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="binary">The binary<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        public unsafe static void StructureFrom(this ValueType structure, byte[] binary, long offset = 0)
        {
            structure = Extractor.BytesToStructure(binary, ref structure, offset);
        }

        /// <summary>
        /// The StructureFrom.
        /// </summary>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="binary">The binary<see cref="IntPtr"/>.</param>
        public unsafe static void StructureFrom(this ValueType structure, IntPtr binary)
        {
            structure = Extractor.PointerToStructure(binary, structure);
        }

        #endregion
    }
}
