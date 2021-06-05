/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.Stock.StockContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract.Stock
{
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="StockContext" />.
    /// </summary>
    public unsafe sealed class StockContext : IStockContext, IDisposable
    {
        #region Fields

        public IntPtr binReceivePtr;
        public IntPtr binSendPtr;
        public int ClientWaitCount = 0;
        public int ReadCount = 0;
        public int ServerWaitCount = 0;
        public int WriteCount = 0;
        private byte[] binReceive = new byte[0];
        private byte[] binSend = new byte[0];
        private MemoryStream msRead = new MemoryStream();
        private MemoryStream msReceive = new MemoryStream();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StockContext"/> class.
        /// </summary>
        public StockContext()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BlockOffset.
        /// </summary>
        public int BlockOffset { get; set; } = 16;

        /// <summary>
        /// Gets or sets the BlockSize.
        /// </summary>
        public long BlockSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the BufferSize.
        /// </summary>
        public long BufferSize { get; set; } = 1048576;

        /// <summary>
        /// Gets or sets the ClientCount.
        /// </summary>
        public int ClientCount { get; set; } = 1;

        /// <summary>
        /// Gets the DeserialBlock.
        /// </summary>
        public byte[] DeserialBlock
        {
            get
            {
                byte[] result = null;
                lock (binReceive)
                {
                    BlockSize = 0;
                    result = binReceive;
                    binReceive = new byte[0];
                }
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the DeserialBlockId.
        /// </summary>
        public int DeserialBlockId { get; set; } = 0;

        /// <summary>
        /// Gets the DeserialBlockPtr.
        /// </summary>
        public IntPtr DeserialBlockPtr
        {
            get { return GCHandle.FromIntPtr(binReceivePtr).AddrOfPinnedObject() + BlockOffset; }
        }

        /// <summary>
        /// Gets or sets the Elements.
        /// </summary>
        public int Elements { get; set; } = 1;

        /// <summary>
        /// Gets or sets the File.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the FreeSize.
        /// </summary>
        public long FreeSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the ItemCapacity.
        /// </summary>
        public long ItemCapacity { get; set; } = -1;

        /// <summary>
        /// Gets or sets the ItemCount.
        /// </summary>
        public int ItemCount { get; set; } = -1;

        /// <summary>
        /// Gets or sets the ItemSize.
        /// </summary>
        public int ItemSize { get; set; } = -1;

        /// <summary>
        /// Gets or sets the NodeCount.
        /// </summary>
        public int NodeCount { get; set; } = 50;

        /// <summary>
        /// Gets or sets the ObjectPosition.
        /// </summary>
        public int ObjectPosition { get; set; } = 0;

        /// <summary>
        /// Gets or sets the ObjectsLeft.
        /// </summary>
        public int ObjectsLeft { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the SectorId.
        /// </summary>
        public ushort SectorId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the SerialBlock.
        /// </summary>
        public byte[] SerialBlock
        {
            get
            {
                return binSend;
            }
            set
            {
                binSend = value;
                if (binSend != null && BlockOffset > 0)
                {
                    long size = binSend.Length - BlockOffset;
                    new byte[] { (byte) 'V',
                                 (byte) 'S',
                                 (byte) 'S',
                                 (byte) 'P' }.CopyTo(binSend, 0);
                    size.GetBytes().CopyTo(binSend, 4);
                    ObjectPosition.GetBytes().CopyTo(binSend, 12);
                    GCHandle gc = GCHandle.Alloc(binSend, GCHandleType.Pinned);
                    binSendPtr = GCHandle.ToIntPtr(gc);
                }
            }
        }

        /// <summary>
        /// Gets or sets the SerialBlockId.
        /// </summary>
        public int SerialBlockId { get; set; } = 0;

        /// <summary>
        /// Gets the SerialBlockPtr.
        /// </summary>
        public IntPtr SerialBlockPtr
        {
            get { return GCHandle.FromIntPtr(binSendPtr).AddrOfPinnedObject(); }
        }

        /// <summary>
        /// Gets or sets the ServerCount.
        /// </summary>
        public int ServerCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the Site.
        /// </summary>
        public ServiceSite Site { get; set; }

        /// <summary>
        /// Gets or sets the StockId.
        /// </summary>
        public ushort StockId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the UsedSize.
        /// </summary>
        public long UsedSize { get; set; } = 0;

        #endregion

        #region Methods

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            msRead.Dispose();
            msReceive.Dispose();
            if (!binReceivePtr.Equals(IntPtr.Zero))
            {
                GCHandle gc = GCHandle.FromIntPtr(binReceivePtr);
                gc.Free();
            }
            if (!binSendPtr.Equals(IntPtr.Zero))
            {
                GCHandle gc = GCHandle.FromIntPtr(binSendPtr);
                gc.Free();
            }
            binReceive = null;
            binSend = null;
        }

        /// <summary>
        /// The ReadStock.
        /// </summary>
        /// <param name="drive">The drive<see cref="IStock"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object ReadStock(IStock drive)
        {
            if (drive != null)
            {
                drive.ReadHeader();
                BufferSize = drive.BufferSize;
                byte[] bufferread = new byte[BufferSize];
                GCHandle handler = GCHandle.Alloc(bufferread, GCHandleType.Pinned);
                IntPtr rawpointer = handler.AddrOfPinnedObject();
                drive.Read(rawpointer, BufferSize, 0L);
                ReceiveBytes(bufferread, BufferSize);
                handler.Free();
            }
            return DeserialBlock;
        }

        /// <summary>
        /// The ReadStockPtr.
        /// </summary>
        /// <param name="drive">The drive<see cref="IStock"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public IntPtr ReadStockPtr(IStock drive)
        {
            if (drive != null)
            {
                drive.ReadHeader();
                BufferSize = drive.BufferSize;
                binReceive = new byte[BufferSize];
                GCHandle handler = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                binReceivePtr = GCHandle.ToIntPtr(handler);
                IntPtr rawpointer = handler.AddrOfPinnedObject();
                drive.Read(rawpointer, BufferSize, 0);
                ReceiveBytes(rawpointer, BufferSize);
            }
            return DeserialBlockPtr;
        }

        /// <summary>
        /// The ReceiveBytes.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public MarkupType ReceiveBytes(byte[] buffer, int received)
        {
            MarkupType noiseKind = MarkupType.None;
            lock (binReceive)
            {
                int offset = 0, length = received;
                bool inprogress = false;

                if (BlockSize == 0)
                {
                    BlockSize = BitConverter.ToInt64(buffer, 4);
                    DeserialBlockId = BitConverter.ToInt32(buffer, 12);
                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceivePtr = GCHandle.ToIntPtr(gc);
                    offset = BlockOffset;
                    length -= BlockOffset;

                }
                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = received;
                    noiseKind = buffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }
                int destid = (binReceive.Length - ((int)BlockSize + length));
                if (inprogress)
                {
                    fixed (void* msgbuff = buffer)
                    {
                        Extractor.CopyBlock(GCHandle.FromIntPtr(binReceivePtr).AddrOfPinnedObject().ToPointer(), (ulong)destid, msgbuff, (ulong)offset, (ulong)length);
                    }
                }
            }
            return noiseKind;
        }

        /// <summary>
        /// The ReceiveBytes.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="received">The received<see cref="long"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public MarkupType ReceiveBytes(byte[] buffer, long received)
        {

            MarkupType noiseKind = MarkupType.None;
            lock (binReceive)
            {
                int offset = 0, length = (int)received;
                bool inprogress = false;
                if (BlockSize == 0)
                {

                    BlockSize = BitConverter.ToInt64(buffer, 4);
                    DeserialBlockId = BitConverter.ToInt32(buffer, 12);
                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceivePtr = GCHandle.ToIntPtr(gc);
                    offset = BlockOffset;
                    length -= BlockOffset;
                }

                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = received;
                    noiseKind = buffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }

                int destid = (binReceive.Length - ((int)BlockSize + length));
                if (inprogress)
                {
                    fixed (byte* msgbuff = buffer)
                    {
                        Extractor.CopyBlock(GCHandle.FromIntPtr(binReceivePtr).AddrOfPinnedObject().ToPointer(), (ulong)destid, msgbuff, (ulong)offset, (ulong)length);
                        //  Extractor.CopyBlock(GCHandle.FromIntPtr(binReceivePtr).AddrOfPinnedObject() + destid, new IntPtr(msgbuff) + offset, (ulong)length);
                    }
                }
            }
            return noiseKind;
        }

        /// <summary>
        /// The ReceiveBytes.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="IntPtr"/>.</param>
        /// <param name="received">The received<see cref="long"/>.</param>
        public void ReceiveBytes(IntPtr buffer, long received)
        {
            lock (binReceive)
            {
                BlockSize = *((int*)(buffer + 4));
                DeserialBlockId = *((int*)(buffer + 12));
            }
        }

        /// <summary>
        /// The WriteStock.
        /// </summary>
        /// <param name="drive">The drive<see cref="IStock"/>.</param>
        public void WriteStock(IStock drive)
        {
            if (drive != null)
            {
                GCHandle handler = GCHandle.Alloc(SerialBlock, GCHandleType.Pinned);
                IntPtr rawpointer = handler.AddrOfPinnedObject();
                drive.BufferSize = SerialBlock.Length;
                drive.WriteHeader();
                drive.Write(rawpointer, SerialBlock.Length);
                handler.Free();
            }
        }

        /// <summary>
        /// The WriteStockPtr.
        /// </summary>
        /// <param name="drive">The drive<see cref="IStock"/>.</param>
        public void WriteStockPtr(IStock drive)
        {
            if (drive != null)
            {
                drive.BufferSize = BlockSize;
                drive.WriteHeader();
                drive.Write(SerialBlockPtr, BlockSize);
            }
        }

        #endregion
    }
}
