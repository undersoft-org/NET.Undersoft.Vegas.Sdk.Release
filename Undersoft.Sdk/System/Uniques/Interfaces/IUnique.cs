using System.Reflection;

namespace System
{
    public interface IUnique<V> : IUnique
    {
        V Value { get; set; }

        int[] IdentityIndexes();

        object[] IdentityValues();

        long IdentitiesToKey();
    }


    public interface IUnique : IEquatable<IUnique>, IComparable<IUnique>
    {
        IUnique Empty { get; }

        byte[]  GetBytes();

        byte[]  GetKeyBytes();       

        void    SetHashKey(long value);      

        long    GetHashKey();      

        long    KeyBlock { get; set; }

        void    SetHashSeed(uint seed);

        uint     GetHashSeed();

        uint     SeedBlock { get; set; }
    }
}