/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealConnection.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;
    using System.Instant;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="IDeal" />.
    /// </summary>
    public interface IDeal
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        object Content { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Close.
        /// </summary>
        void Close();

        /// <summary>
        /// The Initiate.
        /// </summary>
        /// <param name="isAsync">The isAsync<see cref="bool"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Initiate(bool isAsync = true);

        /// <summary>
        /// The Reconnect.
        /// </summary>
        void Reconnect();

        /// <summary>
        /// The SetCallback.
        /// </summary>
        /// <param name="OnCompleteEvent">The OnCompleteEvent<see cref="IDeputy"/>.</param>
        void SetCallback(IDeputy OnCompleteEvent);

        /// <summary>
        /// The SetCallback.
        /// </summary>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="classObject">The classObject<see cref="object"/>.</param>
        void SetCallback(string methodName, object classObject);

        #endregion
    }
    /// <summary>
    /// Defines the <see cref="DealConnection" />.
    /// </summary>
    public class DealConnection : IDeal
    {
        #region Fields

        private readonly ManualResetEvent completeNotice = new ManualResetEvent(false);
        public IDeputy CompleteEvent;
        public IDeputy EchoEvent;
        private IDeputy connected;
        private IDeputy headerReceived;
        private IDeputy headerSent;
        private bool isAsync = true;
        private IDeputy messageReceived;
        private IDeputy messageSent;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealConnection"/> class.
        /// </summary>
        /// <param name="ClientIdentity">The ClientIdentity<see cref="MemberIdentity"/>.</param>
        /// <param name="OnCompleteEvent">The OnCompleteEvent<see cref="IDeputy"/>.</param>
        /// <param name="OnEchoEvent">The OnEchoEvent<see cref="IDeputy"/>.</param>
        public DealConnection(MemberIdentity ClientIdentity, IDeputy OnCompleteEvent = null, IDeputy OnEchoEvent = null)
        {
            MemberIdentity ci = ClientIdentity;
            ci.Site = ServiceSite.Client;
            DealClient client = new DealClient(ci);
            Transfer = new DealTransfer(ci);

            connected = new DealEvent("Connected", this);
            headerSent = new DealEvent("HeaderSent", this);
            messageSent = new DealEvent("MessageSent", this);
            headerReceived = new DealEvent("HeaderReceived", this);
            messageReceived = new DealEvent("MessageReceived", this);

            client.Connected = connected;
            client.HeaderSent = headerSent;
            client.MessageSent = messageSent;
            client.HeaderReceived = headerReceived;
            client.MessageReceived = messageReceived;

            CompleteEvent = OnCompleteEvent;
            EchoEvent = OnEchoEvent;

            Client = client;

            WriteEcho("Client Connection Created");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public object Content
        {
            get { return Transfer.MyHeader.Content; }
            set { Transfer.MyHeader.Content = value; }
        }

        /// <summary>
        /// Gets or sets the Context.
        /// </summary>
        public ITransferContext Context { get; set; }

        /// <summary>
        /// Gets or sets the Transfer.
        /// </summary>
        public DealTransfer Transfer { get; set; }

        /// <summary>
        /// Gets or sets the Client.
        /// </summary>
        private DealClient Client { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Close.
        /// </summary>
        public void Close()
        {
            Client.Dispose();
        }

        /// <summary>
        /// The Connected.
        /// </summary>
        /// <param name="inetdealclient">The inetdealclient<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Connected(object inetdealclient)
        {
            WriteEcho("Client Connection Established");
            Transfer.MyHeader.Context.Echo = "Client say Hello. ";
            Context = Client.Context;
            Client.Context.Transfer = Transfer;

            IDealClient idc = (IDealClient)inetdealclient;

            idc.Send(MessagePart.Header);

            return idc.Context;
        }

        /// <summary>
        /// The HeaderReceived.
        /// </summary>
        /// <param name="inetdealclient">The inetdealclient<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object HeaderReceived(object inetdealclient)
        {
            string serverEcho = Transfer.HeaderReceived.Context.Echo;
            WriteEcho(string.Format("Server header received"));
            if (serverEcho != null && serverEcho != "")
                WriteEcho(string.Format("Server echo: {0}", serverEcho));

            IDealClient idc = (IDealClient)inetdealclient;

            if (idc.Context.Close)
                idc.Dispose();
            else
            {
                if (!idc.Context.Synchronic)
                {
                    if (idc.Context.SendMessage)
                        idc.Send(MessagePart.Message);
                }
                if (idc.Context.ReceiveMessage)
                    idc.Receive(MessagePart.Message);
            }

            if (!idc.Context.ReceiveMessage &&
                !idc.Context.SendMessage)
            {
                if (CompleteEvent != null)
                    CompleteEvent.Execute(idc.Context);
                if (!isAsync)
                    completeNotice.Set();
            }
            return idc.Context;
        }

        /// <summary>
        /// The HeaderSent.
        /// </summary>
        /// <param name="inetdealclient">The inetdealclient<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object HeaderSent(object inetdealclient)
        {
            WriteEcho("Client header sent");
            IDealClient idc = (IDealClient)inetdealclient;
            if (!idc.Context.Synchronic)
                idc.Receive(MessagePart.Header);
            else
                idc.Send(MessagePart.Message);

            return idc.Context;
        }

        /// <summary>
        /// The Initiate.
        /// </summary>
        /// <param name="IsAsync">The IsAsync<see cref="bool"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Initiate(bool IsAsync = true)
        {
            isAsync = IsAsync;
            Client.Connect();
            if (!isAsync)
            {
                completeNotice.WaitOne();
                return Context;
            }
            return null;
        }

        /// <summary>
        /// The MessageReceived.
        /// </summary>
        /// <param name="inetdealclient">The inetdealclient<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object MessageReceived(object inetdealclient)
        {
            WriteEcho(string.Format("Server message received"));

            ITransferContext context = ((IDealClient)inetdealclient).Context;
            if (context.Close)
                ((IDealClient)inetdealclient).Dispose();

            if (CompleteEvent != null)
                CompleteEvent.Execute(context);
            if (!isAsync)
                completeNotice.Set();
            return context;
        }

        /// <summary>
        /// The MessageSent.
        /// </summary>
        /// <param name="inetdealclient">The inetdealclient<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object MessageSent(object inetdealclient)
        {
            WriteEcho("Client message sent");

            IDealClient idc = (IDealClient)inetdealclient;
            if (idc.Context.Synchronic)
                idc.Receive(MessagePart.Header);

            if (!idc.Context.ReceiveMessage)
            {
                if (CompleteEvent != null)
                    CompleteEvent.Execute(idc.Context);
                if (!isAsync)
                    completeNotice.Set();
            }
            return idc.Context;
        }

        /// <summary>
        /// The Reconnect.
        /// </summary>
        public void Reconnect()
        {
            MemberIdentity ci = new MemberIdentity()
            {
                AuthId = Client.Identity.AuthId,
                Site = ServiceSite.Client,
                Name = Client.Identity.Name,
                Token = Client.Identity.Token,
                UserId = Client.Identity.UserId,
                DeptId = Client.Identity.DeptId,
                DataPlace = Client.Identity.DataPlace,
                Id = Client.Identity.Id,
                Ip = Client.EndPoint.Address.ToString(),
                Port = Client.EndPoint.Port,
                Key = Client.Identity.Key
            };
            Transfer.Dispose();
            DealClient client = new DealClient(ci);
            Transfer = new DealTransfer(ci);
            client.Connected = connected;
            client.HeaderSent = headerSent;
            client.MessageSent = messageSent;
            client.HeaderReceived = headerReceived;
            client.MessageReceived = messageReceived;
            Client = client;
        }

        /// <summary>
        /// The SetCallback.
        /// </summary>
        /// <param name="OnCompleteEvent">The OnCompleteEvent<see cref="IDeputy"/>.</param>
        public void SetCallback(IDeputy OnCompleteEvent)
        {
            CompleteEvent = OnCompleteEvent;
        }

        /// <summary>
        /// The SetCallback.
        /// </summary>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="classObject">The classObject<see cref="object"/>.</param>
        public void SetCallback(string methodName, object classObject)
        {
            CompleteEvent = new DealEvent(methodName, classObject);
        }

        /// <summary>
        /// The WriteEcho.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        private void WriteEcho(string message)
        {
            if (EchoEvent != null)
                EchoEvent.Execute(message);
        }

        #endregion
    }
}
