/*************************************************
   Copyright (c) 2021 Undersoft

   System.Bitscan.cs
   
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
    /// Defines the <see cref="Bitscan" />.
    /// </summary>
    public static class Bitscan
    {
        #region Methods

        /// <summary>
        /// The ForwardIndex32.
        /// </summary>
        /// <param name="x">The x<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ForwardIndex32(uint x);

        /// <summary>
        /// The ForwardIndex64.
        /// </summary>
        /// <param name="x">The x<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ForwardIndex64(ulong x);

        /// <summary>
        /// The LengthAfter32.
        /// </summary>
        /// <param name="x">The x<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint LengthAfter32(uint x);

        /// <summary>
        /// The LengthAfter64.
        /// </summary>
        /// <param name="x">The x<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint LengthAfter64(ulong x);

        /// <summary>
        /// The LengthBefore32.
        /// </summary>
        /// <param name="x">The x<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint LengthBefore32(uint x);

        /// <summary>
        /// The LengthBefore64.
        /// </summary>
        /// <param name="x">The x<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint LengthBefore64(ulong x);

        /// <summary>
        /// The ReverseIndex32.
        /// </summary>
        /// <param name="x">The x<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ReverseIndex32(uint x);

        /// <summary>
        /// The ReverseIndex64.
        /// </summary>
        /// <param name="x">The x<see cref="ulong"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ReverseIndex64(ulong x);

        #endregion
    }
}
