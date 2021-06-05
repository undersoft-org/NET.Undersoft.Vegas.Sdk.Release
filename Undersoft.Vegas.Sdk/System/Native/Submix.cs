/*************************************************
   Copyright (c) 2021 Undersoft

   System.Submix.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Submix
              
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                      
    @version 0.7.1.r.d (Feb 7, 2021)                                            
    @licence MIT                                       
 *********************************************************************************/

namespace System
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="Submix" />.
    /// </summary>
    public static class Submix
    {
        #region Methods

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="int"/>.</param>
        /// <param name="dest">The dest<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Map(int src, int dest)
        {
            return SubmixMap32((uint)src, (uint)dest);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="int"/>.</param>
        /// <param name="dest">The dest<see cref="int"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="uint"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Map(int src, int dest, uint bitmask, int msbid)
        {
            return SubmixMapToMask32((uint)src, (uint)dest, bitmask, msbid);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="long"/>.</param>
        /// <param name="dest">The dest<see cref="long"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Map(long src, long dest)
        {
            return SubmixMap64((ulong)src, (ulong)dest);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="long"/>.</param>
        /// <param name="dest">The dest<see cref="long"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="ulong"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Map(long src, long dest, ulong bitmask, int msbid)
        {
            return SubmixMapToMask64((ulong)src, (ulong)dest, bitmask, msbid);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Map(uint src, uint dest)
        {
            return SubmixMap32(src, dest);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="uint"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Map(uint src, uint dest, uint bitmask, int msbid)
        {
            return SubmixMapToMask32(src, dest, bitmask, msbid);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Map(ulong src, ulong dest)
        {
            return SubmixMap64(src, dest);
        }

        /// <summary>
        /// The Map.
        /// </summary>
        /// <param name="src">The src<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="ulong"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Map(ulong src, ulong dest, ulong bitmask, int msbid)
        {
            return SubmixMapToMask64(src, dest, bitmask, msbid);
        }

        /// <summary>
        /// The Mask.
        /// </summary>
        /// <param name="dest">The dest<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Mask(int dest)
        {
            return SubmixMask32((uint)dest);
        }

        /// <summary>
        /// The Mask.
        /// </summary>
        /// <param name="dest">The dest<see cref="long"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Mask(long dest)
        {
            return SubmixMask64((ulong)dest);
        }

        /// <summary>
        /// The Mask.
        /// </summary>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint Mask(uint dest)
        {
            return SubmixMask32(dest);
        }

        /// <summary>
        /// The Mask.
        /// </summary>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong Mask(ulong dest)
        {
            return SubmixMask64(dest);
        }

        /// <summary>
        /// The MsbId.
        /// </summary>
        /// <param name="dest">The dest<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int MsbId(int dest)
        {
            return SubmixMsbId32((uint)dest);
        }

        /// <summary>
        /// The MsbId.
        /// </summary>
        /// <param name="dest">The dest<see cref="long"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int MsbId(long dest)
        {
            return SubmixMsbId64((ulong)dest);
        }

        /// <summary>
        /// The MsbId.
        /// </summary>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int MsbId(uint dest)
        {
            return SubmixMsbId32(dest);
        }

        /// <summary>
        /// The MsbId.
        /// </summary>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int MsbId(ulong dest)
        {
            return SubmixMsbId64(dest);
        }

        /// <summary>
        /// The SubmixMap32.
        /// </summary>
        /// <param name="src">The src<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint SubmixMap32(uint src, uint dest);

        /// <summary>
        /// The SubmixMap64.
        /// </summary>
        /// <param name="src">The src<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong SubmixMap64(ulong src, ulong dest);

        /// <summary>
        /// The SubmixMapToMask32.
        /// </summary>
        /// <param name="src">The src<see cref="uint"/>.</param>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="uint"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint SubmixMapToMask32(uint src, uint dest, uint bitmask, int msbid);

        /// <summary>
        /// The SubmixMapToMask64.
        /// </summary>
        /// <param name="src">The src<see cref="ulong"/>.</param>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <param name="bitmask">The bitmask<see cref="ulong"/>.</param>
        /// <param name="msbid">The msbid<see cref="int"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong SubmixMapToMask64(ulong src, ulong dest, ulong bitmask, int msbid);

        /// <summary>
        /// The SubmixMask32.
        /// </summary>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint SubmixMask32(uint dest);

        /// <summary>
        /// The SubmixMask64.
        /// </summary>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong SubmixMask64(ulong dest);

        /// <summary>
        /// The SubmixMsbId32.
        /// </summary>
        /// <param name="dest">The dest<see cref="uint"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SubmixMsbId32(uint dest);

        /// <summary>
        /// The SubmixMsbId64.
        /// </summary>
        /// <param name="dest">The dest<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport("Undersoft.System.Native.Submix.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SubmixMsbId64(ulong dest);

        #endregion
    }
}
