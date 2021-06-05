/*************************************************
   Copyright (c) 2021 Undersoft

   System.IntPtrConversionExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="IntPtrConversionExtension" />.
    /// </summary>
    public static class IntPtrConversionExtension
    {
        #region Methods

        /// <summary>
        /// Provides the current address of the given object.
        /// </summary>
        /// <param name="obj">.</param>
        /// <returns>.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public unsafe static System.IntPtr AddressOf(object obj)
        {
            if (obj == null) return System.IntPtr.Zero;

            System.TypedReference reference = __makeref(obj);

            System.TypedReference* pRef = &reference;

            return (IntPtr)pRef; //(&pRef)
        }

        /// <summary>
        /// Provides the current address of the given element.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="t">.</param>
        /// <returns>.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public unsafe static System.IntPtr AddressOf<T>(T t)
        {
            System.TypedReference reference = __makeref(t);

            return *(IntPtr*)(&reference);
        }

        /// <summary>
        /// The AddressOfRef.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="t">The t<see cref="T"/>.</param>
        /// <returns>The <see cref="System.IntPtr"/>.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.PreserveSig)]
        public static unsafe System.IntPtr AddressOfRef<T>(ref T t)
        {
            TypedReference reference = __makeref(t);

            TypedReference* pRef = &reference;

            return (IntPtr)pRef; //(&pRef)
        }

        /// <summary>
        /// The ElementAt.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T ElementAt<T>(this IntPtr ptr, int index)
        {
            var offset = Marshal.SizeOf(typeof(T)) * index;
            var offsetPtr = ptr.Increment(offset);
            return (T)Marshal.PtrToStructure(offsetPtr, typeof(T));
        }

        /// <summary>
        /// The ToIntPtr.
        /// </summary>
        /// <param name="value">The value<see cref="int"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr ToIntPtr(this int value)
        {
            return new IntPtr(value);
        }

        /// <summary>
        /// The ToIntPtr.
        /// </summary>
        /// <param name="value">The value<see cref="long"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr ToIntPtr(this long value)
        {
            unchecked
            {
                if (value > 0 && value <= 0xffffffff)
                    return new IntPtr((int)value);
            }

            return new IntPtr(value);
        }

        /// <summary>
        /// The ToIntPtr.
        /// </summary>
        /// <param name="value">The value<see cref="uint"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr ToIntPtr(this uint value)
        {
            unchecked
            {
                return new IntPtr((int)value);
            }
        }

        /// <summary>
        /// The ToIntPtr.
        /// </summary>
        /// <param name="value">The value<see cref="ulong"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr ToIntPtr(this ulong value)
        {
            unchecked
            {
                return new IntPtr((long)value);
            }
        }

        /// <summary>
        /// The ToUInt32.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="UInt32"/>.</returns>
        public unsafe static UInt32 ToUInt32(this IntPtr pointer)
        {
            return (UInt32)((void*)pointer);
        }

        /// <summary>
        /// The ToUInt64.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public unsafe static UInt64 ToUInt64(this IntPtr pointer)
        {
            return (UInt64)((void*)pointer);
        }

        #endregion
    }
}
