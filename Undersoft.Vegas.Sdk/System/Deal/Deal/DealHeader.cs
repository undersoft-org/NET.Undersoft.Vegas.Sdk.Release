using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Instant;
using System.Net;

namespace System.Deal
{

    [Serializable]
    public class DealHeader : ISerialFormatter, IDisposable
    {
        [NonSerialized]
        private DealTransfer transaction;

        public DealHeader()
        {
            Context = new DealContext();
            SerialCount = 0; DeserialCount = 0;
        }
        public DealHeader(DealTransfer _transaction)
        {
            Context = new DealContext();          
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
        public DealHeader(DealTransfer _transaction, MemberIdentity identity)
        {
            Context = new DealContext();
            Context.Identity = identity;
            Context.IdentitySite = identity.Site;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
        public DealHeader(DealTransfer _transaction, ITransferContext context)
        {
            Context = new DealContext();
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }
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

        public void BindContext(ITransferContext context)
        {
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;                     
        }       
      
        public DealContext Context
        { get; set; }

        public object Content
        {
            get;
            set;
        }

        public void Dispose()
        {
            Content = null;
        }

        #region Serialization

        public int Serialize(Stream tostream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(tostream);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(tostream);
            else
                return -1;
        }
        public int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(buffer);
            else
                return -1;
        }

        public object Deserialize(Stream fromstream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(fromstream);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(fromstream);
            else
                return null;
        }
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(buffer);
            else
                return null;
        }

        public object[] GetMessage()
        {
            return null;
        }
        public object GetHeader()
        {
            return this;
        }

        public int SerialCount { get; set; }
        public int DeserialCount { get; set; }
        public int ProgressCount { get; set; }
        public int ItemsCount { get { return Context.ObjectsCount; } }

        #endregion
    }

    public static class BinaryHeader
    {
        public static int SetBinary(this DealHeader bank, Stream tostream)
        {
            if (tostream == null) tostream = new MemoryStream();
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(tostream, bank);
            return (int)tostream.Length;
        }
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
    }

    public static class JsonHeader
    {
        public static int SetJson(this DealHeader thdr, StringBuilder stringbuilder)
        {
            stringbuilder.AppendLine(thdr.SetJsonString());
            return stringbuilder.Length;
        }
        public static int SetJson(this DealHeader thdr, Stream tostream)
        {
            BinaryWriter binwriter = new BinaryWriter(tostream);
            binwriter.Write(thdr.SetJsonString());
            return (int)tostream.Length;
        }
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

        public static string SetJsonString(this DealHeader thdr)
        {
            IDictionary<string, object> toJson = thdr.SetJsonBag();
            return JsonParser.ToJson(toJson, thdr.Context.Complexity);
        }
        public static string SetJsonString(this DealHeader thdr, IDictionary<string, object> jsonbag)
        {
            return JsonParser.ToJson(jsonbag, thdr.Context.Complexity);
        }

        public static IDictionary<string, object> SetJsonBag(this DealHeader thdr)
        {
            return new Dictionary<string, object>() { { "DealHeader", JsonParserProperties.GetJsonProperties(typeof(DealHeader), thdr.Context.Complexity)
                                                                       .Select(k => new KeyValuePair<string, object>(k.Name, k.GetValue(thdr, null)))
                                                                       .ToDictionary(k => k.Key, v => v.Value) } };
        }

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

        public static void GetJsonBag(this DealHeader thdr, string JsonString, IDictionary<string, object> _bag)
        {
            _bag.Add(JsonParser.FromJson(JsonString));
        }

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
    }

}
