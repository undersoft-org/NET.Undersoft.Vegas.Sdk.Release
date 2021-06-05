using System.IO;

namespace System
{
    public interface ISerialObject
    {
        object Merge(object source, string name = null);
        object Locate(object path = null);
    }
}