namespace System.Multemic
{
    public interface ICard<V>: IUnique<V>, IDisposable
    {
        ICard<V> Extent { get; set; }
        long Key { get; set; }
        ICard<V> Next { get; set; }
        int Index { get; set; }
        bool Removed { get; set; }

        int CompareTo(ICard<V> other);
        int CompareTo(long key);
        int CompareTo(object other);
        bool Equals(ICard<V> y);
        bool Equals(long key);
        bool Equals(object y);
        int GetHashCode();
        Type GetUniqueType();
        void Set(ICard<V> card);
        void Set(long key, V value);
        void Set(object key, V value);
        void Set(V value);        
    }
}