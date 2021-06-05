
namespace System
{
    public interface ISerialBuffer : IDisposable
    {
        ServiceSite Site { get; }

        long BlockSize { get; set; }
        int  BlockOffset { get; set; }

        byte[] SerialBlock { get; set; }
        IntPtr SerialBlockPtr { get; }
        int    SerialBlockId { get; set; }

        byte[] DeserialBlock { get; }
        IntPtr DeserialBlockPtr { get; }      
        int    DeserialBlockId { get; set; }

    }
}



