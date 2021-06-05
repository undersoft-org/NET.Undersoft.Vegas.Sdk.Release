using System.Uniques;
using System.Collections.Generic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Catalog64
    
    Implementation of Safe-Thread Album abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT
 ******************************************************************/
namespace System.Sets
{  
    public class Catalog64<V> : BaseCatalog<V>
    {
        #region Constructor

        public Catalog64(int capacity = 17) : base(capacity, HashBits.bit64)
        {
        }
        public Catalog64(IList<V> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog64(IList<IUnique<V>> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog64(IEnumerable<V> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog64(IEnumerable<IUnique<V>> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion        

        public override ICard<V> EmptyCard()
        {
            return new Card64<V>();
        }

        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card64<V>(key, value);
        }
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card64<V>(key, value);
        }
        public override ICard<V> NewCard(V value)
        {
            return new Card64<V>(value);
        }
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card64<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card64<V>[size];
        }
        public override ICard<V>[] EmptyBaseDeck(int size)
        {
            return new Card64<V>[size];
        }

    }   
}
