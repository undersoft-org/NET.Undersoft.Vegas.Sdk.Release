using System.IO;

namespace System.Instant
{
    public interface IFigureFormatter
    {
        int SerialCount { get; set; }
        int DeserialCount { get; set; }
        int ProgressCount { get; set; }
        int ItemsCount { get; }

        int Serialize(Stream stream, int offset, int batchSize, FigureFormat serialFormat = FigureFormat.Binary);
        int Serialize(IFigurePacket buffor, int offset, int batchSize, FigureFormat serialFormat = FigureFormat.Binary);

        object Deserialize(Stream stream, FigureFormat serialFormat = FigureFormat.Binary);
        object Deserialize(ref object source, FigureFormat serialFormat = FigureFormat.Binary);

        object[] GetMessage();
        object GetHeader();
    }

    public interface IDealSource
    {
        object Emulate(object source, string name = null);
        object Impact(object source, string name = null);
        object Locate(object path = null);
    }

   
}