/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.TransferContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Extract;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Sets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="TransferContext" />.
    /// </summary>
    public sealed class TransferContext : ITransferContext, IDisposable
    {
        #region Constants

        private const int Buffer_Size = 4096;

        #endregion

        #region Fields

        [NonSerialized] private readonly DealStream instream;
        [NonSerialized] private readonly DealStream outstream;
        [NonSerialized] public byte[] binReceive = new byte[0];
        [NonSerialized] public IntPtr binReceiveAddress;
        [NonSerialized] public IntPtr binReceiveHandler;
        [NonSerialized] public byte[] binSend = new byte[0];
        [NonSerialized] public IntPtr binSendAddress;
        [NonSerialized] public IntPtr binSendHandler;
        [NonSerialized] public IntPtr headerBufferAddress;
        [NonSerialized] public IntPtr headerBufferHandler;
        [NonSerialized] public Hashtable httpHeaders = new Hashtable();
        [NonSerialized] public Hashtable httpOptions = new Hashtable();
        [NonSerialized] public IntPtr messageBufferAddress;
        [NonSerialized] public IntPtr messageBufferHandler;
        [NonSerialized] public MemoryStream msReceive;
        [NonSerialized] public MemoryStream msSend;
        [NonSerialized] public StringBuilder requestBuilder = new StringBuilder();
        [NonSerialized] public StringBuilder responseBuilder = new StringBuilder();
        [NonSerialized] private Socket _listener;
        private int Block_Offset = 16;
        [NonSerialized] private bool disposed = false;
        [NonSerialized] private byte[] headerbuffer = new byte[Buffer_Size];
        [NonSerialized] private String http_method;
        [NonSerialized] private String http_protocol_version;
        [NonSerialized] private String http_url;
        [NonSerialized]
        private Dictionary<string, string> httpExtensions = new Dictionary<string, string>()
        {
            { ".html", "text/html" },
            { ".css",  "text/css"  },
            { ".less", "text/css"  },
            { ".png",  "image/png" },
            { ".ico",  "image/ico" },
            { ".jpg",  "image/jpg" },
            { ".bmp",  "image/bmp" },
            { ".gif",  "image/gif" },
            { ".js",   "text/javascript" },
            { ".qjs",  "text/javascript" },
            { ".bjs",  "text/babel" },
            { ".json", "application/json" },
            { ".woff", "font/woff" },
            { ".woff2","font/woff2" },
            { ".ttf",  "font/ttf" },
            { ".svg",  "image/svg" }
        };
        private int id;
        [NonSerialized] private byte[] messagebuffer = new byte[Buffer_Size];
        [NonSerialized] private IDeck<byte[]> resources;
        [NonSerialized] private StringBuilder sb = new StringBuilder();
        [NonSerialized] private DealTransfer tr;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferContext"/> class.
        /// </summary>
        /// <param name="listener">The listener<see cref="Socket"/>.</param>
        /// <param name="_id">The _id<see cref="int"/>.</param>
        /// <param name="withStream">The withStream<see cref="bool"/>.</param>
        public TransferContext(Socket listener, int _id = -1, bool withStream = false)
        {
            this._listener = listener;
            if (withStream)
            {
                this.instream = new DealStream(listener);
                this.outstream = new DealStream(listener);
            }

            GCHandle gc = GCHandle.Alloc(messagebuffer, GCHandleType.Pinned);
            messageBufferHandler = GCHandle.ToIntPtr(gc);
            messageBufferAddress = gc.AddrOfPinnedObject();
            gc = GCHandle.Alloc(headerbuffer, GCHandleType.Pinned);
            headerBufferHandler = GCHandle.ToIntPtr(gc);
            headerBufferAddress = gc.AddrOfPinnedObject();

            this.id = _id;
            this.Close = false;
            this.Denied = false;
            this.ObjectPosition = 0;
            this.ObjectsLeft = 0;
            this.DeserialBlockId = 0;
            this.BlockSize = 0;
            this.SendMessage = true;
            this.ReceiveMessage = true;
            this.disposed = true;

            HeaderSentNotice.Reset();
            HeaderReceivedNotice.Reset();
            MessageSentNotice.Reset();
            MessageReceivedNotice.Reset();
            BatchesReceivedNotice.Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BatchesReceivedNotice.
        /// </summary>
        public ManualResetEvent BatchesReceivedNotice { get; set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets the BlockOffset.
        /// </summary>
        public int BlockOffset
        {
            get
            {
                return Block_Offset;
            }
            set
            {
                Block_Offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the BlockSize.
        /// </summary>
        public long BlockSize { get; set; }

        /// <summary>
        /// Gets the BufferSize.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return Buffer_Size;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Close.
        /// </summary>
        public bool Close { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Denied.
        /// </summary>
        public bool Denied { get; set; }

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
                    disposed = false;
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
        public int DeserialBlockId { get; set; }

        /// <summary>
        /// Gets the DeserialBlockPtr.
        /// </summary>
        public IntPtr DeserialBlockPtr => binReceiveAddress;

        /// <summary>
        /// Gets the Echo.
        /// </summary>
        public string Echo
        {
            get
            {
                return this.sb.ToString();
            }
        }

        /// <summary>
        /// Gets the HeaderBuffer.
        /// </summary>
        public byte[] HeaderBuffer
        {
            get
            {
                return this.headerbuffer;
            }
        }

        /// <summary>
        /// Gets or sets the HeaderReceivedNotice.
        /// </summary>
        public ManualResetEvent HeaderReceivedNotice { get; set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets the HeaderSentNotice.
        /// </summary>
        public ManualResetEvent HeaderSentNotice { get; set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets the HttpHeaders.
        /// </summary>
        public Hashtable HttpHeaders
        {
            get { return httpHeaders; }
            set { httpHeaders = value; }
        }

        /// <summary>
        /// Gets or sets the HttpOptions.
        /// </summary>
        public Hashtable HttpOptions
        {
            get { return httpOptions; }
            set { httpOptions = value; }
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets the Listener.
        /// </summary>
        public Socket Listener
        {
            get
            {
                return this._listener;
            }
            set
            {
                this._listener = value;
            }
        }

        /// <summary>
        /// Gets the MessageBuffer.
        /// </summary>
        public byte[] MessageBuffer
        {
            get
            {
                return this.messagebuffer;
            }
        }

        /// <summary>
        /// Gets or sets the MessageReceivedNotice.
        /// </summary>
        public ManualResetEvent MessageReceivedNotice { get; set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets the MessageSentNotice.
        /// </summary>
        public ManualResetEvent MessageSentNotice { get; set; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets the Method.
        /// </summary>
        public ProtocolMethod Method { get; set; } = ProtocolMethod.NONE;

        /// <summary>
        /// Gets or sets the ObjectPosition.
        /// </summary>
        public int ObjectPosition { get; set; }

        /// <summary>
        /// Gets or sets the ObjectsLeft.
        /// </summary>
        public int ObjectsLeft { get; set; }

        /// <summary>
        /// Gets or sets the Protocol.
        /// </summary>
        public DealProtocol Protocol { get; set; } = DealProtocol.NONE;

        /// <summary>
        /// Gets or sets a value indicating whether ReceiveMessage.
        /// </summary>
        public bool ReceiveMessage { get; set; }

        /// <summary>
        /// Gets or sets the RequestBuilder.
        /// </summary>
        public StringBuilder RequestBuilder
        {
            get { return requestBuilder; }
            set { requestBuilder = value; }
        }

        /// <summary>
        /// Gets or sets the Resources.
        /// </summary>
        public IDeck<byte[]> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        /// <summary>
        /// Gets or sets the ResponseBuilder.
        /// </summary>
        public StringBuilder ResponseBuilder
        {
            get { return responseBuilder; }
            set { responseBuilder = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SendMessage.
        /// </summary>
        public bool SendMessage { get; set; }

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
                if (value != null)
                {
                    lock (binSend)
                    {
                        disposed = false;
                        binSend = value;
                        if (Protocol != DealProtocol.HTTP)
                        {
                            long size = binSend.Length - BlockOffset;
                            new byte[] { (byte) 'D',
                                     (byte) 'E',
                                     (byte) 'A',
                                     (byte) 'L' }.CopyTo(binSend, 0);
                            BitConverter.GetBytes(size).CopyTo(binSend, 4);
                            BitConverter.GetBytes(ObjectPosition).CopyTo(binSend, 12);
                        }
                        value = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the SerialBlockId.
        /// </summary>
        public int SerialBlockId { get; set; }

        /// <summary>
        /// Gets the SerialBlockPtr.
        /// </summary>
        public IntPtr SerialBlockPtr => binSendAddress;

        /// <summary>
        /// Gets the Site.
        /// </summary>
        public ServiceSite Site
        {
            get
            {
                return Transfer.MyHeader.Context.IdentitySite;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Synchronic.
        /// </summary>
        public bool Synchronic { get; set; }

        /// <summary>
        /// Gets or sets the Transfer.
        /// </summary>
        public DealTransfer Transfer
        {
            get
            {
                return this.tr;
            }
            set
            {
                if (value.Context == null)
                {
                    value.Context = this;
                    if (value.Identity != null)
                        value.MyHeader.BindContext(value.Context);
                }
                if (value.MyMessage.Content != null)
                {
                    if (value.MyMessage.Content.GetType() == typeof(object[][]))
                        value.MyHeader.Context.ObjectsCount = ((object[][])value.MyMessage.Content).Length;
                }
                this.tr = value;
            }
        }

        /// <summary>
        /// Gets or sets the SendEcho.
        /// </summary>
        internal DealEvent SendEcho { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Append.
        /// </summary>
        /// <param name="text">The text<see cref="string"/>.</param>
        public void Append(string text)
        {
            this.sb.Append(text);
        }

        /// <summary>
        /// The DirSearch.
        /// </summary>
        /// <param name="dir">The dir<see cref="string"/>.</param>
        /// <param name="jspfiles">The jspfiles<see cref="List{string}"/>.</param>
        public void DirSearch(string dir, List<string> jspfiles)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                    jspfiles.Add(f);
                foreach (string d in Directory.GetDirectories(dir))
                    DirSearch(d, jspfiles);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                sb.Clear();
                msSend.Dispose();
                msReceive.Dispose();
                GCHandle gc;
                lock (binReceive)
                {
                    if (!binReceiveHandler.Equals(IntPtr.Zero))
                    {
                        gc = GCHandle.FromIntPtr(binReceiveHandler);
                        gc.Free();
                    }
                    binReceive = null;
                }
                lock (binSend)
                {
                    if (!binSendHandler.Equals(IntPtr.Zero))
                    {
                        gc = GCHandle.FromIntPtr(binSendHandler);
                        gc.Free();
                    }
                    binSend = null;
                }

                gc = GCHandle.FromIntPtr(messageBufferHandler);
                gc.Free();
                messagebuffer = null;
                gc = GCHandle.FromIntPtr(headerBufferHandler);
                gc.Free();
                headerbuffer = null;

                HeaderSentNotice.Dispose();
                HeaderReceivedNotice.Dispose();
                MessageSentNotice.Dispose();
                MessageReceivedNotice.Dispose();
                BatchesReceivedNotice.Dispose();

                disposed = true;
            }
        }

        /// <summary>
        /// The GetHttpExtensionType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetHttpExtensionType()
        {
            string extension = http_url.Substring(http_url.LastIndexOf('.'));
            string result = null;
            httpExtensions.TryGetValue(extension, out result);
            return result = (result == null) ? "text/html" : result;
        }

        /// <summary>
        /// The GetJavaScriptProject.
        /// </summary>
        /// <param name="ms">The ms<see cref="MemoryStream"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool GetJavaScriptProject(MemoryStream ms)
        {
            string extension = http_url.Substring(http_url.LastIndexOf('.'));
            if (extension.Equals(".qjs") || extension.Equals(".bjs"))
            {
                string jspdir = "../../Web" + http_url.Substring(0, http_url.LastIndexOf('.'));
                List<string> jspfiles = new List<string>();
                DirSearch(jspdir, jspfiles);
                foreach (string jspfile in jspfiles)
                {
                    Stream fs = File.Open(jspfile, FileMode.Open);
                    fs.CopyTo(ms);
                    //ms.Write(("\n").ToBytes(CharEncoding.UTF8), 0, 1);
                    fs.Close();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// The HandleDeniedRequest.
        /// </summary>
        public void HandleDeniedRequest()
        {
            writeDenied();
            writeClose();
            binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
        }

        /// <summary>
        /// The HandleGetRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        public void HandleGetRequest(string content_type = "text/html")
        {
            if (http_url.Equals("/") ||
                http_url.Equals(""))
                http_url = "/Index.html";

            if (!Resources.TryGet(http_url, out binSend))
            {
                MemoryStream ms = new MemoryStream();
                if (GetJavaScriptProject(ms))
                {
                    content_type = GetHttpExtensionType();
                    writeSuccess(content_type);
                }
                else
                {
                    if (File.Exists("../../Web" + http_url))
                    {
                        content_type = GetHttpExtensionType();
                        Stream fs = File.Open("../../Web" + http_url, FileMode.Open);
                        fs.CopyTo(ms);
                        fs.Close();
                        writeSuccess(content_type);
                    }
                    else
                        writeFailure();
                }

                if (HttpHeaders.ContainsKey("Connection"))
                    writeClose(HttpHeaders["Connection"].ToString());
                else
                    writeClose();

                if (content_type.Contains("text") ||
                    content_type.Contains("json"))
                {
                    ResponseBuilder.Append((ms.ToArray().ToChars(CharEncoding.UTF8)));
                    binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
                    Resources.Add(http_url, binSend);
                }
                else
                {
                    binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8).Concat(ms.ToArray()).ToArray();
                    Resources.Add(http_url, binSend);
                }
                ms.Dispose();
            }
        }

        /// <summary>
        /// The HandleOptionsRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        public void HandleOptionsRequest(string content_type = "text/html")
        {
            writeSuccess(content_type);
            writeOptions();
            writeClose();
            binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
        }

        /// <summary>
        /// The HandlePostRequest.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        public void HandlePostRequest(string content_type = "text/html")
        {
            writeSuccess(content_type);
            writeOptions();
            writeClose();
            string requestBuilder = RequestBuilder.ToString();//.Trim(new char[] { '\n', '\r' });
            ResponseBuilder.AppendLine(requestBuilder);
            string responseString = ResponseBuilder.ToString();//.TrimEnd(new char[] { '\n', '\r' });           
            binSend = responseString.ToBytes(CharEncoding.UTF8);
        }

        /// <summary>
        /// The HttpHeader.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public MarkupType HttpHeader(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                if (BlockSize == 0)
                {
                    BlockSize = 1;
                    msReceive = new MemoryStream();
                }

                msReceive.Write(HeaderBuffer, 0, received);

                if (received < BufferSize)
                {
                    BlockSize = 0;
                    msReceive.Position = 0;
                    ParseRequest(msReceive);
                    ReadHeaders(msReceive);
                    if (VerifyRequest())
                        binReceive = msReceive.ToArray().Skip(Convert.ToInt32(msReceive.Position)).ToArray();
                    else
                        Denied = true;
                }
            }
            return noiseKind;
        }

        /// <summary>
        /// The IdentifyProtocol.
        /// </summary>
        /// <returns>The <see cref="DealProtocol"/>.</returns>
        public DealProtocol IdentifyProtocol()
        {
            StringBuilder sb = new StringBuilder();
            Protocol = DealProtocol.NONE;
            ProtocolMethod method = ProtocolMethod.NONE;
            for (int i = 0; i < HeaderBuffer.Length; i++)
            {
                MarkupType splitter = MarkupType.None;
                HeaderBuffer[i].IsSpliter(out splitter);
                if ((splitter != MarkupType.Empty) &&
                    (splitter != MarkupType.Space) &&
                    (splitter != MarkupType.Line))
                    sb.Append(HeaderBuffer[i].ToChars(CharEncoding.UTF8));
                if (sb.Length > 3)
                {
                    method = ProtocolMethod.NONE;
                    Enum.TryParse(sb.ToString().ToUpper(), out method);
                    if (method != ProtocolMethod.NONE)
                    {
                        switch (method)
                        {
                            case ProtocolMethod.DEAL:
                                Protocol = DealProtocol.DOTP;
                                break;
                            case ProtocolMethod.SYNC:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.GET:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.POST:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.OPTIONS:
                                Protocol = DealProtocol.HTTP;
                                break;
                            default:
                                Protocol = DealProtocol.NONE;
                                break;
                        }
                    }
                    Method = method;
                    if (Protocol != DealProtocol.NONE)
                    {
                        sb = null;
                        return Protocol;
                    }
                }
            }
            sb = null;
            return Protocol;
        }

        /// <summary>
        /// The IncomingHeader.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public MarkupType IncomingHeader(int received)
        {
            disposed = false;
            MarkupType noiseKind = MarkupType.None;
            if (Protocol == DealProtocol.NONE)
                IdentifyProtocol();
            if (Protocol == DealProtocol.DOTP)
                return SyncHeader(received);
            else if (Protocol == DealProtocol.HTTP)
                return HttpHeader(received);
            return noiseKind;
        }

        /// <summary>
        /// The IncomingMessage.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public unsafe MarkupType IncomingMessage(int received)
        {
            disposed = false;
            MarkupType noiseKind = MarkupType.None;
            if (Protocol == DealProtocol.DOTP)
                return SyncMessage(received);
            return noiseKind;
        }

        /// <summary>
        /// The ParseRequest.
        /// </summary>
        /// <param name="ms">The ms<see cref="Stream"/>.</param>
        public void ParseRequest(Stream ms)
        {
            String request = streamReadLine(ms);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_version = tokens[2];
        }

        /// <summary>
        /// The ReadHeaders.
        /// </summary>
        /// <param name="ms">The ms<see cref="Stream"/>.</param>
        public void ReadHeaders(Stream ms)
        {
            String line;
            while ((line = streamReadLine(ms)) != null)
            {
                if (line.Equals(""))
                    return;

                int separator = line.IndexOf(':');
                if (separator == -1)
                    throw new Exception("invalid http header line: " + line);

                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                    pos++; // strip any spaces                

                string value = line.Substring(pos, line.Length - pos);
                HttpHeaders[name] = value;
            }
        }

        /// <summary>
        /// The Reset.
        /// </summary>
        public void Reset()
        {
            if (!disposed)
            {
                sb.Clear();
                sb = new StringBuilder();
                msSend.Dispose();
                msSend = new MemoryStream();
                msReceive.Dispose();
                msReceive = new MemoryStream();

                lock (binReceive)
                {
                    if (!binReceiveHandler.Equals(IntPtr.Zero))
                    {
                        GCHandle gc = GCHandle.FromIntPtr(binReceiveHandler);
                        gc.Free();
                        binReceive = new byte[0];
                    }
                }
                lock (binSend)
                    binSend = new byte[0];
            }
        }

        /// <summary>
        /// The SyncHeader.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public unsafe MarkupType SyncHeader(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                int offset = 0, length = received;
                bool inprogress = false;
                if (BlockSize == 0)
                {
                    BlockSize = *((int*)(headerBufferAddress + 4).ToPointer());
                    DeserialBlockId = *((int*)(headerBufferAddress + 12).ToPointer());

                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceiveHandler = GCHandle.ToIntPtr(gc);
                    binReceiveAddress = gc.AddrOfPinnedObject();

                    offset = BlockOffset;
                    length -= BlockOffset;
                }

                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = length;
                    noiseKind = HeaderBuffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }

                int destid = (int)(binReceive.Length - (BlockSize + length));

                if (inprogress)
                {
                    Extractor.CopyBlock(binReceiveAddress, destid, headerBufferAddress, offset, length);
                }
            }
            return noiseKind;
        }

        /// <summary>
        /// The SyncMessage.
        /// </summary>
        /// <param name="received">The received<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public unsafe MarkupType SyncMessage(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                int offset = 0, length = received;
                bool inprogress = false;

                if (BlockSize == 0)
                {
                    BlockSize = *((int*)(messageBufferAddress + 4).ToPointer());
                    DeserialBlockId = *((int*)(messageBufferAddress + 12).ToPointer());

                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceiveHandler = GCHandle.ToIntPtr(gc);
                    binReceiveAddress = gc.AddrOfPinnedObject();

                    offset = BlockOffset;
                    length -= BlockOffset;
                }

                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = length;
                    noiseKind = MessageBuffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }

                int destid = (int)(binReceive.Length - (BlockSize + length));
                if (inprogress)
                {
                    Extractor.CopyBlock(binReceiveAddress, destid, messageBufferAddress, offset, length);
                }
            }
            return noiseKind;
        }

        /// <summary>
        /// The VerifyRequest.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool VerifyRequest()
        {
            bool verified = false;
            string ip = tr.MyHeader.Context.RemoteEndPoint.Address.ToString();

            if (HttpHeaders.ContainsKey("DealerToken"))
                if (HttpHeaders["DealerToken"].ToString() != "")
                {
                    string token = HttpHeaders["DealerToken"].ToString();
                    MemberIdentity di = null;
                    if (DealServer.Security.Register(token, out di, ip))
                    {
                        verified = true;
                        HttpOptions["DealerToken"] = di.Token;
                        HttpOptions["DealerUserId"] = di.UserId;
                        HttpOptions["DealerDeptId"] = di.DeptId;
                    }
                }

            if (!verified)
                if (HttpHeaders.ContainsKey("Authorization"))
                    if (HttpHeaders["Authorization"].ToString() != "")
                    {
                        string[] codes = HttpHeaders["Authorization"].ToString().Split(' ');
                        string decode64 = "";
                        string name = "";
                        string key = "";
                        if (codes.Length > 1)
                        {
                            decode64 = Encoding.UTF8.GetString(Convert.FromBase64String(codes[1]));
                            string[] namekey = decode64.Split(':');
                            name = namekey[0];
                            key = namekey[1];

                            MemberIdentity di = null;
                            if (DealServer.Security.Register(name, key, out di, ip))
                            {
                                verified = true;
                                HttpOptions["DealerToken"] = di.Token;
                                HttpOptions["DealerUserId"] = di.UserId;
                                HttpOptions["DealerDeptId"] = di.DeptId;
                            }
                        }
                    }

            return verified;
        }

        /// <summary>
        /// The streamReadLine.
        /// </summary>
        /// <param name="inputStream">The inputStream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            StringBuilder data = new StringBuilder();
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data.Append(((byte)next_char).ToChars(CharEncoding.UTF8));
            }
            return data.ToString();
        }

        /// <summary>
        /// The writeClose.
        /// </summary>
        /// <param name="state">The state<see cref="string"/>.</param>
        private void writeClose(string state = "close")
        {
            ResponseBuilder.AppendLine("Connection: " + state);
            ResponseBuilder.AppendLine("");
        }

        /// <summary>
        /// The writeDenied.
        /// </summary>
        private void writeDenied()
        {
            ResponseBuilder.AppendLine("HTTP/1.1 401.7 Access denied");
        }

        /// <summary>
        /// The writeFailure.
        /// </summary>
        private void writeFailure()
        {
            ResponseBuilder.AppendLine("HTTP/1.1 404 File not found");
        }

        /// <summary>
        /// The writeOptions.
        /// </summary>
        private void writeOptions()
        {
            if (HttpOptions.Count > 0)
                foreach (DictionaryEntry option in HttpOptions)
                    ResponseBuilder.AppendLine(string.Format("{0}: {1}", option.Key, option.Value));

            ResponseBuilder.AppendLine("Accept: application/json");
            ResponseBuilder.AppendLine("Access-Control-Allow-Headers: content-type");
            ResponseBuilder.AppendLine("Access-Control-Allow-Origin: " + HttpHeaders["Origin"].ToString());
        }

        /// <summary>
        /// The writeSuccess.
        /// </summary>
        /// <param name="content_type">The content_type<see cref="string"/>.</param>
        private void writeSuccess(string content_type = "text/html")
        {
            ResponseBuilder.AppendLine("HTTP/1.1 200 OK");
            ResponseBuilder.AppendLine("Content-Type: " + content_type);
        }

        #endregion
    }
}
