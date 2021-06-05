/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.ITransferContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Collections;
    using System.IO;
    using System.Net.Sockets;
    using System.Sets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="ITransferContext" />.
    /// </summary>
    public interface ITransferContext : ISerialBuffer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the BatchesReceivedNotice.
        /// </summary>
        ManualResetEvent BatchesReceivedNotice { get; set; }

        /// <summary>
        /// Gets the BufferSize.
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Close.
        /// </summary>
        bool Close { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Denied.
        /// </summary>
        bool Denied { get; set; }

        /// <summary>
        /// Gets the Echo.
        /// </summary>
        string Echo { get; }

        /// <summary>
        /// Gets the HeaderBuffer.
        /// </summary>
        byte[] HeaderBuffer { get; }

        /// <summary>
        /// Gets or sets the HeaderReceivedNotice.
        /// </summary>
        ManualResetEvent HeaderReceivedNotice { get; set; }

        /// <summary>
        /// Gets or sets the HeaderSentNotice.
        /// </summary>
        ManualResetEvent HeaderSentNotice { get; set; }

        /// <summary>
        /// Gets or sets the HttpHeaders.
        /// </summary>
        Hashtable HttpHeaders { get; set; }

        /// <summary>
        /// Gets or sets the HttpOptions.
        /// </summary>
        Hashtable HttpOptions { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the Listener.
        /// </summary>
        Socket Listener { get; set; }

        /// <summary>
        /// Gets the MessageBuffer.
        /// </summary>
        byte[] MessageBuffer { get; }

        /// <summary>
        /// Gets or sets the MessageReceivedNotice.
        /// </summary>
        ManualResetEvent MessageReceivedNotice { get; set; }

        /// <summary>
        /// Gets or sets the MessageSentNotice.
        /// </summary>
        ManualResetEvent MessageSentNotice { get; set; }

        /// <summary>
        /// Gets or sets the Method.
        /// </summary>
        ProtocolMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the ObjectPosition.
        /// </summary>
        int ObjectPosition { get; set; }

        /// <summary>
        /// Gets or sets the ObjectsLeft.
        /// </summary>
        int ObjectsLeft { get; set; }

        /// <summary>
        /// Gets or sets the Protocol.
        /// </summary>
        DealProtocol Protocol { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ReceiveMessage.
        /// </summary>
        bool ReceiveMessage { get; set; }

        /// <summary>
        /// Gets or sets the RequestBuilder.
        /// </summary>
        StringBuilder RequestBuilder { get; set; }

        /// <summary>
        /// Gets or sets the Resources.
        /// </summary>
        IDeck<byte[]> Resources { get; set; }

        /// <summary>
        /// Gets or sets the ResponseBuilder.
        /// </summary>
        StringBuilder ResponseBuilder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SendMessage.
        /// </summary>
        bool SendMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Synchronic.
        /// </summary>
        bool Synchronic { get; set; }

        /// <summary>
        /// Gets or sets the Transfer.
        /// </summary>
        DealTransfer Transfer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Append.
        /// </summary>
        /// <param name="text">The text<see cref="string"/>.</param>
        void Append(string text);

        /// <summary>
        /// The Dispose.
        /// </summary>
        void Dispose();

        /// <summary>
        /// The HandleDeniedRequest.
        /// </summary>
        void HandleDeniedRequest();

        /// <summary>
        /// The HandleGetRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        void HandleGetRequest(string content_type = "text/html");

        /// <summary>
        /// The HandleOptionsRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        void HandleOptionsRequest(string content_type = "text/html");

        /// <summary>
        /// The HandlePostRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        void HandlePostRequest(string content_type = "text/html");

        /// <summary>
        /// The IdentifyProtocol.
        /// </summary>
        /// <returns>The <see cref="DealProtocol"/>.</returns>
        DealProtocol IdentifyProtocol();

        /// <summary>
        /// The IncomingHeader.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        MarkupType IncomingHeader(int received);

        /// <summary>
        /// The IncomingMessage.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        MarkupType IncomingMessage(int received);

        /// <summary>
        /// The Reset.
        /// </summary>
        void Reset();

        #endregion
    }
}
