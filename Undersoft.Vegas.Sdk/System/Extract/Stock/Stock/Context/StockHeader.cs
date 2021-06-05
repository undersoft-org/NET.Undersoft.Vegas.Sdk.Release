/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.Stock.StockHeader.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract.Stock
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// A structure that is always located at the start of the shared memory in a <see cref="Stock"/> instance. 
    /// This allows the shared memory to be opened by other instances without knowing its size before hand.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StockHeader
    {
        #region Fields

        public long FreeMemorySize;
        public long ItemCapacity;
        public int ItemCount;
        public int ItemSize;
        public ushort SectorId;
        public long SharedMemorySize;
        public volatile int Shutdown;
        public ushort StockId;
        public long UsedMemorySize;

        #endregion
    }
}
