using System.Runtime.InteropServices;
using System.Collections;
using System.Linq;

namespace System.Extract
{
    public static partial class Extractor
    {
        private static IExtractor extractor = Extraction.GetExtractor();

        public static unsafe void CopyBlock(byte* dest, byte* src, int count)
        {
            extractor.CopyBlock(dest, 0, src, 0, (uint)count);
        }
        public static unsafe void CopyBlock(void* dest, void* src, int count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, (uint)count);
        }

        public static unsafe void CopyBlock(byte* dest, byte* src, int destOffset, int count)
        {
            extractor.CopyBlock(dest, (uint)destOffset, src, 0, (uint)count);
        }
        public static unsafe void CopyBlock(void* dest, void* src, int destOffset, int count)
        {
            extractor.CopyBlock((byte*)dest, (uint)destOffset, (byte*)src, 0, (uint)count);
        }

        public static unsafe void CopyBlock(byte* dest, int destOffset, byte* src, int srcOffset, int count)
        {
            extractor.CopyBlock(dest, (uint)destOffset, src, (uint)srcOffset, (uint)count);
        }
        public static unsafe void CopyBlock(void* dest, int destOffset, void* src, int srcOffset, int count)
        {
            extractor.CopyBlock((byte*)dest, (uint)destOffset, (byte*)src, (uint)srcOffset, (uint)count);
        }

        public static unsafe void CopyBlock(byte* dest, byte* src, long count)
        {
            extractor.CopyBlock(dest, 0, src, 0, (ulong)count);
        }
        public static unsafe void CopyBlock(void* dest, void* src, long count)
        {
            extractor.CopyBlock((byte*)dest, 0, (byte*)src, 0, (ulong)count);
        }

        public static unsafe void CopyBlock(byte* dest, byte* src, long destOffset, long count)
        {
            extractor.CopyBlock(dest, (ulong)destOffset, src, 0, (ulong)count);
        }
        public static unsafe void CopyBlock(void* dest, void* src, long destOffset, long count)
        {
            extractor.CopyBlock((byte*)dest, (ulong)destOffset, (byte*)src, 0, (ulong)count);
        }

        public static unsafe void CopyBlock(byte* dest, long destOffset, byte* src, long srcOffset, long count)
        {
            extractor.CopyBlock(dest, (ulong)destOffset, src, (ulong)srcOffset, (ulong)count);
        }
        public static unsafe void CopyBlock(void* dest, long destOffset, void* src, long srcOffset, long count)
        {
            extractor.CopyBlock((byte*)dest, (ulong)destOffset, (byte*)src, (ulong)srcOffset, (ulong)count);
        }

        public static unsafe void CopyBlock(byte[] dest, byte[] src, int count)
        {
            extractor.CopyBlock(dest, 0, src, 0, (uint)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, int count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, (uint)count);
        }

        public static unsafe void CopyBlock(byte[] dest, byte[] src, int destOffset, int count)
        {
            extractor.CopyBlock(dest, (uint)destOffset, src, 0, (uint)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, int destOffset, int count)
        {
            extractor.CopyBlock((byte*)src.ToPointer(), (uint)destOffset, (byte*)dest.ToPointer(), 0, (uint)count);
        }

        public static unsafe void CopyBlock(byte[] dest, int destOffset, byte[] src, int srcOffset, int count)
        {
            extractor.CopyBlock(dest, (uint)destOffset, src, (uint)srcOffset, (uint)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, int destOffset, IntPtr src, int srcOffset, int count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), (uint)destOffset, (byte*)(src.ToPointer()), (uint)srcOffset, (uint)count);
        }

        public static unsafe void CopyBlock(byte[] dest, byte[] src, long count)
        {
            extractor.CopyBlock(dest, 0, src, 0, (ulong)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, long count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), 0, (byte*)(src.ToPointer()), 0, (ulong)count);
        }

        public static unsafe void CopyBlock(byte[] dest, byte[] src, long destOffset, long count)
        {
            extractor.CopyBlock(dest, (ulong)destOffset, src, 0, (ulong)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, IntPtr src, long destOffset, long count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), (ulong)destOffset, (byte*)(src.ToPointer()), 0, (ulong)count);
        }

        public static unsafe void CopyBlock(byte[] dest, long destOffset, byte[] src, long srcOffset, long count)
        {
            extractor.CopyBlock(dest, (ulong)destOffset, src, (ulong)srcOffset, (ulong)count);
        }
        public static unsafe void CopyBlock(IntPtr dest, long destOffset, IntPtr src, long srcOffset, long count)
        {
            extractor.CopyBlock((byte*)(dest.ToPointer()), (ulong)destOffset, (byte*)(src.ToPointer()), (ulong)srcOffset, (ulong)count);
        }

        public unsafe static bool BlockEqual(byte[] source, byte[] dest)
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
        public unsafe static bool BlockEqual(byte* source, long srcOffset, byte* dest, long destOffset, long count)
        {
            long sl = count;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            long* lsrc = (long*)(source + srcOffset), ldst = (long*)(dest + destOffset);
            for (int i = 0; i < sl64; i++)
                if (*(&lsrc[i]) != *(&ldst[i]))
                    return false;
            byte* psrc8 = source + (sl64 * 8), pdst8 = dest + (sl64 * 8);
            for (int i = 0; i < sl8; i++)
                if (*(&psrc8[i]) != *(&pdst8[i]))
                    return false;
            return true;
        }

        public static unsafe object BytesToStructure(byte[] binary, Type structure, long offset)
        {
            fixed (byte* b = &binary[offset])
                return PointerToStructure(new IntPtr(b), structure, 0);
        }
        public unsafe static object PointerToStructure(byte* binary, Type structure, long offset)
        {
            return PointerToStructure(new IntPtr(binary + offset), structure, 0);
        }
        public unsafe static object PointerToStructure(IntPtr binary, Type structure, int offset)
        {
            if (structure == typeof(DateTime))
                return DateTime.FromBinary((long)Marshal.PtrToStructure(binary + (int)offset, typeof(long)));
            else
                return Marshal.PtrToStructure(binary, structure);
        }

        public unsafe static object PointerToStructure(byte* binary, object structure)
        {
           return PointerToStructure(new IntPtr(binary), structure);
        }
        public unsafe static object PointerToStructure(IntPtr binary, object structure)
        {
            if (structure is ValueType)
            {
                Type t = structure.GetType();
                if (t.IsLayoutSequential)
                {
                    return Extraction.PointerToValueStructure((byte*)binary, structure, 0);                    
                }
                else
                    return PointerToStructure(binary, structure.GetType(), 0);
            }
            else
                Marshal.PtrToStructure(binary, structure);
            return structure;
        }

        public unsafe static ValueType PointerToStructure(byte* binary, ValueType structure)
        {
            return Extraction.PointerToValueStructure(binary, structure, 0);           
        }
        public unsafe static ValueType PointerToStructure(IntPtr binary, ValueType structure)
        {
            return Extraction.PointerToValueStructure((byte*)binary, structure, 0);
        }

        public unsafe static object BytesToStructure(byte[] binary, object structure, long offset)
        {
            if (structure is ValueType)
            {
                return Extraction.BytesToValueStructure(binary, structure, 0);               
            }
            else
            {
                fixed (byte* b = &binary[offset])
                    return PointerToStructure(new IntPtr(b), structure);
            }
        }
        public unsafe static ValueType BytesToStructure(byte[] binary, ref ValueType structure, long offset)
        {
            extractor.BytesToValueStructure(binary, ref structure, 0);
            return structure;
        }

        public unsafe static void StructureToPointer(object structure, byte* binary)
        {
            IntPtr p = new IntPtr(binary);
            StructureToPointer(structure, p);
            binary = (byte*)p;
        }
        public unsafe static void StructureToPointer(object structure, IntPtr binary)
        {
            if (structure is ValueType)
            {
                Type t = structure.GetType();

                if (t.IsPrimitive)
                {                   
                    Marshal.StructureToPtr(structure, binary, false);
                    return;
                }

                if (t.IsLayoutSequential)
                {
                    extractor.ValueStructureToPointer(structure, (byte*)binary, 0);
                    return;
                }

                if (structure is DateTime)
                {
                    structure = ((DateTime)structure).ToBinary();
                    Marshal.StructureToPtr(structure, binary, false);
                    return;
                }          
                
                if(structure is Enum)
                {
                    structure = Convert.ToUInt32(structure);
                    Marshal.StructureToPtr(structure, binary, false);
                    return;
                }
            }

            Marshal.StructureToPtr(structure, binary, false);
        }
        public unsafe static void StructureToBytes(object structure, ref byte[] binary, long offset)
        {
            fixed (byte* pb = &binary[offset])
            {
                IntPtr p = new IntPtr(pb);
                StructureToPointer(structure, p);
            }
        }

        public unsafe static void StructureToPointer(ValueType structure, byte* binary)
        {
            IntPtr p = new IntPtr(binary);
            StructureToPointer(structure, p);
            binary = (byte*)p;
        }
        public unsafe static void StructureToPointer(ValueType structure, IntPtr binary)
        {
            if (structure is DateTime)           
                structure = ((DateTime)structure).ToBinary();
            if (structure.GetType().IsLayoutSequential)
            {
                extractor.ValueStructureToPointer(structure, (byte*)binary, 0);
                return;
            }

            Marshal.StructureToPtr(structure, binary, false);
        }
        public unsafe static void StructureToBytes(ValueType structure, ref byte[] binary, long offset)
        {
            fixed (byte* pb = &binary[offset])
            {
                IntPtr p = new IntPtr(pb);
                StructureToPointer(structure, p);
            }
        }

        public unsafe static byte[] GetStructureBytes(object structure)
        {
            byte[] b = null;

            if (structure is ValueType)
            {
                Type t = structure.GetType();
                if (t.IsPrimitive || t.IsLayoutSequential)
                    return extractor.ValueStructureToBytes(structure);

                if (structure is DateTime)
                {
                    b = new byte[8];
                    structure = ((DateTime)structure).ToBinary();
                }
                else if (structure is Enum)
                {
                    b = new byte[4];
                    structure = Convert.ToInt32((Enum)structure);
                }
                else
                {
                    b = new byte[Marshal.SizeOf(structure)];
                }
            }
            else if (structure.GetType() == typeof(string))
            {
                int l = ((string)structure).Length;
                b = new byte[l];
                fixed (char* c = (string)structure)
                fixed (byte* pb = b)
                    CopyBlock(pb, (byte*)c, l);
                return b;
            }
            else
                b = new byte[Marshal.SizeOf(structure)];


            fixed (byte* pb = b)
                Marshal.StructureToPtr(structure, new IntPtr(pb), false);
            return b;
        }
        public unsafe static byte[] GetStructureBytes(ValueType structure)
        {
          
            Type t = structure.GetType();
            if (t.IsPrimitive || t.IsLayoutSequential)
                return extractor.ValueStructureToBytes(structure);

            byte[] b = null;
            var _structure = structure;
            if (structure is DateTime)
            {
                b = new byte[8];
                _structure = ((DateTime)structure).ToBinary();
            }
            else if (structure is Enum)
            {
                b = new byte[4];
                _structure = Convert.ToInt32((Enum)structure);
            }
            else
            {
                b = new byte[Marshal.SizeOf(_structure)];
            }


            fixed (byte* pb = b)
                Marshal.StructureToPtr(_structure, new IntPtr(pb), false);
            return b;
        }
        public unsafe static byte* GetStructurePointer(object structure)
        {
            int size = 0;

            if (structure is ValueType)
            {
                Type t = structure.GetType();
                if (t.IsPrimitive || t.IsLayoutSequential)
                    return extractor.ValueStructureToPointer(structure);

                if (structure is DateTime)
                {
                    size = 8;
                    structure = ((DateTime)structure).ToBinary();
                }
                //else if (structure is ISerialNumber)
                //    size = 24;
                //else if (structure is IUnique)
                //    size = 8;
                else if (structure is Enum)
                {
                    size = 4;
                    structure = Convert.ToInt32((Enum)structure);
                }
                else
                    size = Marshal.SizeOf(structure);
            }
            else
                size = Marshal.SizeOf(structure);

            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, p, false);

            return (byte*)p;
        }
        public unsafe static IntPtr GetStructureIntPtr(object structure)
        {
            int size = 0;
         
            if (structure is ValueType)
            {
                Type t = structure.GetType();
                if (t.IsPrimitive || t.IsLayoutSequential)
                {
                    return new IntPtr(extractor.ValueStructureToPointer(structure));
                }

                if (structure is DateTime)
                {
                    size = 8;
                    structure = ((DateTime)structure).ToBinary();
                }             
                //else if (structure is ISerialNumber)
                //    size = 24;
                //else if (structure is IUnique)
                //    size = 8;
                else if (structure is Enum)
                {
                    size = 4;
                    structure = Convert.ToInt32((Enum)structure);
                }
                else
                    size = Marshal.SizeOf(structure);
            }
            else
                size = Marshal.SizeOf(structure);

            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, p, false);

            return p;
        }
    
        private static int ValueTypeElementSize(Type e)
        {
            if (e.IsPrimitive || e.IsLayoutSequential)
                return Marshal.SizeOf(e);
            if (e == typeof(DateTime))
                return 8;
            if (e == typeof(Enum))
                return 4;          
            return Marshal.SizeOf(e);
        }
        private static int ValueTypeObjectSize(object structure)
        {
            Type t = structure.GetType();
            if (t.IsPrimitive || t.IsLayoutSequential)
                return Marshal.SizeOf(structure);
            if (t == typeof(DateTime))
                return 8;
            if (t == typeof(Enum))
                return 4;           
            return Marshal.SizeOf(structure);
        }

        public unsafe static int[] GetSizes(object[] array)
        {
            return GetSizes((IList)array);           
        }      
        public unsafe static int[] GetSizes(IList list)
        {
            int c = list.Count;
            if (c > 0)
            {
                if (list.GetType().HasElementType)
                {
                    var e = list.GetType().GetElementType();
                    if (e.IsValueType)
                        return new int[] { ValueTypeElementSize(e) * c };
                    if (e == typeof(string))
                        return list.Cast<string>().Select(p => p.Length).ToArray();
                    if (e.IsLayoutSequential)
                        return new int[c].Select(l => l = Marshal.SizeOf(e)).ToArray();
                    if (e.IsArray)
                    {
                        return list.Cast<object>().Select(a => GetSizes(a).Sum()).ToArray();
                    }
                }
                return list.Cast<object>()
                                .Select(o => o.GetSize()).ToArray();
            }
            return new int[0];
        }
        public unsafe static int[] GetSizes(object structure)
        {
            if (structure is ValueType)
                return new int[] { ValueTypeObjectSize(structure) };             
            if (structure is String || structure is IFormattable)
                return new int[] { structure.ToString().Length };
            if (structure.GetType().IsLayoutSequential)
                return new int[] { Marshal.SizeOf(structure) };
            if (structure is IList)
                return GetSizes(((IList)structure));
            
            return new int[0];
        }
        public unsafe static int GetSize(object structure)
        {          
            if (structure is ValueType)            
               return ValueTypeObjectSize(structure);
            if (structure.GetType().IsLayoutSequential)
                return Marshal.SizeOf(structure);
            if (structure is String || structure is IFormattable)
                return structure.ToString().Length * sizeof(char);
            if (structure is IList)
                return GetSizes(((IList)structure)).Sum();
            return 0;
        }
    }
}
