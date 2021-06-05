/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.IDealListener.cs
   
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

    /// <summary>
    /// Defines the <see cref="IDealListener" />.
    /// </summary>
    public interface IDealListener : IDisposable
    {
        #region Properties

        //  IMemberSecurity Security { get; set; }
        /// <summary>
        /// Gets or sets the HeaderReceived.
        /// </summary>
        IDeputy HeaderReceived { get; set; }

        /// <summary>
        /// Gets or sets the HeaderSent.
        /// </summary>
        IDeputy HeaderSent { get; set; }

        /// <summary>
        /// Gets or sets the Identity.
        /// </summary>
        MemberIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the MessageReceived.
        /// </summary>
        IDeputy MessageReceived { get; set; }

        /// <summary>
        /// Gets or sets the MessageSent.
        /// </summary>
        IDeputy MessageSent { get; set; }

        /// <summary>
        /// Gets or sets the SendEcho.
        /// </summary>
        IDeputy SendEcho { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The ClearClients.
        /// </summary>
        void ClearClients();

        /// <summary>
        /// The ClearResources.
        /// </summary>
        void ClearResources();

        /// <summary>
        /// The CloseClient.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        void CloseClient(int id);

        /// <summary>
        /// The CloseListener.
        /// </summary>
        void CloseListener();

        /// <summary>
        /// The Echo.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        void Echo(string message);

        /// <summary>
        /// The HeaderReceivedCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        void HeaderReceivedCallback(IAsyncResult result);

        /// <summary>
        /// The HeaderSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        void HeaderSentCallback(IAsyncResult result);

        /// <summary>
        /// The IsConnected.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsConnected(int id);

        /// <summary>
        /// The MessageReceivedCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        void MessageReceivedCallback(IAsyncResult result);

        /// <summary>
        /// The MessageSentCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        void MessageSentCallback(IAsyncResult result);

        /// <summary>
        /// The OnConnectCallback.
        /// </summary>
        /// <param name="result">The result<see cref="IAsyncResult"/>.</param>
        void OnConnectCallback(IAsyncResult result);

        /// <summary>
        /// The Receive.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        /// <param name="id">The id<see cref="int"/>.</param>
        void Receive(MessagePart messagePart, int id);

        /// <summary>
        /// The Send.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        /// <param name="id">The id<see cref="int"/>.</param>
        void Send(MessagePart messagePart, int id);

        /// <summary>
        /// The StartListening.
        /// </summary>
        void StartListening();

        #endregion
    }
}
