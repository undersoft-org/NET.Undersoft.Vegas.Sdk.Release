using System.IO;

namespace System
{
    public interface ISerialFormatter
    {
        int SerialCount { get; set; }
        int DeserialCount { get; set; }
        int ProgressCount { get; set; }
        int ItemsCount { get; }

        int Serialize(Stream stream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);
        int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);

        object Deserialize(Stream stream, SerialFormat serialFormat = SerialFormat.Binary);
        object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary);

        object[] GetMessage();
        object   GetHeader();
    }
}