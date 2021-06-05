/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealHeader.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="BinaryHeader" />.
    /// </summary>
    public static class BinaryHeader
    {
        #region Methods

        /// <summary>
        /// The GetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealHeader"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetBinary(this DealHeader bank, ISerialBuffer buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer.DeserialBlock);
                BinaryFormatter binform = new BinaryFormatter();
                DealHeader _bank = (DealHeader)binform.Deserialize(ms);
                ms.Dispose();
                return _bank;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealHeader"/>.</param>
        /// <param name="fromstream">The fromstream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetBinary(this DealHeader bank, Stream fromstream)
        {
            try
            {
                BinaryFormatter binform = new BinaryFormatter();
                return (DealHeader)binform.Deserialize(fromstream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The SetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealHeader"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetBinary(this DealHeader bank, ISerialBuffer buffer)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(new byte[buffer.BlockOffset], 0, buffer.BlockOffset);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(ms, bank);
            buffer.SerialBlock = ms.ToArray();
            ms.Dispose();
            return buffer.SerialBlock.Length;
        }

        /// <summary>
        /// The SetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealHeader"/>.</param>
        /// <param name="tostream">The tostream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetBinary(this DealHeader bank, Stream tostream)
        {
            if (tostream == null) tostream = new MemoryStream();
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(tostream, bank);
            return (int)tostream.Length;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="DealHeader" />.
    /// </summary>
    [Serializable]
    public class DealHeader : ISerialFormatter, IDisposable
    {
        #region Fields

        [NonSerialized]
        private DealTransfer transaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader"/> class.
        /// </summary>
        public DealHeader()
        {
            Context = new DealContext();
            SerialCount = 0; DeserialCount = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        public DealHeader(DealTransfer _transaction)
        {
            Context = new DealContext();
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        public DealHeader(DealTransfer _transaction, ITransferContext context)
        {
            Context = new DealContext();
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        /// <param name="identity">The identity<see cref="MemberIdentity"/>.</param>
        public DealHeader(DealTransfer _transaction, ITransferContext context, MemberIdentity identity)
        {
            Context = new DealContext();
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
            Context.Identity = identity;
            Context.IdentitySite = identity.Site;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        /// <param name="identity">The identity<see cref="MemberIdentity"/>.</param>
        public DealHeader(DealTransfer _transaction, MemberIdentity identity)
        {
            Context = new DealContext();
            Context.Identity = identity;
            Context.IdentitySite = identity.Site;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the Context.
        /// </summary>
        public DealContext Context { get; set; }

        /// <summary>
        /// Gets or sets the DeserialCount.
        /// </summary>
        public int DeserialCount { get; set; }

        /// <summary>
        /// Gets the ItemsCount.
        /// </summary>
        public int ItemsCount
        {
            get { return Context.ObjectsCount; }
        }

        /// <summary>
        /// Gets or sets the ProgressCount.
        /// </summary>
        public int ProgressCount { get; set; }

        /// <summary>
        /// Gets or sets the SerialCount.
        /// </summary>
        public int SerialCount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The BindContext.
        /// </summary>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        public void BindContext(ITransferContext context)
        {
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
        }

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(buffer);
            else
                return null;
        }

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <param name="fromstream">The fromstream<see cref="Stream"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Deserialize(Stream fromstream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(fromstream);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(fromstream);
            else
                return null;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            Content = null;
        }

        /// <summary>
        /// The GetHeader.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetHeader()
        {
            return this;
        }

        /// <summary>
        /// The GetMessage.
        /// </summary>
        /// <returns>The <see cref="object[]"/>.</returns>
        public object[] GetMessage()
        {
            return null;
        }

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(buffer);
            else
                return -1;
        }

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="tostream">The tostream<see cref="Stream"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Serialize(Stream tostream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(tostream);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(tostream);
            else
                return -1;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonHeader" />.
    /// </summary>
    public static class JsonHeader
    {
        #region Methods

        /// <summary>
        /// The GetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetJson(this DealHeader thdr, ISerialBuffer buffer)
        {
            try
            {
                DealHeader trs = null;

                byte[] _fromarray = buffer.DeserialBlock;
                StringBuilder sb = new StringBuilder();

                sb.Append(_fromarray.ToChars(CharEncoding.UTF8));
                trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];

                _fromarray = null;
                sb = null;
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="fromstream">The fromstream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetJson(this DealHeader thdr, Stream fromstream)
        {
            try
            {
                fromstream.Position = 0;
                byte[] array = new byte[4096];
                int read = 0;
                StringBuilder sb = new StringBuilder();
                while ((read = fromstream.Read(array, 0, array.Length)) > 0)
                {
                    sb.Append(array.Select(b => (char)b).ToArray());
                }
                DealHeader trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];
                sb = null;
                fromstream.Dispose();
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="jsonstring">The jsonstring<see cref="string"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetJson(this DealHeader thdr, string jsonstring)
        {
            try
            {
                DealHeader trs = thdr.GetJsonObject(jsonstring)["DealHeader"];
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="stringbuilder">The stringbuilder<see cref="StringBuilder"/>.</param>
        /// <returns>The <see cref="DealHeader"/>.</returns>
        public static DealHeader GetJson(this DealHeader thdr, StringBuilder stringbuilder)
        {
            try
            {
                StringBuilder sb = stringbuilder;
                DealHeader trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetJsonBag.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="JsonString">The JsonString<see cref="string"/>.</param>
        /// <param name="_bag">The _bag<see cref="IDictionary{string, object}"/>.</param>
        public static void GetJsonBag(this DealHeader thdr, string JsonString, IDictionary<string, object> _bag)
        {
            _bag.Add(JsonParser.FromJson(JsonString));
        }

        /// <summary>
        /// The GetJsonObject.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="_bag">The _bag<see cref="IDictionary{string, object}"/>.</param>
        /// <returns>The <see cref="Dictionary{string, DealHeader}"/>.</returns>
        public static Dictionary<string, DealHeader> GetJsonObject(this DealHeader thdr, IDictionary<string, object> _bag)
        {
            Dictionary<string, DealHeader> result = new Dictionary<string, DealHeader>();
            IDictionary<string, object> bags = _bag;
            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealHeader));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealHeader deck = (DealHeader)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }

        /// <summary>
        /// The GetJsonObject.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="JsonString">The JsonString<see cref="string"/>.</param>
        /// <returns>The <see cref="Dictionary{string, DealHeader}"/>.</returns>
        public static Dictionary<string, DealHeader> GetJsonObject(this DealHeader thdr, string JsonString)
        {
            Dictionary<string, DealHeader> result = new Dictionary<string, DealHeader>();
            Dictionary<string, object> bags = new Dictionary<string, object>();
            thdr.GetJsonBag(JsonString, bags);

            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealHeader));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealHeader deck = (DealHeader)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealHeader thdr, ISerialBuffer buffer, int offset = 0)
        {
            if (offset > 0)
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(thdr.SetJsonString());
                byte[] serialBytes = new byte[jsonBytes.Length + offset];
                jsonBytes.CopyTo(serialBytes, offset);
                buffer.SerialBlock = serialBytes;
                jsonBytes = null;
            }
            else
                buffer.SerialBlock = Encoding.UTF8.GetBytes(thdr.SetJsonString());

            return buffer.SerialBlock.Length;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="tostream">The tostream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealHeader thdr, Stream tostream)
        {
            BinaryWriter binwriter = new BinaryWriter(tostream);
            binwriter.Write(thdr.SetJsonString());
            return (int)tostream.Length;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="stringbuilder">The stringbuilder<see cref="StringBuilder"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealHeader thdr, StringBuilder stringbuilder)
        {
            stringbuilder.AppendLine(thdr.SetJsonString());
            return stringbuilder.Length;
        }

        /// <summary>
        /// The SetJsonBag.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        public static IDictionary<string, object> SetJsonBag(this DealHeader thdr)
        {
            return new Dictionary<string, object>() { { "DealHeader", JsonParserProperties.GetJsonProperties(typeof(DealHeader), thdr.Context.Complexity)
                                                                       .Select(k => new KeyValuePair<string, object>(k.Name, k.GetValue(thdr, null)))
                                                                       .ToDictionary(k => k.Key, v => v.Value) } };
        }

        /// <summary>
        /// The SetJsonString.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SetJsonString(this DealHeader thdr)
        {
            IDictionary<string, object> toJson = thdr.SetJsonBag();
            return JsonParser.ToJson(toJson, thdr.Context.Complexity);
        }

        /// <summary>
        /// The SetJsonString.
        /// </summary>
        /// <param name="thdr">The thdr<see cref="DealHeader"/>.</param>
        /// <param name="jsonbag">The jsonbag<see cref="IDictionary{string, object}"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SetJsonString(this DealHeader thdr, IDictionary<string, object> jsonbag)
        {
            return JsonParser.ToJson(jsonbag, thdr.Context.Complexity);
        }

        #endregion
    }
}
