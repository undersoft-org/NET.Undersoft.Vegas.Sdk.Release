/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.UniqueKeyExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Collections;
    using System.Extract;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the <see cref="UniqueKeyExtension32" />.
    /// </summary>
    public unsafe static class UniqueKeyExtension32
    {
        #region Methods

        /// <summary>
        /// The BitAggregate32to16.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] BitAggregate32to16(this Byte[] bytes)
        {
            byte[] bytes16 = new byte[2];
            fixed (byte* h16 = bytes16)
            fixed (byte* h32 = bytes)
            {
                *((ushort*)h16) = new ushort[] { *((ushort*)&h32), *((ushort*)&h32[2]) }
                                               .Aggregate((ushort)7, (a, b) => (ushort)((a + b) * 7));
                return bytes16;
            }
        }

        /// <summary>
        /// The BitAggregate64to16.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] BitAggregate64to16(this Byte[] bytes)
        {
            byte[] bytes16 = new byte[2];
            fixed (byte* h16 = bytes16)
            fixed (byte* h64 = bytes)
            {
                *((ushort*)h16) = new ushort[] { *((ushort*)&h64), *((ushort*)&h64[2]),
                                               *((ushort*)&h64[4]), *((ushort*)&h64[6]) }
                                               .Aggregate((ushort)7, (a, b) => (ushort)((a + b) * 7));
                return bytes16;
            }
        }

        /// <summary>
        /// The BitAggregate64to32.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 BitAggregate64to32(byte* bytes)
        {
            return new uint[] { *((uint*)&bytes), *((uint*)&bytes[4]) }
                                       .Aggregate(7U, (a, b) => (a + b) * 23);
        }

        /// <summary>
        /// The BitAggregate64to32.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] BitAggregate64to32(this Byte[] bytes)
        {
            byte[] bytes32 = new byte[4];
            fixed (byte* h32 = bytes32)
            fixed (byte* h64 = bytes)
            {
                *((uint*)h32) = new uint[] { *((uint*)&h64), *((uint*)&h64[4]) }
                                           .Aggregate(7U, (a, b) => (a + b) * 23);
                return bytes32;
            }
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="obj">The obj<see cref="IEquatable{T}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode<T>(this IEquatable<T> obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IList obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IntPtr ptr, int length, ulong seed = 0)
        {
            return ptr.UniqueKey32(length, seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IUnique obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed).ToUInt32();
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this Object obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this string obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this Byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this IList obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            byte* buffer = stackalloc byte[totalsize];
            int[] _sizes = sizes;
            int offset = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                object o = obj[i];
                int s = _sizes[i];
                if (o is string)
                {
                    string str = ((string)o);
                    fixed (char* c = str)
                        Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (o is IUnique)
                    {
                        s = 8;
                        *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        Extractor.StructureToPointer(o, buffer + offset);
                    }
                }
                offset += s;
            }

            return Hasher32.ComputeBytes(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte[] UniqueBytes32(this IList obj, ulong seed = 0)
        {
            int length = 256, offset = 0, postoffset = 0, count = obj.Count, s = 0;
            byte* buffer = stackalloc byte[length];
            bool toResize = false;

            for (int i = 0; i < count; i++)
            {
                object o = obj[i];
                if (o is string)
                {
                    string str = ((string)o);
                    s = str.Length * sizeof(char);
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
                            Extractor.StructureToPointer(o, buffer + offset);
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

            return Hasher32.ComputeBytes(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this IntPtr ptr, int length, ulong seed = 0)
        {
            return Hasher32.ComputeBytes((byte*)ptr.ToPointer(), length, seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this IUnique obj)
        {
            return obj.GetUniqueBytes().BitAggregate64to32();
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this Object obj, ulong seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).UniqueBytes32();
            if (obj is ValueType)
                return getValueTypeUniqueBytes32(obj, seed);
            if (obj is string)
                return (((string)obj)).UniqueBytes32(seed);
            if (obj is IList)
            {
                if (obj is Byte[])
                    return Hasher32.ComputeBytes((Byte[])obj, seed);

                IList o = (IList)obj;
                if (o.Count == 1)
                    return UniqueBytes32(o[0], seed);

                return UniqueBytes32(o, seed);
            }
            return Hasher32.ComputeBytes(obj.GetBytes(true), seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this Object[] obj, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueBytes32(obj[0], seed);
            return UniqueBytes32((IList)obj, seed);
        }

        /// <summary>
        /// The UniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="String"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes32(this String obj, ulong seed = 0)
        {
            fixed (char* c = obj)
                return Hasher32.ComputeBytes((byte*)c, obj.Length * sizeof(char), seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this Byte[] obj, ulong seed = 0)
        {
            return Hasher32.ComputeKey(obj, (uint)seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey32(this IList obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            byte* buffer = stackalloc byte[totalsize];
            int[] _sizes = sizes;
            int offset = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                object o = obj[i];
                int s = _sizes[i];
                if (o is string)
                {
                    string str = ((string)o);
                    fixed (char* c = str)
                        Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (o is IUnique)
                    {
                        s = 8;
                        *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        Extractor.StructureToPointer(o, buffer + offset);
                    }
                }
                offset += s;
            }

            return Hasher32.ComputeKey(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 UniqueKey32(this IList obj, ulong seed = 0)
        {
            int length = 256, offset = 0, postoffset = 0, count = obj.Count, s = 0;
            byte* buffer = stackalloc byte[length];
            bool toResize = false;

            for (int i = 0; i < count; i++)
            {
                object o = obj[i];
                if (o is string)
                {
                    string str = ((string)o);
                    s = str.Length * sizeof(char);
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
                            Extractor.StructureToPointer(o, buffer + offset);
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

            return Hasher32.ComputeKey(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this IntPtr ptr, int length, ulong seed = 0)
        {
            return Hasher32.ComputeKey((byte*)ptr.ToPointer(), length, seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this IUnique obj)
        {
            return obj.UniqueBytes32().ToUInt32();
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this IUnique obj, ulong seed)
        {
            return Hasher32.ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32<V>(this IUnique<V> obj, ulong seed)
        {
            return UniqueKey32(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this Object obj, ulong seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).UniqueBytes32().ToUInt32();
            if (obj is ValueType)
                return getValueTypeUniqueKey32(obj, seed);
            if (obj is string)
                return (((string)obj)).UniqueKey32(seed);
            if (obj is IList)
            {
                if (obj is Byte[])
                    return Hasher32.ComputeKey((Byte[])obj, seed);

                IList o = (IList)obj;
                if (o.Count == 1)
                    return UniqueKey32(o[0], seed);

                return UniqueKey32(o, seed);
            }
            return Hasher32.ComputeKey(obj.GetBytes(true), seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this Object[] obj, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueKey32(obj[0], seed);
            return UniqueKey32((IList)obj, seed);
        }

        /// <summary>
        /// The UniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 UniqueKey32(this string obj, ulong seed = 0)
        {
            fixed (char* c = obj)
                return Hasher32.ComputeKey((byte*)c, obj.Length * sizeof(char), seed);
        }

        /// <summary>
        /// The getValueTypeUniqueBytes32.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        private static Byte[] getValueTypeUniqueBytes32(object obj, ulong seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte[] s = new byte[8];
                fixed (byte* ps = s)
                {
                    Extraction.ValueStructureToPointer(obj, ps, 0);
                    return s.BitAggregate64to32();
                }
            }

            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary().GetBytes().BitAggregate64to32();

            if (t == typeof(Enum))
                return Convert.ToUInt32(obj).GetBytes();

            return (Hasher32.ComputeKey(obj.GetBytes(true)), seed).GetBytes(true);
        }

        /// <summary>
        /// The getValueTypeUniqueKey32.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        private static UInt32 getValueTypeUniqueKey32(object obj, ulong seed = 0)
        {
            Type t = obj.GetType();

            if (t.IsPrimitive || t.IsLayoutSequential)
            {
                byte* ps = stackalloc byte[8];
                Extraction.ValueStructureToPointer(obj, ps, 0);
                return BitAggregate64to32(ps);
            }

            if (t == typeof(DateTime))
                return ((DateTime)obj).ToBinary().GetBytes().BitAggregate64to32().ToUInt32();

            if (t == typeof(Enum))
                return Convert.ToUInt32(obj);

            return Hasher32.ComputeKey(obj.GetBytes(true), seed);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="UniqueKeyExtension64" />.
    /// </summary>
    public unsafe static class UniqueKeyExtension64
    {
        #region Methods

        /// <summary>
        /// The ComparableDouble.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="Double"/>.</returns>
        public static Double ComparableDouble(this Object obj, Type type = null)
        {
            if (type == null)
                type = obj.GetType();

            if (obj is string)
            {
                if (type != typeof(string))
                {
                    if (type == typeof(Usid))
                        return new Usid((string)obj).UniqueKey();
                    if (type == typeof(DateTime))
                        return ((DateTime)Convert.ChangeType(obj, type)).ToOADate();
                    return Convert.ToDouble(Convert.ChangeType(obj, type));
                }
                return (((string)obj).UniqueKey64());
            }

            if (obj is IUnique)
                return (((IUnique)obj).UniqueKey());
            if (type == typeof(DateTime))
                return ((DateTime)obj).ToOADate();
            if (obj is ValueType)
                return Convert.ToDouble(obj);
            return (obj.UniqueKey64());
        }

        /// <summary>
        /// The ComparableUInt64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 ComparableUInt64(this Object obj, Type type = null)
        {
            if (type == null)
                type = obj.GetType();

            if (obj is string)
            {
                if (type != typeof(string))
                {
                    if (type == typeof(IUnique))
                        return new Usid((string)obj).UniqueKey();
                    if (type == typeof(DateTime))
                        return (ulong)((DateTime)Convert.ChangeType(obj, type)).ToBinary();
                    return Convert.ToUInt64(Convert.ChangeType(obj, type));
                }
                return ((string)obj).UniqueKey64();
            }

            if (obj is IUnique)
                return ((IUnique)obj).UniqueKey();
            if (type == typeof(DateTime))
                return (ulong)((DateTime)obj).Ticks;
            if (obj is ValueType)
                return Convert.ToUInt64(obj);
            return obj.UniqueKey64();
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="obj">The obj<see cref="IEquatable{T}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode<T>(this IEquatable<T> obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IList obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IntPtr ptr, int length, ulong seed = 0)
        {
            return ptr.UniqueKey32(length, seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this IUnique obj)
        {
            return obj.UniqueBytes32().ToUInt32();
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this Object obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public static UInt32 GetHashCode(this string obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The IsSameOrNull.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="value">The value<see cref="Object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsSameOrNull(this Object obj, Object value)
        {
            if (obj != null)
                return obj.Equals(value);
            return (obj == null && value == null);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this Byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this IList obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            byte* buffer = stackalloc byte[totalsize];
            int[] _sizes = sizes;
            int offset = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                object o = obj[i];
                int s = _sizes[i];
                if (o is string)
                {
                    string str = ((string)o);
                    fixed (char* c = str)
                        Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (o is IUnique)
                    {
                        s = 8;
                        *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        Extractor.StructureToPointer(o, buffer + offset);
                    }
                }
                offset += s;
            }

            return Hasher64.ComputeBytes(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte[] UniqueBytes64(this IList obj, ulong seed = 0)
        {
            int length = 256, offset = 0, postoffset = 0, count = obj.Count, s = 0;
            byte* buffer = stackalloc byte[length];
            bool toResize = false;

            for (int i = 0; i < count; i++)
            {
                object o = obj[i];
                if (o is string)
                {
                    string str = ((string)o);
                    s = str.Length * sizeof(char);
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
                            Extractor.StructureToPointer(o, buffer + offset);
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

            return Hasher64.ComputeBytes(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this IntPtr bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeBytes((byte*)bytes.ToPointer(), length, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this IUnique obj)
        {
            return obj.GetUniqueBytes();
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this Object obj, ulong seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).GetUniqueBytes();
            if (obj is ValueType)
                return getValueTypeHashBytes64(obj, seed);
            if (obj is string)
                return (((string)obj)).UniqueBytes64(seed);
            if (obj is IList)
            {
                if (obj is Byte[])
                    return Hasher64.ComputeBytes((Byte[])obj, seed);

                IList o = (IList)obj;
                if (o.Count == 1)
                    return UniqueBytes64(o[0], seed);

                return UniqueBytes64(o, seed);
            }
            return Hasher64.ComputeBytes(obj.GetBytes(true), seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this Object[] obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueBytes64(obj[0], seed);
            return UniqueBytes64((IList)obj, sizes, totalsize, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this Object[] obj, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueBytes64(obj[0], seed);
            return UniqueBytes64((IList)obj, seed);
        }

        /// <summary>
        /// The UniqueBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="String"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public static Byte[] UniqueBytes64(this String obj, ulong seed = 0)
        {
            fixed (char* c = obj)
                return Hasher64.ComputeBytes((byte*)c, obj.Length * sizeof(char), seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this Byte[] bytes, ulong seed = 0)
        {
            return UniqueKey64(bytes, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="obj">The obj<see cref="IEquatable{T}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey<T>(this IEquatable<T> obj, ulong seed = 0)
        {
            return UniqueKey64(obj, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this IList obj, ulong seed = 0)
        {
            return UniqueKey64(obj, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this IntPtr ptr, int length, ulong seed = 0)
        {
            return UniqueKey64(ptr, length, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this IUnique obj, ulong seed)
        {
            return Hasher64.ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey<V>(this IUnique<V> obj, ulong seed)
        {
            return UniqueKey64(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this Object obj, ulong seed = 0)
        {
            return UniqueKey64(obj, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this Object[] obj, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueKey64(obj[0], seed);
            return UniqueKey64((IList)obj, seed);
        }

        /// <summary>
        /// The UniqueKey.
        /// </summary>
        /// <param name="obj">The obj<see cref="String"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey(this String obj, ulong seed = 0)
        {
            return UniqueKey64(obj, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this Byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this IList obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            byte* buffer = stackalloc byte[totalsize];
            int[] _sizes = sizes;
            int offset = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                object o = obj[i];
                int s = _sizes[i];
                if (o is string)
                {
                    string str = ((string)o);
                    fixed (char* c = str)
                        Extractor.CopyBlock(buffer, (byte*)c, offset, s);
                }
                else
                {
                    if (o is IUnique)
                    {
                        s = 8;
                        *((ulong*)(buffer + offset)) = ((IUnique)o).UniqueKey;
                    }
                    else
                    {
                        Extractor.StructureToPointer(o, buffer + offset);
                    }
                }
                offset += s;
            }

            return Hasher64.ComputeKey(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 UniqueKey64(this IList obj, ulong seed = 0)
        {
            int length = 256, offset = 0, postoffset = 0, count = obj.Count, s = 0;
            byte* buffer = stackalloc byte[length];
            bool toResize = false;

            for (int i = 0; i < count; i++)
            {
                object o = obj[i];
                if (o is string)
                {
                    string str = ((string)o);
                    s = str.Length * sizeof(char);
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
                            Extractor.StructureToPointer(o, buffer + offset);
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

            return Hasher64.ComputeKey(buffer, offset, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this IntPtr ptr, int length, ulong seed = 0)
        {
            return Hasher64.ComputeKey((byte*)ptr.ToPointer(), length, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this IUnique obj, ulong seed)
        {
            return Hasher64.ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64<V>(this IUnique<V> obj, ulong seed)
        {
            return UniqueKey64(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this Object obj, ulong seed = 0)
        {
            if (obj is IUnique)
                return ((IUnique)obj).UniqueKey;
            if (obj is ValueType)
                return getValueTypeUniqueKey64(obj, seed);
            if (obj is string)
                return (((string)obj)).UniqueKey64(seed);
            if (obj is IList)
            {
                if (obj is Byte[])
                    return Hasher64.ComputeKey((Byte[])obj, seed);

                IList o = (IList)obj;
                if (o.Count == 1)
                    return UniqueKey64(o[0], seed);

                return UniqueKey64(o, seed);
            }
            return Hasher64.ComputeKey(obj.GetBytes(true), seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="sizes">The sizes<see cref="int[]"/>.</param>
        /// <param name="totalsize">The totalsize<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this Object[] obj, int[] sizes, int totalsize, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueKey64(obj[0], seed);
            return UniqueKey64((IList)obj, sizes, totalsize, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this Object[] obj, ulong seed = 0)
        {
            if (obj.Length == 1)
                return UniqueKey64(obj[0], seed);
            return UniqueKey64((IList)obj, seed);
        }

        /// <summary>
        /// The UniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="String"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public static UInt64 UniqueKey64(this String obj, ulong seed = 0)
        {
            fixed (char* c = obj)
            {
                return Hasher64.ComputeKey((byte*)c, obj.Length * sizeof(char), seed);
            }
        }

        /// <summary>
        /// The getValueTypeHashBytes64.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        private static Byte[] getValueTypeHashBytes64(object obj, ulong seed = 0)
        {
            byte[] s = new byte[8];
            fixed (byte* ps = s)
            {
                Extractor.StructureToPointer(obj, ps);
                if (seed == 0)
                    return s;
                return Hasher64.ComputeBytes(ps, 8, seed);
            }
        }

        /// <summary>
        /// The getValueTypeUniqueKey64.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        private static UInt64 getValueTypeUniqueKey64(object obj, ulong seed = 0)
        {
            byte* ps = stackalloc byte[8];
            Extractor.StructureToPointer(obj, ps);
            if (seed == 0)
                return *(ulong*)ps;
            return Hasher64.ComputeKey(ps, 8, seed);
        }

        #endregion
    }
}
