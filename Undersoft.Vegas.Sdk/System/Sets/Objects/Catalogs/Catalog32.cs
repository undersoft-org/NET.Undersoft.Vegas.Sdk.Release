using System.Uniques;
using System.Collections.Generic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Catalog32
    
    Implementation of Safe-Thread Album abstract class
    using 32 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                      
 
 ******************************************************************/
namespace System.Sets
{  
    public class Catalog32<V> : BaseCatalog<V>
    {        
        public Catalog32(int capacity = 17) : base(capacity, HashBits.bit32)
        {
        }
        public Catalog32(IList<V> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog32(IList<IUnique<V>> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog32(IEnumerable<V> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Catalog32(IEnumerable<IUnique<V>> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        public override ICard<V> EmptyCard()
        {
            return new Card32<V>();
        }

        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card32<V>(key, value);
        }
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card32<V>(key, value);
        }
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card32<V>(card);
        }
        public override ICard<V> NewCard(V value)
        {
            return new Card32<V>(value);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card32<V>[size];
        }
        public override ICard<V>[] EmptyBaseDeck(int size)
        {
            return new Card32<V>[size];
        }
    }   
}
