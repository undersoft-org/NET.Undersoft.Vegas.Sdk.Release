namespace System.Sets
{
    public interface ICard<V>: IUnique<V>, IDisposable
    {
        ICard<V> Extent { get; set; }
        ulong Key { get; set; }
        ICard<V> Next { get; set; }
        int Index { get; set; }
        bool Removed { get; set; }

        int CompareTo(ICard<V> other);
        int CompareTo(ulong key);
        int CompareTo(object other);
        bool Equals(ICard<V> y);
        bool Equals(ulong key);
        bool Equals(object y);
        int GetHashCode();
        Type GetUniqueType();
        void Set(ICard<V> card);
        void Set(ulong key, V value);
        void Set(object key, V value);
        void Set(V value);        
    }
}