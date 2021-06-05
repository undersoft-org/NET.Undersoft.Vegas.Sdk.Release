using System.Uniques;
using System.Collections.Generic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Deck32
    
    Implementation of Deck abstract class
    using 32 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                          
 
 ******************************************************************/
namespace System.Multemic
{  
    public class Deck32<V> : CardList<V>
    {
        public Deck32(int capacity = 9) : base(capacity, HashBits.bit32)
        {
        }
        public Deck32(IList<ICard<V>> collection, int capacity = 9) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Deck32(IList<IUnique<V>> collection, int capacity = 9) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Deck32(IEnumerable<ICard<V>> collection, int capacity = 9) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Deck32(IEnumerable<IUnique<V>> collection, int capacity = 9) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        public override ICard<V> EmptyCard()
        {
            return new Card32<V>();
        }

        public override ICard<V> NewCard(long key, V value)
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
            return new Card32<V>(value, value);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card32<V>[size];
        }
    }
}
