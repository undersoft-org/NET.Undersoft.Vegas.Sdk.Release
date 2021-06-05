/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.IDealClient.cs
   
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
    /// Defines the <see cref="IDealClient" />.
    /// </summary>
    public interface IDealClient : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Connected.
        /// </summary>
        IDeputy Connected { get; set; }

        /// <summary>
        /// Gets or sets the Context.
        /// </summary>
        ITransferContext Context { get; set; }

        /// <summary>
        /// Gets or sets the HeaderReceived.
        /// </summary>
        IDeputy HeaderReceived { get; set; }

        /// <summary>
        /// Gets or sets the HeaderSent.
        /// </summary>
        IDeputy HeaderSent { get; set; }

        /// <summary>
        /// Gets or sets the MessageReceived.
        /// </summary>
        IDeputy MessageReceived { get; set; }

        /// <summary>
        /// Gets or sets the MessageSent.
        /// </summary>
        IDeputy MessageSent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Connect.
        /// </summary>
        void Connect();

        /// <summary>
        /// The IsConnected.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsConnected();

        /// <summary>
        /// The Receive.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        void Receive(MessagePart messagePart);

        /// <summary>
        /// The Send.
        /// </summary>
        /// <param name="messagePart">The messagePart<see cref="MessagePart"/>.</param>
        void Send(MessagePart messagePart);

        #endregion
    }
}
