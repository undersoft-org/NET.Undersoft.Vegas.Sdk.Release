/*************************************************
   Copyright (c) 2021 Undersoft

   System.UInt32Extension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="UInt32Extension" />.
    /// </summary>
    public static class UInt32Extension
    {
        #region Methods

        /// <summary>
        /// The CountLeadingZeros.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint CountLeadingZeros(this uint i)
        {
            return Bitscan.LengthAfter32(i);
        }

        /// <summary>
        /// The CountTrailingZeros.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint CountTrailingZeros(this uint i)
        {
            return Bitscan.LengthBefore32(i);
        }

        /// <summary>
        /// The HighestBitId.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint HighestBitId(this uint i)
        {
            return Bitscan.ReverseIndex32(i);
        }

        /// <summary>
        /// The IsEven.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsEven(this uint i)
        {
            return !((i & 1) != 0);
        }

        /// <summary>
        /// The IsOdd.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsOdd(this uint i)
        {
            return ((i & 1) != 0);
        }

        /// <summary>
        /// The LowestBitId.
        /// </summary>
        /// <param name="i">The i<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint LowestBitId(this uint i)
        {
            return Bitscan.ForwardIndex32(i);
        }

        #endregion
    }
}
