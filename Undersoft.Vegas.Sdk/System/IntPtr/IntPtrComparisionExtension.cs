/*************************************************
   Copyright (c) 2021 Undersoft

   System.IntPtrComparisionExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="IntPtrComparisionExtension" />.
    /// </summary>
    public static class IntPtrComparisionExtension
    {
        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="right">The right<see cref="Int32"/>.</param>
        /// <returns>The <see cref="Int32"/>.</returns>
        public static Int32 CompareTo(this IntPtr left, Int32 right)
        {
            return left.CompareTo((UInt32)right);
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="right">The right<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="Int32"/>.</returns>
        public static Int32 CompareTo(this IntPtr left, IntPtr right)
        {
            if (left.ToUInt64() > right.ToUInt64())
                return 1;

            if (left.ToUInt64() < right.ToUInt64())
                return -1;

            return 0;
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="right">The right<see cref="UInt32"/>.</param>
        /// <returns>The <see cref="Int32"/>.</returns>
        public static Int32 CompareTo(this IntPtr left, UInt32 right)
        {
            if (left.ToUInt64() > right)
                return 1;

            if (left.ToUInt64() < right)
                return -1;

            return 0;
        }

        #endregion
    }
}
