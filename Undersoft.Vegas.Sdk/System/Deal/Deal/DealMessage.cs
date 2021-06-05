/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealMessage.cs
   
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
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="BinaryMessage" />.
    /// </summary>
    public static class BinaryMessage
    {
        #region Methods

        /// <summary>
        /// The GetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealMessage"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetBinary(this DealMessage bank, ISerialBuffer buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer.DeserialBlock);
                BinaryFormatter binform = new BinaryFormatter();
                DealMessage _bank = (DealMessage)binform.Deserialize(ms);
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
        /// <param name="bank">The bank<see cref="DealMessage"/>.</param>
        /// <param name="fromstream">The fromstream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetBinary(this DealMessage bank, Stream fromstream)
        {
            try
            {
                BinaryFormatter binform = new BinaryFormatter();
                return (DealMessage)binform.Deserialize(fromstream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The SetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealMessage"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetBinary(this DealMessage bank, ISerialBuffer buffer)
        {
            int offset = buffer.BlockOffset;
            MemoryStream ms = new MemoryStream();
            ms.Write(new byte[offset], 0, offset);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(ms, bank);
            buffer.SerialBlock = ms.ToArray();
            ms.Dispose();
            return buffer.SerialBlock.Length;
        }

        /// <summary>
        /// The SetBinary.
        /// </summary>
        /// <param name="bank">The bank<see cref="DealMessage"/>.</param>
        /// <param name="tostream">The tostream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetBinary(this DealMessage bank, Stream tostream)
        {
            if (tostream == null) tostream = new MemoryStream();
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(tostream, bank);
            return (int)tostream.Length;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="DealMessage" />.
    /// </summary>
    [Serializable]
    public class DealMessage : ISerialFormatter, IDisposable
    {
        #region Fields

        private object content;
        private DirectionType direction;
        [NonSerialized]
        private DealTransfer transaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealMessage"/> class.
        /// </summary>
        public DealMessage()
        {
            content = new object();
            SerialCount = 0;
            DeserialCount = 0;
            direction = DirectionType.Receive;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealMessage"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        /// <param name="_direction">The _direction<see cref="DirectionType"/>.</param>
        /// <param name="message">The message<see cref="object"/>.</param>
        public DealMessage(DealTransfer _transaction, DirectionType _direction, object message = null)
        {
            transaction = _transaction;
            direction = _direction;

            if (message != null)
                Content = message;
            else
                content = new object();

            SerialCount = 0;
            DeserialCount = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public object Content
        {
            get { return content; }
            set { transaction.Manager.MessageContent(ref content, value, direction); }
        }

        /// <summary>
        /// Gets or sets the DeserialCount.
        /// </summary>
        public int DeserialCount { get; set; }

        /// <summary>
        /// Gets the ItemsCount.
        /// </summary>
        public int ItemsCount
        {
            get { return (content != null) ? ((ISerialFormatter[])content).Sum(t => t.ItemsCount) : 0; }
        }

        /// <summary>
        /// Gets or sets the Notice.
        /// </summary>
        public string Notice { get; set; }

        /// <summary>
        /// Gets the ObjectsCount.
        /// </summary>
        public int ObjectsCount
        {
            get { return (content != null) ? ((ISerialFormatter[])content).Length : 0; }
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
                return -1;
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
                return -1;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            content = null;
        }

        /// <summary>
        /// The GetHeader.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetHeader()
        {
            if (direction == DirectionType.Send)
                return transaction.MyHeader.Content;
            else
                return transaction.HeaderReceived.Content;
        }

        /// <summary>
        /// The GetMessage.
        /// </summary>
        /// <returns>The <see cref="object[]"/>.</returns>
        public object[] GetMessage()
        {
            if (content != null)
                return (ISerialFormatter[])content;
            return null;
        }

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="buffor">The buffor<see cref="ISerialBuffer"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Serialize(ISerialBuffer buffor, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(buffor);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(buffor);
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
    /// Defines the <see cref="JsonMessage" />.
    /// </summary>
    public static class JsonMessage
    {
        #region Methods

        /// <summary>
        /// The GetJson.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, ISerialBuffer buffer)
        {
            try
            {
                DealMessage trs = null;

                byte[] _fromarray = buffer.DeserialBlock;
                StringBuilder sb = new StringBuilder();

                sb.Append(_fromarray.ToChars(CharEncoding.UTF8));
                trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];

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
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="fromstream">The fromstream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, Stream fromstream)
        {
            try
            {
                fromstream.Position = 0;
                byte[] array = new byte[4096];
                int read = 0;
                StringBuilder sb = new StringBuilder();
                while ((read = fromstream.Read(array, 0, array.Length)) > 0)
                {
                    sb.Append(array.Cast<char>());
                }
                DealMessage trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];
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
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="jsonstring">The jsonstring<see cref="string"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, string jsonstring)
        {
            try
            {
                DealMessage trs = tmsg.GetJsonObject(jsonstring)["DealMessage"];
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
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="stringbuilder">The stringbuilder<see cref="StringBuilder"/>.</param>
        /// <returns>The <see cref="DealMessage"/>.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, StringBuilder stringbuilder)
        {
            try
            {
                StringBuilder sb = stringbuilder;
                DealMessage trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];
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
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="JsonString">The JsonString<see cref="string"/>.</param>
        /// <param name="_bag">The _bag<see cref="IDictionary{string, object}"/>.</param>
        public static void GetJsonBag(this DealMessage tmsg, string JsonString, IDictionary<string, object> _bag)
        {
            _bag.Add(JsonParser.FromJson(JsonString));
        }

        /// <summary>
        /// The GetJsonObject.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="_bag">The _bag<see cref="IDictionary{string, object}"/>.</param>
        /// <returns>The <see cref="Dictionary{string, DealMessage}"/>.</returns>
        public static Dictionary<string, DealMessage> GetJsonObject(this DealMessage tmsg, IDictionary<string, object> _bag)
        {
            Dictionary<string, DealMessage> result = new Dictionary<string, DealMessage>();
            IDictionary<string, object> bags = _bag;
            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealMessage));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealMessage deck = (DealMessage)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }

        /// <summary>
        /// The GetJsonObject.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="JsonString">The JsonString<see cref="string"/>.</param>
        /// <returns>The <see cref="Dictionary{string, DealMessage}"/>.</returns>
        public static Dictionary<string, DealMessage> GetJsonObject(this DealMessage tmsg, string JsonString)
        {
            Dictionary<string, DealMessage> result = new Dictionary<string, DealMessage>();
            Dictionary<string, object> bags = new Dictionary<string, object>();
            tmsg.GetJsonBag(JsonString, bags);

            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealMessage));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealMessage deck = (DealMessage)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealMessage tmsg, ISerialBuffer buffer)
        {
            buffer.SerialBlock = Encoding.UTF8.GetBytes(tmsg.SetJsonString());
            return buffer.SerialBlock.Length;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="tostream">The tostream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealMessage tmsg, Stream tostream)
        {
            BinaryWriter binwriter = new BinaryWriter(tostream);
            binwriter.Write(tmsg.SetJsonString());
            return (int)tostream.Length;
        }

        /// <summary>
        /// The SetJson.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="stringbuilder">The stringbuilder<see cref="StringBuilder"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int SetJson(this DealMessage tmsg, StringBuilder stringbuilder)
        {
            stringbuilder.AppendLine(tmsg.SetJsonString());
            return stringbuilder.Length;
        }

        /// <summary>
        /// The SetJsonBag.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <returns>The <see cref="Dictionary{string, object}"/>.</returns>
        public static Dictionary<string, object> SetJsonBag(this DealMessage tmsg)
        {
            return new Dictionary<string, object>() { { "DealMessage", JsonParserProperties.GetJsonProperties(typeof(DealMessage))
                                                                       .Select(k => new KeyValuePair<string, object>(k.Name, k.GetValue(tmsg, null)))
                                                                       .ToDictionary(k => k.Key, v => v.Value) } };
        }

        /// <summary>
        /// The SetJsonString.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SetJsonString(this DealMessage tmsg)
        {
            IDictionary<string, object> toJson = tmsg.SetJsonBag();
            return JsonParser.ToJson(toJson);
        }

        /// <summary>
        /// The SetJsonString.
        /// </summary>
        /// <param name="tmsg">The tmsg<see cref="DealMessage"/>.</param>
        /// <param name="jsonbag">The jsonbag<see cref="IDictionary{string, object}"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SetJsonString(this DealMessage tmsg, IDictionary<string, object> jsonbag)
        {
            return JsonParser.ToJson(jsonbag);
        }

        #endregion
    }
}
