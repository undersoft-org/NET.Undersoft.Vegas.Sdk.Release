/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealStream.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;
    using System.IO;
    using System.Net.Sockets;

    /// <summary>
    /// Defines the <see cref="DealStream" />.
    /// </summary>
    public sealed class DealStream : Stream
    {
        #region Constants

        internal const int readLimit = 4194304;
        internal const int writeLimit = 65536;

        #endregion

        #region Fields

        // GLOBALS CLASS HELPER PROPERTIES
        private Socket socket;
        private int timeout = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealStream"/> class.
        /// </summary>
        /// <param name="socketToStream">The socketToStream<see cref="Socket"/>.</param>
        public DealStream(Socket socketToStream)
        {
            socket = socketToStream;
            if (socket == null)
                throw new ArgumentNullException("socket");
        }

        #endregion

        #region Properties

        // PROPERTIES FOR STREAM REWRITE
        /// <summary>
        /// Gets a value indicating whether CanRead.
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether CanSeek.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether CanWrite.
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException("dont't ever use in net kind of streams"); }
        }

        // METHODS NOT SUPPORTED OR DISABLED IData NET KIDataD OF STREAM
        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        public override long Position
        {
            get { throw new NotSupportedException("dont't ever use in net kind of streams"); }
            set { throw new NotSupportedException("dont't ever use in net kind of streams"); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BeginRead.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="asyncCallback">The asyncCallback<see cref="AsyncCallback"/>.</param>
        /// <param name="contextObject">The contextObject<see cref="object"/>.</param>
        /// <returns>The <see cref="IAsyncResult"/>.</returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object contextObject)
        {
            if (size >= readLimit) { throw new NotSupportedException("reach read Limit 4MB"); }
            IAsyncResult result = socket.BeginReceive(buffer, offset, size, SocketFlags.None, asyncCallback, contextObject);
            return result;
        }

        // ASYNCHROUS METHODS FOR STREAM REWRITE
        /// <summary>
        /// The BeginWrite.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="asyncCallback">The asyncCallback<see cref="AsyncCallback"/>.</param>
        /// <param name="contextObject">The contextObject<see cref="object"/>.</param>
        /// <returns>The <see cref="IAsyncResult"/>.</returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object contextObject)
        {
            IAsyncResult result = socket.BeginSend(buffer, offset, size, SocketFlags.None, asyncCallback, contextObject);
            return result;
        }

        /// <summary>
        /// The EndRead.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

        /// <summary>
        /// The EndWrite.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            socket.EndSend(asyncResult);
        }

        /// <summary>
        /// The Flush.
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException("don't use its a empty method for future use maybe");
        }

        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int Read(byte[] buffer, int offset, int size)
        {
            if (timeout <= 0)
            {
                if (size >= readLimit) { throw new NotSupportedException("reach read Limit 64K"); }
                return socket.Receive(buffer, offset, Math.Min(size, readLimit), SocketFlags.None);
            }
            else
            {
                if (size >= readLimit) { throw new NotSupportedException("reach read Limit 64K"); }
                IAsyncResult ar = socket.BeginReceive(buffer, offset, size, SocketFlags.None, null, null);
                if (timeout > 0 && !ar.IsCompleted)
                {
                    ar.AsyncWaitHandle.WaitOne(timeout, false);
                    if (!ar.IsCompleted)
                        throw new Exception();

                }
                return socket.EndReceive(ar);
            }
        }

        /// <summary>
        /// The Seek.
        /// </summary>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <param name="origin">The origin<see cref="SeekOrigin"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("dont't ever use in socket kind of streams");
        }

        /// <summary>
        /// The SetLength.
        /// </summary>
        /// <param name="value">The value<see cref="long"/>.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("dont't ever use in net kind of streams");
        }

        // SYNCHROUS METHODS FOR STREAM REWRITE
        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        public override void Write(byte[] buffer, int offset, int size)
        {
            int tempSize = size;
            while (tempSize > 0)
            {
                size = Math.Min(tempSize, writeLimit);
                socket.Send(buffer, offset, size, SocketFlags.None);
                tempSize -= size;
                offset += size;
            }
        }

        // CLOSE SOCKET BY DISPOSE METHOD REWRITE
        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposeIt">The disposeIt<see cref="bool"/>.</param>
        protected override void Dispose(bool disposeIt)
        {
            try
            {
                if (disposeIt)
                    socket.Close();
            }
            finally
            {
                base.Dispose(disposeIt);
            }
        }

        #endregion
    }
}
