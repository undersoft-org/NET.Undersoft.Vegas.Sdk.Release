/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealSocketListener.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (02.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;
    using System.Instant;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Sets;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="DealListener" />.
    /// </summary>
    public sealed class DealListener : IDealListener
    {
        #region Fields

        private readonly Catalog<ITransferContext> clients =
                     new Catalog<ITransferContext>();
        private readonly ManualResetEvent connectingNotice = new ManualResetEvent(false);
        private readonly Catalog<byte[]> resources =
                     new Catalog<byte[]>();
        private static IMemberSecurity security;
        private MemberIdentity identity;
        private bool shutdown = false;
        private int timeout = 50;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DealListener"/> class from being created.
        /// </summary>
        private DealListener()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static IDealListener Instance { get; } = new DealListener();

        /// <summary>
        /// Gets or sets the Security.
        /// </summary>
        public static IMemberSecurity Security
        {
            get { return security; }
            set { security = value; }
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
                                     Port = 28465,
                                     Limit = 200,
                                     Scale = 1,
                                     Site = ServiceSite.Server
                                 };
            }
            set
            {
                if (value != null)
                {
                    value.Site = ServiceSite.Server;
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

        /// <summary>
        /// Gets or sets the SendEcho.
        /// </summary>
        public IDeputy SendEcho { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The ClearClients.
        /// </summary>
        public void ClearClients()
        {
            foreach (ITransferContext closeContext in clients.AsValues())
            {
                ITransferContext context = closeContext;

                if (context == null)
                {
                    throw new Exception("Client does not exist.");
                }

                try
                {
                    context.Listener.Shutdown(SocketShutdown.Both);
                    context.Listener.Close();
                }
                catch (SocketException sx)
                {
                    Echo(sx.Message);
                }
                finally
                {
                    context.Dispose();
                    Echo(string.Format("Client disconnected with Id {0}", context.Id));
                }
            }
            clients.Clear();
        }

        /// <summary>
        /// The ClearResources.
        /// </summary>
        public void ClearResources()
        {
            resources.Clear();
        }

        /// <summary>
        /// The CloseClient.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{ITransferContext}"/>.</param>
        public void CloseClient(ICard<ITransferContext> card)
        {
            ITransferContext context = card.Value;

            if (context == null)
            {
                Echo(string.Format("Client {0} does not exist.", context.Id));
            }
            else
            {
                try
                {
                    if (context.Listener != null && context.Listener.Connected)
                    {
                        context.Listener.Shutdown(SocketShutdown.Both);
                        context.Listener.Close();
                    }
                }
                catch (SocketException sx)
                {
                    Echo(sx.Message);
                }
                finally
                {
                    ITransferContext contextRemoved = clients.Remove(context.Id);
                    contextRemoved.Dispose();
                    Echo(string.Format("Client disconnected with Id {0}", context.Id));
                }
            }
        }

        /// <summary>
        /// The CloseClient.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        public void CloseClient(int id)
        {
            CloseClient(GetClient(id));
        }

        /// <summary>
        /// The CloseListener.
        /// </summary>
        public void CloseListener()
        {
            foreach (ITransferContext closeContext in clients.AsValues())
            {
                ITransferContext context = closeContext;

                if (context == null)
                {
                    Echo(string.Format("Client  does not exist."));
                }
                else
                {
                    try
                    {
                        if (context.Listener != null && context.Listener.Connected)
                        {
                            context.Listener.Shutdown(SocketShutdown.Both);
                            context.Listener.Close();
                        }
                    }
                    catch (SocketException sx)
                    {
                        Echo(sx.Message);
                    }
                    finally
                    {
                        context.Dispose();
                        Echo(string.Format("Client disconnected with Id {0}", context.Id));
                    }
                }
            }
            clients.Clear();
            shutdown = true;
            connectingNotice.Set();
            GC.Collect();
        }

        /// <summary>
        /// The DealHeaderReceived.
        /// </summary>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        public void DealHeaderReceived(ITransferContext context)
        {
            if (context.BlockSize > 0)
            {
                int buffersize = (context.BlockSize < context.BufferSize) ? (int)context.BlockSize : context.BufferSize;
                context.Listener.BeginReceive(context.HeaderBuffer, 0, buffersize, SocketFlags.None, HeaderReceivedCallback, context);
            }
            else
            {
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Header, DirectionType.Receive);
                request.Resolve(context);

                context.HeaderReceivedNotice.Set();

                try
                {
                    HeaderReceived.Execute(context);
                }
                catch (Exception ex)
                {
                    Echo(ex.Message);
                    CloseClient(context.Id);
                }
            }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            foreach (var card in clients.AsCards())
            {
                CloseClient(card);
            }

            connectingNotice.Dispose();
        }

        /// <summary>
        /// The Echo.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public void Echo(string message)
        {
            if (SendEcho != null)
                SendEcho.Execute(message);
        }

        /// <summary>
        /// The HeaderReceivedCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        public void HeaderReceivedCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            int receive = context.Listener.EndReceive(result);

            if (receive > 0)
                context.IncomingHeader(receive);

            if (context.Protocol == DealProtocol.DOTP)
                DealHeaderReceived(context);
            else if (context.Protocol == DealProtocol.HTTP)
                HttpHeaderReceived(context);
        }

        /// <summary>
        /// The HeaderSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        public void HeaderSentCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            try
            {
                int sendcount = context.Listener.EndSend(result);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }

            if (!context.ReceiveMessage && !context.SendMessage)
            {
                //int _timeout = 0;
                //while (IsConnected(context.Id) && timeout < 10) _timeout++;
                context.Close = true;
            }

            context.HeaderSentNotice.Set();

            try
            {
                HeaderSent.Execute(context);
            }
            catch (Exception ex)
            {
                Echo(ex.Message);
                CloseClient(context.Id);
            }
        }

        /// <summary>
        /// The HttpHeaderReceived.
        /// </summary>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        public void HttpHeaderReceived(ITransferContext context)
        {
            if (context.BlockSize > 0)
            {
                context.Listener.BeginReceive(context.HeaderBuffer, 0, context.BufferSize, SocketFlags.None, HeaderReceivedCallback, context);
            }
            else
            {
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Header, DirectionType.Receive);
                request.Resolve(context);

                context.HeaderReceivedNotice.Set();

                try
                {
                    HeaderReceived.Execute(context);
                }
                catch (Exception ex)
                {
                    Echo(ex.Message);
                    CloseClient(context.Id);
                }
            }
        }

        /// <summary>
        /// The IsConnected.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsConnected(int id)
        {
            ITransferContext context = GetClient(id).Value;
            if (context != null && context.Listener != null && context.Listener.Connected)
                return !(context.Listener.Poll(timeout * 100, SelectMode.SelectRead) && context.Listener.Available == 0);
            else
                return false;
        }

        /// <summary>
        /// The MessageReceivedCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        public void MessageReceivedCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            MarkupType noiseKind = MarkupType.None;

            int receive = context.Listener.EndReceive(result);

            if (receive > 0)
                noiseKind = context.IncomingMessage(receive);

            if (context.BlockSize > 0)
            {
                int buffersize = (context.BlockSize < context.BufferSize) ? (int)context.BlockSize : context.BufferSize;
                context.Listener.BeginReceive(context.MessageBuffer, 0, buffersize, SocketFlags.None, MessageReceivedCallback, context);
            }
            else
            {
                object readPosition = context.DeserialBlockId;

                if (noiseKind == MarkupType.Block || (noiseKind == MarkupType.End && (int)readPosition < (context.Transfer.HeaderReceived.Context.ObjectsCount - 1)))
                    context.Listener.BeginReceive(context.MessageBuffer, 0, context.BufferSize, SocketFlags.None, MessageReceivedCallback, context);

                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Message, DirectionType.Receive);
                request.Resolve(context);

                if (context.ObjectsLeft <= 0 && !context.BatchesReceivedNotice.SafeWaitHandle.IsClosed)
                    context.BatchesReceivedNotice.Set();

                if (noiseKind == MarkupType.End && (int)readPosition >= (context.Transfer.HeaderReceived.Context.ObjectsCount - 1))
                {
                    context.BatchesReceivedNotice.WaitOne();
                    context.MessageReceivedNotice.Set();

                    try
                    {
                        MessageReceived.Execute(context);
                    }
                    catch (Exception ex)
                    {
                        Echo(ex.Message);
                        CloseClient(context.Id);
                    }
                }
            }
        }

        /// <summary>
        /// The MessageSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        public void MessageSentCallback(IAsyncResult result)
        {
            ITransferContext context = (ITransferContext)result.AsyncState;
            try
            {
                int sendcount = context.Listener.EndSend(result);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }

            if (context.SerialBlockId >= 0 || context.ObjectPosition < (context.Transfer.MyHeader.Context.ObjectsCount - 1))
            {
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Message, DirectionType.Send);
                request.Resolve();
                context.Listener.BeginSend(context.SerialBlock, 0, context.SerialBlock.Length, SocketFlags.None, MessageSentCallback, context);
            }
            else
            {
                if (context.ReceiveMessage)
                    context.MessageReceivedNotice.WaitOne();

                context.Close = true;

                context.MessageSentNotice.Set();

                try
                {
                    MessageSent.Execute(context);
                }
                catch (Exception ex)
                {
                    Echo(ex.Message);
                    CloseClient(context.Id);
                }
            }
        }

        /// <summary>
        /// The OnConnectCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        public void OnConnectCallback(IAsyncResult result)
        {
            try
            {
                if (!shutdown)
                {
                    ITransferContext context;
                    int id = -1;
                    id = DateTime.Now.Ticks.ToString().GetHashCode();
                    context = new TransferContext(((Socket)result.AsyncState).EndAccept(result), id);
                    context.Transfer = new DealTransfer(identity, null, context);
                    context.Resources = resources;
                    while (true)
                    {
                        if (!clients.Add(id, context))
                        {
                            id = DateTime.Now.Ticks.ToString().GetHashCode();
                            context.Id = id;
                        }
                        else
                            break;
                    }
                    Echo("Client connected. Get Id " + id);
                    context.Listener.BeginReceive(context.HeaderBuffer, 0, context.BufferSize, SocketFlags.None, HeaderReceivedCallback, clients[id]);
                }
                connectingNotice.Set();
            }
            catch (SocketException sx)
            {
                Echo(sx.Message);
            }
        }

        /// <summary>
        /// The Receive.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        /// <param name="id">The id<see cref="int"/>.</param>
        public void Receive(MessagePart messagePart, int id)
        {
            ITransferContext context = GetClient(id).Value;

            AsyncCallback callback = HeaderReceivedCallback;

            if (messagePart != MessagePart.Header && context.ReceiveMessage)
            {
                callback = MessageReceivedCallback;
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
        /// <param name="id">The id<see cref="int"/>.</param>
        public void Send(MessagePart messagePart, int id)
        {
            ITransferContext context = GetClient(id).Value;
            if (!IsConnected(context.Id))
                throw new Exception("Destination socket is not connected.");

            AsyncCallback callback = HeaderSentCallback;

            if (messagePart == MessagePart.Header)
            {
                callback = HeaderSentCallback;
                TransferOperation request = new TransferOperation(context.Transfer, MessagePart.Header, DirectionType.Send);
                request.Resolve();
            }
            else if (context.SendMessage)
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
        /// The StartListening.
        /// </summary>
        public void StartListening()
        {
            ushort port = Convert.ToUInt16(Identity.Port),
                  limit = Convert.ToUInt16(Identity.Limit);
            IPAddress address = IPAddress.Parse(Identity.Ip);
            IPEndPoint endpoint = new IPEndPoint(address, port);
            shutdown = false;
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(endpoint);
                    socket.Listen(limit);
                    while (!shutdown)
                    {

                        connectingNotice.Reset();
                        socket.BeginAccept(OnConnectCallback, socket);
                        connectingNotice.WaitOne();
                    }
                }
            }
            catch (SocketException sx)
            {
                Echo(sx.Message);
            }
        }

        /// <summary>
        /// The GetClient.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{ITransferContext}"/>.</returns>
        private ICard<ITransferContext> GetClient(int id)
        {
            return clients.GetCard(id);
        }

        #endregion
    }
}
