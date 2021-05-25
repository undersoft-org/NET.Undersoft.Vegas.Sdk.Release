using System.Collections.Generic;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Deck      

    Default Implementation of Deck class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Multemic
{  
    public class Deck<V> : CardList<V>                                                     
    {
        public Deck(int capacity = 9) : base(capacity) { }
        public Deck(IList<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public Deck(IList<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }   
        public Deck(IEnumerable<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public Deck(IEnumerable<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }

        public override ICard<V> EmptyCard()
        {
            return new Card64<V>();
        }

        public override ICard<V> NewCard(long key, V value)
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
    }

}
