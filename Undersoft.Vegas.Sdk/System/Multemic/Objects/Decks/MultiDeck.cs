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
    public class MultiDeck<V> : CardList<V> where V : IUnique                                                   
    {
        public MultiDeck(int capacity = 9) : base(capacity) { }
        public MultiDeck(IList<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public MultiDeck(IList<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }   
        public MultiDeck(IEnumerable<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public MultiDeck(IEnumerable<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }

        public override ICard<V> EmptyCard()
        {
            return new MultiCard<V>();
        }

        public override ICard<V> NewCard(long key, V value)
        {
            return new MultiCard<V>(key, value);
        }
        public override ICard<V> NewCard(object key, V value)
        {
            return new MultiCard<V>(key, value);
        }
        public override ICard<V> NewCard(V value)
        {
            return new MultiCard<V>(value);
        }
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new MultiCard<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MultiCard<V>[size];
        }
    }

}
