using System.Collections;
using System.Extract;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Uniques
{
    using System.Runtime.CompilerServices;

    public static class HashHandle64
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Byte[] ComputeHashBytes(byte[] bytes, uint seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b, pa = bytes)
            {
                *((ulong*)pb) = xxHash64.UnsafeComputeHash(pa, bytes.Length, seed);
            }
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Byte[] ComputeHashBytes(byte* bytes, int length, uint seed = 0)
        {
            byte[] b = new byte[8];
            fixed (byte* pb = b)
            {
                *((ulong*)pb) = xxHash64.UnsafeComputeHash(bytes, length, seed);
            }
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ulong  ComputeHashKey(byte[] bytes, uint seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return xxHash64.UnsafeComputeHash(pa, bytes.Length, seed);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ulong  ComputeHashKey(byte* ptr, int length, ulong seed = 0)
        {
            return xxHash64.UnsafeComputeHash(ptr, length, seed);
        }
    }

    public static class HashHandle32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Byte[] ComputeHashBytes(byte[] bytes, uint seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b, pa = bytes)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(pa, bytes.Length, seed);
            }
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Byte[] ComputeHashBytes(byte* ptr, int length, uint seed = 0)
        {
            byte[] b = new byte[4];
            fixed (byte* pb = b)
            {
                *((uint*)pb) = xxHash32.UnsafeComputeHash(ptr, length, seed);
            }
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ComputeHashKey(byte[] bytes, uint seed = 0)
        {
            fixed (byte* pa = bytes)
            {
                return xxHash32.UnsafeComputeHash(pa, bytes.Length, seed);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ComputeHashKey(byte* ptr, int length, uint seed = 0)
        {
            return xxHash32.UnsafeComputeHash(ptr, length, seed);
        }
    }
}
