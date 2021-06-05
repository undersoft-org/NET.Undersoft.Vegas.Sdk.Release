using System.Collections.Generic;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Deck      

    Default Implementation of Deck class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Sets
{  
    public class MassDeck<V> : BaseMassDeck<V> where V : IUnique                                                   
    {
        public MassDeck(int capacity = 9) : base(capacity) { }
        public MassDeck(IList<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public MassDeck(IList<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }   
        public MassDeck(IEnumerable<ICard<V>> collection, int capacity = 9) : base(collection, capacity) { }
        public MassDeck(IEnumerable<IUnique<V>> collection, int capacity = 9) : base(collection, capacity) { }

        public override ICard<V> EmptyCard()
        {
            return new MassCard<V>();
        }

        public override ICard<V> NewCard(ulong key, V value)
        {
            return new MassCard<V>(key, value);
        }
        public override ICard<V> NewCard(object key, V value)
        {
            return new MassCard<V>(key, value);
        }
        public override ICard<V> NewCard(V value)
        {
            return new MassCard<V>(value);
        }
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new MassCard<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MassCard<V>[size];
        }
    }

}
