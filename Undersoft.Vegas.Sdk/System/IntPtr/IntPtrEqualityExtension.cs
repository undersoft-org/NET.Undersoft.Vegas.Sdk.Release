/*************************************************
   Copyright (c) 2021 Undersoft

   System.IntPtrEqualityExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="IntPtrEqualityExtension" />.
    /// </summary>
    public static class IntPtrEqualityExtension
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="ptr2">The ptr2<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr left, IntPtr ptr2)
        {
            return (left == ptr2);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int32"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr pointer, Int32 value)
        {
            return (pointer.ToInt32() == value);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="Int64"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr pointer, Int64 value)
        {
            return (pointer.ToInt64() == value);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="UInt32"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr pointer, UInt32 value)
        {
            return (pointer.ToUInt32() == value);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="value">The value<see cref="UInt64"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr pointer, UInt64 value)
        {
            return (pointer.ToUInt64() == value);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr[]"/>.</param>
        /// <param name="right">The right<see cref="IntPtr[]"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean Equals(this IntPtr[] left, IntPtr[] right)
        {
            int length = left.Length;
            for (int i = 0; i < length; i++)
                if (!left[i].Equals(right[i]))
                    return false;
            return true;
        }

        /// <summary>
        /// The isGreaterThanOrEqualTo.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="right">The right<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean isGreaterThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) >= 0);
        }

        /// <summary>
        /// The IsLessThanOrEqualTo.
        /// </summary>
        /// <param name="left">The left<see cref="IntPtr"/>.</param>
        /// <param name="right">The right<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="Boolean"/>.</returns>
        public static Boolean IsLessThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) <= 0);
        }

        #endregion
    }
}
