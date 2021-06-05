/*************************************************
   Copyright (c) 2021 Undersoft

   System.Int32Extension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="Int32Extension" />.
    /// </summary>
    public static class Int32Extension
    {
        #region Methods

        /// <summary>
        /// The CountLeadingZeros.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint CountLeadingZeros(this int i)
        {
            return Bitscan.LengthAfter32((uint)i);
        }

        /// <summary>
        /// The CountTrailingZeros.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint CountTrailingZeros(this int i)
        {
            return Bitscan.LengthBefore32((uint)i);
        }

        /// <summary>
        /// The HighestBitId.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint HighestBitId(this int i)
        {
            return Bitscan.ReverseIndex32((uint)i);
        }

        /// <summary>
        /// The IsEven.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsEven(this int i)
        {
            return !((i & 1) != 0);
        }

        /// <summary>
        /// The IsOdd.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsOdd(this int i)
        {
            return ((i & 1) != 0);
        }

        /// <summary>
        /// The LowestBitId.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint LowestBitId(this int i)
        {
            return Bitscan.ForwardIndex32((uint)i);
        }

        /// <summary>
        /// The RemoveSign.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int RemoveSign(this int i)
        {
            return (int)(((uint)i << 1) >> 1);
        }

        #endregion
    }
}
