using System.Collections;
using System.Runtime.InteropServices;

namespace System.Extract
{
    public static class ObjectExtractExtenstion
    {
        public unsafe static object StructureFrom(this object structure, byte* binary)
        {
            return Extractor.PointerToStructure(binary, structure);
        }
        public unsafe static object StructureFrom(this object structure, IntPtr binary)
        {
            return Extractor.PointerToStructure(binary, structure);
        }
        public unsafe static object StructureFrom(this object structure, byte[] binary, long offset = 0)
        {
           return Extractor.BytesToStructure(binary, structure, offset);
        }

        public unsafe static void StructureTo(this object structure, byte* binary)
        {
            Extractor.StructureToPointer(structure, binary);
        }
        public unsafe static void StructureTo(this object structure, IntPtr binary)
        {
            Extractor.StructureToPointer(structure, binary);
        }
        public unsafe static void StructureTo(this object structure, ref byte[] binary, long offset = 0)
        {
            Extractor.StructureToBytes(structure, ref binary, offset);
        }

        public unsafe static byte[] GetStructureBytes(this object structure)
        {
            return Extractor.GetStructureBytes(structure);
        }
        public unsafe static byte[] GetValueStructureBytes(this object structure)
        {
            return Extraction.ValueStructureToBytes(structure);
        }
        public unsafe static byte[] GetSequentialBytes(this Object objvalue)
        {
            byte[] b = new byte[Marshal.SizeOf(objvalue)];
            fixed (byte* pb = b)
                Marshal.StructureToPtr(objvalue, new IntPtr(pb), false);
            return b;
        }

        public unsafe static byte*  GetStructurePointer(this object structure)
        {
            return Extractor.GetStructurePointer(structure);
        }
        public unsafe static IntPtr GetStructureIntPtr(this object structure)
        {
            return Extractor.GetStructureIntPtr(structure);
        }

        public unsafe static int   GetSize(this object structure)
        {
            return Extractor.GetSize(structure);
        }
        public unsafe static int[] GetSizes(this object structure)
        {
            return Extractor.GetSizes(structure);
        }

        public unsafe static Byte[]   GetBytes(this Object objvalue, bool forKeys= false)
        {            
            Type t = objvalue.GetType();
            if (objvalue is IUnique)
            {
                if (forKeys)
                    return ((IUnique)objvalue).GetUniqueBytes();
                return ((IUnique)objvalue).GetBytes();
            }

            if (objvalue is IList)
                return ((IList)objvalue).GetBytes(forKeys);

            if (t.IsValueType)
            {               
                if (t.IsPrimitive)
                    return Extraction.ValueStructureToBytes(objvalue);              
                if (objvalue is DateTime)
                    return ((DateTime)objvalue).ToBinary().GetBytes();
                if (objvalue is Enum)
                    return Convert.ToInt32(objvalue).GetBytes();
                return objvalue.GetStructureBytes();
            }

            if (t.IsLayoutSequential)
                return objvalue.GetSequentialBytes();

            if (objvalue is String || objvalue is IFormattable)
                return ((string)objvalue).GetBytes();
          
            return new byte[0];
        }
        public unsafe static bool  TryGetBytes(this Object objvalue, out Byte[] bytes, bool forKeys = false)
        {
            if ((bytes = objvalue.GetBytes(forKeys)).Length < 1)
                return false;
            return true;
        }

        public unsafe static int GetBytes(this IList objvalue, ref byte* buffer, int length, bool forKeys = false)
        {
            int offset = 0;            
            int charsize = sizeof(char);
           //  int count = objvalue.Count;
            int s;

            foreach(var o in objvalue)
            {
                s = 0;
               // object o = objvalue[i];
                if (o is string)
                {
                    s = ((string)o).Length * charsize;
                    int fs = (s + offset);
                    if (fs > length)
                    {
                        byte* _buffer = stackalloc byte[fs];
                        Extractor.CopyBlock(_buffer, buffer, offset);
                        buffer = _buffer;
                        length = fs;
                    }

                    fixed (char* c = (string)o)
                        Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (forKeys && o is IUnique)
                    {
                        s = 8;
                        *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        s = o.GetSize();
                        Extractor.StructureToPointer(o, buffer + offset);
                    }
                }
                offset += s;
            }

            return offset;
        }
        public unsafe static Byte[] GetBytes(this IList obj, bool forKeys = false)
        {
            int length = 256, offset = 0, postoffset = 0, count = obj.Count, charsize = sizeof(char), s = 0;
            byte* buffer = stackalloc byte[length];
            bool toResize = false;

            for (int i = 0; i < count; i++)
            {
                object o = obj[i];
                if (o is string)
                {
                    string str = ((string)o);
                    s = str.Length * charsize;
                    postoffset = (s + offset);

                    if (postoffset > length)
                        toResize = true;
                    else
                        fixed (char* c = str)
                            Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (o is IUnique)
                    {
                        s = 8;
                        postoffset = (s + offset);

                        if (postoffset > length)
                            toResize = true;
                        else
                            *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        s = o.GetSize();
                        postoffset = (s + offset);

                        if (postoffset > length)
                            toResize = true;
                        else
                            Extractor.StructureToPointer(o, new IntPtr(buffer + offset));
                    }
                }

                if (toResize)
                {
                    i--;
                    toResize = false;
                    byte* _buffer = stackalloc byte[postoffset];
                    Extractor.CopyBlock(_buffer, buffer, offset);
                    buffer = _buffer;
                    length = postoffset;
                }
                else
                    offset = postoffset;
            }

            byte[] result = new byte[offset];
            fixed (byte* result_p = result)
            {
                Extractor.CopyBlock(result_p, buffer, offset);
            }
            return result;
        }
        public unsafe static bool TryGetBytes(this IList objvalue, out byte[] bytes, bool forKeys = false)
        {
            bytes = objvalue.GetBytes(forKeys);
            if(bytes.Length > 0)
                return true;
            return false;
        }

        public unsafe static Byte[] GetBytes(this long[] objvalue)
        {
            int l = objvalue.Length * 8;
            byte[] b = new byte[l];
            fixed (byte* bp = b)
            fixed (long* lp = objvalue)
                Extractor.CopyBlock(bp, (byte*)lp, l);
            return b;
        }
        public unsafe static Byte[] GetBytes(this int[] objvalue)
        {
            int l = objvalue.Length * 4;
            byte[] b = new byte[l];
            fixed (byte* bp = b)
            fixed (int* lp = objvalue)
                Extractor.CopyBlock(bp, (byte*)lp, l);
            return b;
        }

        public unsafe static Byte[] GetBytes(this ulong[] objvalue)
        {
            int l = objvalue.Length * 8;
            byte[] b = new byte[l];
            fixed (byte* bp = b)
            fixed (ulong* lp = objvalue)
                Extractor.CopyBlock(bp, (byte*)lp, l);
            return b;
        }
        public unsafe static Byte[] GetBytes(this uint[] objvalue)
        {
            int l = objvalue.Length * 4;
            byte[] b = new byte[l];
            fixed (byte* bp = b)
            fixed (uint* lp = objvalue)
                Extractor.CopyBlock(bp, (byte*)lp, l);
            return b;
        }

        public unsafe static Byte[]  GetBytes(this String objvalue)
        {
            int l = objvalue.Length * sizeof(char);
            byte[] b = new byte[l];
            fixed (char* c = objvalue)
                fixed(byte* pb = b)
            {
                Extractor.Cpblk(pb, (byte*)c, (uint)l);
            }
            return b;
        }      

        public unsafe static bool StructureEqual(this object structure, object other)
        {
            long asize = Extractor.GetSize(structure);
            long bsize = Extractor.GetSize(structure);
            if (asize < bsize)
                return false;
            byte* a = (byte*)structure.GetStructurePointer(), b = (byte*)other.GetStructurePointer();
            bool equal = Extractor.BlockEqual(a, 0, b, 0, asize);
            Marshal.FreeHGlobal(new IntPtr(a));
            Marshal.FreeHGlobal(new IntPtr(b));
            return equal;
        }
        public unsafe static bool CompareBlocks(byte* source, long srcOffset, byte* dest, long destOffset, long count)
        {
            long sl = count;
            long sl64 = sl / 8;
            long sl8 = sl % 8;
            long* lsrc = (long*)(source + srcOffset), ldst = (long*)(dest + destOffset);
            for (int i = 0; i < sl64; i++)
                if (*(&lsrc[i]) != (*(&ldst[i])))
                    return false;
            byte* psrc8 = source + (sl64 * 8), pdst8 = dest + (sl64 * 8);
            for (int i = 0; i < sl8; i++)
                if (*(&psrc8[i]) != (*(&pdst8[i])))
                    return false;
            return true;
        }

       
    }

}
