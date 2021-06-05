/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealServer.cs
   
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
    /// Defines the <see cref="DealServer" />.
    /// </summary>
    public class DealServer
    {
        #region Fields

        public static IMemberSecurity Security;
        private IDealListener server;

        #endregion

        #region Methods

        /// <summary>
        /// The ClearClients.
        /// </summary>
        public void ClearClients()
        {
            WriteEcho("Client registry cleaned");
            if (server != null)
                server.ClearClients();
        }

        /// <summary>
        /// The ClearResources.
        /// </summary>
        public void ClearResources()
        {
            WriteEcho("Resource buffer cleaned");
            if (server != null)
                server.ClearResources();
        }

        /// <summary>
        /// The Close.
        /// </summary>
        public void Close()
        {
            if (server != null)
            {
                WriteEcho("Server instance shutdown ");
                server.CloseListener();
                server = null;
            }
            else
            {
                WriteEcho("Server instance doesn't exist ");
            }
        }

        /// <summary>
        /// The HeaderReceived.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object HeaderReceived(object inetdealcontext)
        {
            string clientEcho = ((ITransferContext)inetdealcontext).Transfer.HeaderReceived.Context.Echo;
            WriteEcho(string.Format("Client header received"));
            if (clientEcho != null && clientEcho != "")
                WriteEcho(string.Format("Client echo: {0}", clientEcho));

            DealContext trctx = ((ITransferContext)inetdealcontext).Transfer.MyHeader.Context;
            if (trctx.Echo == null || trctx.Echo == "")
                trctx.Echo = "Server say Hello";
            if (!((ITransferContext)inetdealcontext).Synchronic)
                server.Send(MessagePart.Header, ((ITransferContext)inetdealcontext).Id);
            else
                server.Receive(MessagePart.Message, ((ITransferContext)inetdealcontext).Id);

            return ((ITransferContext)inetdealcontext);
        }

        /// <summary>
        /// The HeaderSent.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object HeaderSent(object inetdealcontext)
        {

            WriteEcho("Server header sent");

            ITransferContext context = (ITransferContext)inetdealcontext;
            if (context.Close)
            {
                context.Transfer.Dispose();
                server.CloseClient(context.Id);
            }
            else
            {
                if (!context.Synchronic)
                {
                    if (context.ReceiveMessage)
                        server.Receive(MessagePart.Message, context.Id);
                }
                if (context.SendMessage)
                    server.Send(MessagePart.Message, context.Id);
            }
            return context;
        }

        /// <summary>
        /// The IsActive.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsActive()
        {

            if (server != null)
            {
                WriteEcho("Server Instance Is Active");
                return true;
            }
            else
            {
                WriteEcho("Server Instance Doesn't Exist");
                return false;
            }
        }

        /// <summary>
        /// The MessageReceived.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object MessageReceived(object inetdealcontext)
        {
            WriteEcho(string.Format("Client message received"));
            if (((ITransferContext)inetdealcontext).Synchronic)
                server.Send(MessagePart.Header, ((ITransferContext)inetdealcontext).Id);
            return (ITransferContext)inetdealcontext;
        }

        /// <summary>
        /// The MessageSent.
        /// </summary>
        /// <param name="inetdealcontext">The inetdealcontext<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object MessageSent(object inetdealcontext)
        {
            WriteEcho("Server message sent");
            ITransferContext result = (ITransferContext)inetdealcontext;
            if (result.Close)
            {
                result.Transfer.Dispose();
                server.CloseClient(result.Id);
            }
            return result;
        }

        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="ServerIdentity">The ServerIdentity<see cref="MemberIdentity"/>.</param>
        /// <param name="security">The security<see cref="IMemberSecurity"/>.</param>
        /// <param name="OnEchoEvent">The OnEchoEvent<see cref="IDeputy"/>.</param>
        public void Start(MemberIdentity ServerIdentity, IMemberSecurity security = null, IDeputy OnEchoEvent = null)
        {
            server = DealListener.Instance;
            server.Identity = ServerIdentity;
            Security = security;

            new Thread(new ThreadStart(server.StartListening)).Start();

            server.HeaderSent = new DealEvent("HeaderSent", this);
            server.MessageSent = new DealEvent("MessageSent", this);
            server.HeaderReceived = new DealEvent("HeaderReceived", this);
            server.MessageReceived = new DealEvent("MessageReceived", this);
            server.SendEcho = OnEchoEvent;

            WriteEcho("Dealer instance started");
        }

        /// <summary>
        /// The WriteEcho.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public void WriteEcho(string message)
        {
            if (server != null)
                server.Echo(message);
        }

        #endregion
    }
}
