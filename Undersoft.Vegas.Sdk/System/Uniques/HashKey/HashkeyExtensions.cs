using System.Collections;
using System.Extract;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Uniques
{
    public unsafe static class HashKeyExtensions64
    {
        public static Byte[] GetHashBytes64(this IntPtr bytes, int length, uint seed = 0)
        {
            return HashHandle64.ComputeHashBytes((byte*)bytes.ToPointer(), length, seed);
        }
        public static Byte[] GetHashBytes64(this Byte[] bytes, uint seed = 0)
        {
            return HashHandle64.ComputeHashBytes(bytes, seed);
        }
        public static Byte[] GetHashBytes64(this Object obj, uint seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).GetKeyBytes();
            if (obj is ValueType)
                return GetValueTypeHashBytes64(obj);
            if (obj is string)
                return (((string)obj)).GetHashBytes64();
            if (obj is IList)
                return GetHashBytes64((IList)obj);
            return HashHandle64.ComputeHashBytes(obj.GetBytes(true), seed);
        }
        public static Byte[] GetHashBytes64(this IList obj, uint seed = 0)
        {
            byte* bytes = stackalloc byte[256];
            int length = obj.GetBytes(ref bytes, 256, true);
            return HashHandle64.ComputeHashBytes(bytes, length, seed);
        }
        public static Byte[] GetHashBytes64(this String obj, uint seed = 0)
        {
            fixed (char* c = obj)
                return HashHandle64.ComputeHashBytes((byte*)c, obj.Length * sizeof(char), seed);
        }
        public static Byte[] GetHashBytes64(this IUnique obj)
        {
            return obj.GetKeyBytes();
        }

        public static unsafe 
                       Int64 GetHashKey64(this IntPtr ptr, int length, uint seed = 0)
        {
            return (long)HashHandle64.ComputeHashKey((byte*)ptr.ToPointer(), length, seed);
        }
        public static  Int64 GetHashKey64(this Byte[] bytes, uint seed = 0)
        {
            return (long)HashHandle64.ComputeHashKey(bytes, seed);
        }
        public static  Int64 GetHashKey64(this Object obj, uint seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).GetHashKey();
            if (obj is ValueType)
                return GetValueTypeHashKey64(obj);          
            if (obj is string)
                return (((string)obj)).GetHashKey64();
            if (obj is IList)
                return GetHashKey64((IList)obj);
            return (long)HashHandle64.ComputeHashKey(obj.GetBytes(true), seed);
        }
        public static unsafe 
                       Int64 GetHashKey64(this IList obj, uint seed = 0)
        {
            if (obj is Byte[])
                return GetHashKey64((Byte[])obj);

            byte* bytes = stackalloc byte[256];
            int length = obj.GetBytes(ref bytes, 256, true);
            return (long)HashHandle64.ComputeHashKey(bytes, length, seed);
        }      
        public static  Int64 GetHashKey64(this String obj, uint seed = 0)
        {
            fixed (char* c = obj)
            {
                return (long)HashHandle64.ComputeHashKey((byte*)c, obj.Length * sizeof(char), seed);
            }
        }
        public static  Int64 GetHashKey64<V>(this IUnique<V> obj, uint seed)
        {
            return GetHashKey64(obj.IdentityValues(), seed);
        }
        public static  Int64 GetHashKey64(this IUnique obj, uint seed)
        {
            return GetHashKey64(obj.GetKeyBytes(), seed);
        }
        public static  Int64 GetHashKey64(this IUnique obj)
        {
            return obj.GetHashKey();
        }

        public static Int32 GetHashCode(this IntPtr ptr, int length, uint seed = 0)
        {
            return ptr.GetHashKey32(length, seed);
        }
        public static Int32 GetHashCode<T>(this IEquatable<T> obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this Byte[] obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this Object obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this IList obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this string obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this IUnique obj)
        {
            return obj.GetHashBytes32().ToInt32();
        }

        public static Int64 GetHashKey(this IntPtr ptr, int length, uint seed = 0)
        {
            return ptr.GetHashKey64(length, seed);
        }
        public static Int64 GetHashKey<T>(this IEquatable<T> obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public static Int64 GetHashKey(this Byte[] bytes, uint seed = 0)
        {
            return bytes.GetHashKey64(seed);
        }
        public static Int64 GetHashKey(this Object obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public static Int64 GetHashKey(this IList obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public static Int64 GetHashKey(this String obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public static Int64 GetHashKey<V>(this IUnique<V> obj, uint seed)
        {
            return GetHashKey64(obj.IdentityValues(), seed);
        }
        public static Int64 GetHashKey(this IUnique obj, uint seed)
        {
            return GetHashKey64(obj.GetKeyBytes(), seed);
        }
        public static Int64 GetHashKey(this IUnique obj)
        {
            return obj.GetHashKey();
        }

        public static bool   IsSameOrNull(this Object obj, Object value)
        {
            if (obj != null)
                return obj.Equals(value);
            return (obj == null && value == null);
        }

        public static Int64  ComparableInt64(this Object obj, Type type = null)
        {
            if (type == null)
                type = obj.GetType();

            if (obj is string)
            {
                if (type != typeof(string))
                {
                    if (type == typeof(IUnique))
                        return new Usid((string)obj).GetHashKey();
                    if (type == typeof(DateTime))
                        return ((DateTime)Convert.ChangeType(obj, type)).ToBinary();
                    return Convert.ToInt64(Convert.ChangeType(obj, type));
                }
                return ((string)obj).GetHashKey64();
            }

            if (type == typeof(IUnique))
                return ((IUnique)obj).GetHashKey();
            if (type == typeof(DateTime))
                return ((DateTime)obj).Ticks;
            if (obj is ValueType)
                return Convert.ToInt64(obj);
            return obj.GetHashKey64();
        }
        public static Double ComparableDouble(this Object obj, Type type = null)
        {
            if (type == null)
                type = obj.GetType();

            if (obj is string)
            {
                if (type != typeof(string))
                {
                    if (type == typeof(Usid))
                        return new Usid((string)obj).GetHashKey();
                    if (type == typeof(DateTime))
                        return ((DateTime)Convert.ChangeType(obj, type)).ToOADate();
                    return Convert.ToDouble(Convert.ChangeType(obj, type));
                }
                return (((string)obj).GetHashKey64());
            }

            if (type == typeof(IUnique))
                return (((IUnique)obj).GetHashKey());
            if (type == typeof(DateTime))
                return ((DateTime)obj).ToOADate();
            if (obj is ValueType)
                return Convert.ToDouble(obj);
            return (obj.GetHashKey64());
        }

        private static Int64  GetValueTypeHashKey64(object obj, uint seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte* ps = stackalloc byte[8];               
                ExtractOperation.ValueStructureToPointer(obj, ps, 0);
                return *(long*)ps;
            }

            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary();

            if (t == typeof(Enum))
                return Convert.ToInt32(obj);

            //if (obj is IUnique)
            //    return ((IUnique)obj).GetHashKey();

            //if (t.IsLayoutSequential)
            //{
            //    byte* ps = stackalloc byte[8];
            //    ExtractOperation.ValueStructureToPointer(obj, ps, 0);
            //    return *(long*)ps;
            //}

            return (long)HashHandle64.ComputeHashKey(obj.GetBytes(true), seed);
        }

        private static Byte[] GetValueTypeHashBytes64(object obj, uint seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte[] s = new byte[8];
                fixed (byte* ps = s)
                    ExtractOperation.ValueStructureToPointer(obj, ps, 0);
                return s;
            }          

            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary().GetBytes();

            if (t == typeof(Enum))
                return Convert.ToInt32(obj).GetBytes();

            //if (obj is IUnique)
            //    return ((IUnique)obj).GetKeyBytes();

            //if (t.IsLayoutSequential)
            //{
            //    byte[] s = new byte[8];
            //    fixed (byte* ps = s)                
            //        ExtractOperation.ValueStructureToPointer(obj, ps, 0);                
            //    return s;
            //}

            return ((long)HashHandle64.ComputeHashKey(obj.GetBytes(true), seed)).GetBytes(true);
        }
    }

    public unsafe static class HashKeyExtensions32
    {
        public static Int32 BitAggregate64to32(byte* bytes)
        {
            return new int[] { *((int*)&bytes), *((int*)&bytes[4]) }
                                       .Aggregate(7, (a, b) => (a + b) * 23);

        }
        public static Byte[] BitAggregate64to32(this Byte[] bytes)
        {
            byte[] bytes32 = new byte[4];
            fixed (byte* h32 = bytes32)
            fixed (byte* h64 = bytes)
            {
                *((int*)h32) = new int[] { *((int*)&h64), *((int*)&h64[4]) }
                                           .Aggregate(7, (a, b) => (a + b) * 23);
                return bytes32;
            }
        }
        public static Byte[] BitAggregate32to16(this Byte[] bytes)
        {
            byte[] bytes16 = new byte[2];
            fixed (byte* h16 = bytes16)
            fixed (byte* h32 = bytes)
            {
                *((short*)h16) = new short[] { *((short*)&h32), *((short*)&h32[2]) }
                                               .Aggregate((short)7, (a, b) => (short)((a + b) * 7));
                return bytes16;
            }
        }
        public static Byte[] BitAggregate64to16(this Byte[] bytes)
        {
            byte[] bytes16 = new byte[2];
            fixed (byte* h16 = bytes16)
            fixed (byte* h64 = bytes)
            {
                *((short*)h16) = new short[] { *((short*)&h64), *((short*)&h64[2]),
                                               *((short*)&h64[4]), *((short*)&h64[6]) }
                                               .Aggregate((short)7, (a, b) => (short)((a + b) * 7));
                return bytes16;
            }
        }

        public static unsafe Byte[] GetHashBytes32(this IntPtr ptr, int length, uint seed = 0)
        {
            return HashHandle32.ComputeHashBytes((byte*)ptr.ToPointer(), length, seed);
        }

        public static Byte[] GetHashBytes32(this Byte[] bytes, uint seed = 0)
        {
            return HashHandle32.ComputeHashBytes(bytes, seed);
        }
        public static Byte[] GetHashBytes32(this Object obj, uint seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).GetHashBytes32();
            if (obj is ValueType)
                return GetValueTypeHashBytes32(obj, seed);
            if (obj is string)
                return (((string)obj)).GetHashBytes32(seed);
            if (obj is IList)
                return GetHashBytes32((IList)obj, seed);
            return HashHandle32.ComputeHashBytes(obj.GetBytes(true), seed);
        }
        public static Byte[] GetHashBytes32(this IList obj, uint seed = 0)
        {
            byte* bytes = stackalloc byte[256];
            int length = obj.GetBytes(ref bytes, 256, true);
            return HashHandle32.ComputeHashBytes(bytes, length, seed);
        }
        public static Byte[] GetHashBytes32(this String obj, uint seed = 0)
        {
            fixed (char* c = obj)
                return HashHandle32.ComputeHashBytes((byte*)c, obj.Length * sizeof(char), seed);
        }
        public static Byte[] GetHashBytes32(this IUnique obj)
        {
            return obj.GetKeyBytes().BitAggregate64to32();
        }

        public static unsafe Int32 GetHashKey32(this IntPtr ptr, int length, uint seed = 0)
        {
            return (int)HashHandle32.ComputeHashKey((byte*)ptr.ToPointer(), length, seed);
        }

        public static Int32 GetHashKey32(this Byte[] obj, uint seed = 0)
        {
            return (int)HashHandle32.ComputeHashKey(obj, seed);
        }
        public static Int32 GetHashKey32(this Object obj, uint seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).GetHashKey32();
            if (obj is ValueType)
                return GetValueTypeHashKey32(obj, seed);           
            if (obj is string)
                return (((string)obj)).GetHashKey32(seed);
            if (obj is IList)
                return GetHashKey32((IList)obj, seed);
            return (int)HashHandle32.ComputeHashKey(obj.GetBytes(true), seed);
        }
        public static Int32 GetHashKey32(this IList obj, uint seed = 0)
        {
            if (obj is Byte[])
                return GetHashKey32((Byte[])obj, seed);

            byte* bytes = stackalloc byte[256];
            int length = obj.GetBytes(ref bytes, 256, true);
            return (int)HashHandle32.ComputeHashKey(bytes, length, seed);
        }
        public static Int32 GetHashKey32(this string obj, uint seed = 0)
        {
            fixed (char* c = obj)
                return (int)HashHandle32.ComputeHashKey((byte*)c, obj.Length * sizeof(char), seed);
        }
        public static Int32 GetHashKey32<V>(this IUnique<V> obj, uint seed)
        {
            return GetHashKey32(obj.IdentityValues(), seed);
        }
        public static Int32 GetHashKey32(this IUnique obj, uint seed)
        {
            return GetHashKey32(obj.GetKeyBytes(), seed);
        }
        public static Int32 GetHashKey32(this IUnique obj)
        {
            return obj.GetHashBytes32().ToInt32();
        }

        public static unsafe 
                      Int32 GetHashCode(this IntPtr ptr, int length, uint seed = 0)
        {
            return ptr.GetHashKey32(length, seed);
        }

        public static Int32 GetHashCode<T>(this IEquatable<T> obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this Byte[] obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this Object obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this IList obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this string obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public static Int32 GetHashCode(this IUnique obj, uint seed = 0)
        {
            return obj.GetHashBytes32(seed).ToInt32();
        }

        private static Int32 GetValueTypeHashKey32(object obj, uint seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte* ps = stackalloc byte[8];
                ExtractOperation.ValueStructureToPointer(obj, ps, 0);
                return BitAggregate64to32(ps);
            }
          
            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary().GetBytes().BitAggregate64to32().ToInt32();

            if (t == typeof(Enum))
                return Convert.ToInt32(obj);

            //if (t == typeof(IUnique))
            //    return ((IUnique)obj).GetKeyBytes().BitAggregate64to32().ToInt32();

            //if (t.IsLayoutSequential)
            //{
            //    byte* ps = stackalloc byte[8];
            //    ExtractOperation.ValueStructureToPointer(obj, ps, 0);
            //    return BitAggregate64to32(ps);
            //}

            return (int)HashHandle32.ComputeHashKey(obj.GetBytes(true), seed);
        }

        private static Byte[] GetValueTypeHashBytes32(object obj, uint seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte[] s = new byte[8];
                fixed (byte* ps = s)
                {
                    ExtractOperation.ValueStructureToPointer(obj, ps, 0);
                    return s.BitAggregate64to32();
                }
            }

            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary().GetBytes().BitAggregate64to32();

            if (t == typeof(Enum))
                return Convert.ToInt32(obj).GetBytes();

            //if (t == typeof(IUnique))
            //    return ((IUnique)obj).GetKeyBytes().BitAggregate64to32();

            //if (t.IsLayoutSequential)
            //{
            //    byte[] s = new byte[8];
            //    fixed (byte* ps = s)
            //    {
            //        ExtractOperation.ValueStructureToPointer(obj, ps, 0);
            //        return s.BitAggregate64to32();
            //    }
            //}

            return ((int)HashHandle32.ComputeHashKey(obj.GetBytes(true)), seed).GetBytes(true);
        }
    }

}
