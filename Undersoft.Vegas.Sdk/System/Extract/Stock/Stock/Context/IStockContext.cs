/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.Stock.IStockContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract.Stock
{
    using System;

    /// <summary>
    /// Defines the <see cref="IStockContext" />.
    /// </summary>
    public interface IStockContext : ISerialBuffer, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the BufferSize.
        /// </summary>
        long BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the ClientCount.
        /// </summary>
        int ClientCount { get; set; }

        /// <summary>
        /// Gets or sets the Elements.
        /// </summary>
        int Elements { get; set; }

        /// <summary>
        /// Gets or sets the File.
        /// </summary>
        string File { get; set; }

        /// <summary>
        /// Gets or sets the FreeSize.
        /// </summary>
        long FreeSize { get; set; }

        /// <summary>
        /// Gets or sets the ItemCapacity.
        /// </summary>
        long ItemCapacity { get; set; }

        /// <summary>
        /// Gets or sets the ItemCount.
        /// </summary>
        int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the ItemSize.
        /// </summary>
        int ItemSize { get; set; }

        /// <summary>
        /// Gets or sets the NodeCount.
        /// </summary>
        int NodeCount { get; set; }

        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets or sets the SectorId.
        /// </summary>
        ushort SectorId { get; set; }

        /// <summary>
        /// Gets or sets the ServerCount.
        /// </summary>
        int ServerCount { get; set; }

        /// <summary>
        /// Gets or sets the StockId.
        /// </summary>
        ushort StockId { get; set; }

        /// <summary>
        /// Gets or sets the UsedSize.
        /// </summary>
        long UsedSize { get; set; }

        #endregion
    }
}
