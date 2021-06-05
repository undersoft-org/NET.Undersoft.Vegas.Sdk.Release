/*************************************************
   Copyright (c) 2021 Undersoft

   System.IntPtrLogicExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="IntPtrLogicExtension" />.
    /// </summary>
    public static class IntPtrLogicExtension
    {
        #region Methods

        /// <summary>
        /// The And.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr And(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() & value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() & value.ToInt64()));
            }
        }

        /// <summary>
        /// The Not.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Not(this IntPtr pointer)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(~pointer.ToInt32()));

                default:
                    return (new IntPtr(~pointer.ToInt64()));
            }
        }

        /// <summary>
        /// The Or.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Or(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() | value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() | value.ToInt64()));
            }
        }

        /// <summary>
        /// The Xor.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static IntPtr Xor(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() ^ value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() ^ value.ToInt64()));
            }
        }

        #endregion
    }
}
