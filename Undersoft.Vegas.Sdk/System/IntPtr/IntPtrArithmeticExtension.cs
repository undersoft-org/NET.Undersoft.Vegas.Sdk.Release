/*************************************************
   Copyright (c) 2021 Undersoft

   System.IntPtrArithmeticExtension.cs
   
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
    /// Defines the <see cref="IntPtrArithmeticExtension" />.
    /// </summary>
    public static class IntPtrArithmeticExtension
    {
        #region Methods

        /// <summary>
        /// The Decrement.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int32"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Decrement(this IntPtr pointer, Int32 value)
        {
            return Increment(pointer, -value);
        }

        /// <summary>
        /// The Decrement.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int64"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Decrement(this IntPtr pointer, Int64 value)
        {
            return Increment(pointer, -value);
        }

        /// <summary>
        /// The Decrement.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Decrement(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() - value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() - value.ToInt64()));
            }
        }

        /// <summary>
        /// The Increment.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int32"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Increment(this IntPtr pointer, Int32 value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        return (new IntPtr(pointer.ToInt32() + value));

                    default:
                        return (new IntPtr(pointer.ToInt64() + value));
                }
            }
        }

        /// <summary>
        /// The Increment.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int64"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Increment(this IntPtr pointer, Int64 value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        return (new IntPtr((Int32)(pointer.ToInt32() + value)));

                    default:
                        return (new IntPtr(pointer.ToInt64() + value));
                }
            }
        }

        /// <summary>
        /// The Increment.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Increment(this IntPtr pointer, IntPtr value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(int):
                        return new IntPtr(pointer.ToInt32() + value.ToInt32());
                    default:
                        return new IntPtr(pointer.ToInt64() + value.ToInt64());
                }
            }
        }

        /// <summary>
        /// The Increment.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Increment<T>(this IntPtr ptr)
        {
            return ptr.Increment(Marshal.SizeOf(typeof(T)));
        }

        #endregion
    }
}
