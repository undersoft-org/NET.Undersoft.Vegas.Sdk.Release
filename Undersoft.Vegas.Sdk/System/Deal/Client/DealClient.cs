/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealClient.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Instant;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="DealClient" />.
    /// </summary>
    public sealed class DealClient : IDealClient
    {
        #region Fields

        private readonly ManualResetEvent connectNotice = new ManualResetEvent(false);
        public IPEndPoint EndPoint;
        private ITransferContext context;
        private IPHostEntry host;
        private MemberIdentity identity;
        private IPAddress ip;
        private ushort port;
        private Socket socket;
        private int timeout = 50;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealClient"/> class.
        /// </summary>
        /// <param name="ConnectionIdentity">The ConnectionIdentity<see cref="MemberIdentity"/>.</param>
        public DealClient(MemberIdentity ConnectionIdentity)
        {
            Identity = ConnectionIdentity;

            if (Identity.Ip == null || Identity.Ip == "")
                Identity.Ip = "127.0.0.1";
            ip = IPAddress.Parse(Identity.Ip);
            port = Convert.ToUInt16(Identity.Port);
            host = Dns.GetHostEntry((Identity.Ip != null &&
                                     Identity.Ip != string.Empty) ?
                                     Identity.Ip :
                                     string.Empty);

            EndPoint = new IPEndPoint(ip, port);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Connected.
        /// </summary>
        public IDeputy Connected { get; set; }

        /// <summary>
        /// Gets or sets the Context.
        /// </summary>
        public ITransferContext Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Gets or sets the HeaderReceived.
        /// </summary>
        public IDeputy HeaderReceived { get; set; }

        /// <summary>
        /// Gets or sets the HeaderSent.
        /// </summary>
        public IDeputy HeaderSent { get; set; }

        /// <summary>
        /// Gets or sets the Identity.
        /// </summary>
        public MemberIdentity Identity
        {
            get
            {
                return (identity != null) ?
                                 identity :
                                 identity = new MemberIdentity()
                                 {
                                     Id = 0,
                                     Ip = "127.0.0.1",
                                     Host = "localhost",
                                     Port = 44004,
                                     Limit = 0,
                                     Scale = 0,
                                     Site = ServiceSite.Client
                                 };
            }
            set
            {
                if (value != null)
                {
                    value.Site = ServiceSite.Client;
                    identity = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the MessageReceived.
        /// </summary>
        public IDeputy MessageReceived { get; set; }

        /// <summary>
        /// Gets or sets the MessageSent.
        /// </summary>
        public IDeputy MessageSent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Connect.
        /// </summary>
        public void Connect()
        {
            ushort _port = port;
            string hostname = host.HostName;
            IPAddress _ip = ip;
            IPEndPoint endpoint = new IPEndPoint(_ip, _port);

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                context = new TransferContext(socket);
                socket.BeginConnect(endpoint, OnConnectCallback, context);
                connectNotice.WaitOne();

                Connected.Execute(this);
            }
            catch (SocketException ex)
            { }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            connectNotice.Dispose();
            Close();
        }

        /// <summary>
        /// The IsConnected.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsConnected()
        {
            if (socket != null && socket.Connected)
                return !(socket.Poll(timeout * 1000, SelectMode.SelectRead) && socket.Available == 0);
            return true;
        }

        /// <summary>
        /// The Receive.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        public void Receive(MessagePart messagePart)
        {
            AsyncCallback callback = HeaderReceivedCallBack;
            if (messagePart != MessagePart.Header && context.ReceiveMessage)
            {
                callback = MessageReceivedCallBack;
                context.ObjectsLeft = context.Transfer.HeaderReceived.Context.ObjectsCount;
                context.Listener.BeginReceive(context.MessageBuffer, 0, context.BufferSize, SocketFlags.None, callback, context);
            }
            else
                context.Listener.BeginReceive(context.HeaderBuffer, 0, context.BufferSize, SocketFlags.None, callback, context);
        }

        /// <summary>
        /// The Send.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        public void Send(MessagePart messagePart)
        {
            if (!IsConnected())
                throw new Exception("Destination socket is not connected.");
            AsyncCallback callback = HeaderSentCallback;
            if (messagePart == MessagePart.Header)
            {
                callback = HeaderSentCallback;
                TransferOperation request = new TransferOperation(Context.Transfer, MessagePart.Header, DirectionType.Send);
                request.Resolve();
            }
            else if (Context.SendMessage)
            {
                callback = MessageSentCallback;
                context.SerialBlockId = 0;
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Message, DirectionType.Send);
                request.Resolve();
            }
            else
                return;
            context.Listener.BeginSend(context.SerialBlock, 0, context.SerialBlock.Length, SocketFlags.None, callback, context);
        }

        /// <summary>
        /// The Close.
        /// </summary>
        private void Close()
        {
            try
            {
                if (!IsConnected())
                {
                    context.Dispose();
                    return;
                }
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                context.Dispose();
            }
            catch (SocketException)
            {
                // 4 U 2 DO
            }
        }

        /// <summary>
        /// The HeaderReceivedCallBack.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        private void HeaderReceivedCallBack(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            int receive = context.Listener.EndReceive(result);

            if (receive > 0)
                context.IncomingHeader(receive);

            if (context.BlockSize > 0)
            {
                int buffersize = (context.BlockSize < context.BufferSize) ? (int)context.BlockSize : context.BufferSize;
                context.Listener.BeginReceive(context.HeaderBuffer, 0, buffersize, SocketFlags.None, HeaderReceivedCallBack, context);
            }
            else
            {
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Header, DirectionType.Receive);
                request.Resolve(context);

                if (!context.ReceiveMessage &&
                    !context.SendMessage)
                    context.Close = true;

                context.HeaderReceivedNotice.Set();
                HeaderReceived.Execute(this);
            }
        }

        /// <summary>
        /// The HeaderSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        private void HeaderSentCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            try
            {
                int sendcount = context.Listener.EndSend(result);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }

            context.HeaderSentNotice.Set();
            HeaderSent.Execute(this);
        }

        /// <summary>
        /// The MessageReceivedCallBack.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        private void MessageReceivedCallBack(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            MarkupType noiseKind = MarkupType.None;

            int receive = context.Listener.EndReceive(result);

            if (receive > 0)
                noiseKind = context.IncomingMessage(receive);

            if (context.BlockSize > 0)
            {
                int buffersize = (context.BlockSize < context.BufferSize) ? (int)context.BlockSize : context.BufferSize;
                context.Listener.BeginReceive(context.MessageBuffer, 0, buffersize, SocketFlags.None, MessageReceivedCallBack, context);
            }
            else
            {
                object readPosition = context.DeserialBlockId;

                if (noiseKind == MarkupType.Block || (noiseKind == MarkupType.End && (int)readPosition < (context.Transfer.HeaderReceived.Context.ObjectsCount - 1)))
                    context.Listener.BeginReceive(context.MessageBuffer, 0, context.BufferSize, SocketFlags.None, MessageReceivedCallBack, context);

                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Message, DirectionType.Receive);
                request.Resolve(context);

                if (context.ObjectsLeft <= 0 && !context.BatchesReceivedNotice.SafeWaitHandle.IsClosed)
                    context.BatchesReceivedNotice.Set();

                if (noiseKind == MarkupType.End && (int)readPosition >= (context.Transfer.HeaderReceived.Context.ObjectsCount - 1))
                {
                    context.BatchesReceivedNotice.WaitOne();

                    if (context.SendMessage)
                        context.MessageSentNotice.WaitOne();

                    context.Close = true;

                    context.MessageReceivedNotice.Set();
                    MessageReceived.Execute(this);
                }
            }
        }

        /// <summary>
        /// The MessageSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        private void MessageSentCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            try
            {
                int sendcount = context.Listener.EndSend(result);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }

            if (context.SerialBlockId >= 0)
            {
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Message, DirectionType.Send);
                request.Resolve();
                context.Listener.BeginSend(context.SerialBlock, 0, context.SerialBlock.Length, SocketFlags.None, MessageSentCallback, context);
            }
            else
            {
                if (!context.ReceiveMessage)
                    context.Close = true;

                context.MessageSentNotice.Set();
                MessageSent.Execute(this);
            }
        }

        /// <summary>
        /// The OnConnectCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        private void OnConnectCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;

            try
            {
                context.Listener.EndConnect(result);
                connectNotice.Set();
            }
            catch (SocketException ex)
            {
            }
        }

        #endregion
    }
}
