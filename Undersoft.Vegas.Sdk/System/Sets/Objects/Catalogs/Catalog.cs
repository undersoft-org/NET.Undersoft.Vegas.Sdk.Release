using System.Collections.Generic;
using System.Uniques;
using System.Threading;

/*******************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Catalog   
    
    Default Implementation of Safe-Thread Catalog class
    using 64 bit hash code and long representation;  

    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ********************************************************************************/
namespace System.Sets
{
    public class Catalog<V> : BaseCatalog<V>
    {
        public Catalog(int capacity = 17) : base(capacity) { }
        public Catalog(IList<V> collection, int capacity = 17) : base(collection, capacity) { }
        public Catalog(IList<IUnique<V>> collection, int capacity = 17) : base(collection, capacity) { }
        public Catalog(IEnumerable<V> collection, int capacity = 17) : base(collection, capacity) { }
        public Catalog(IEnumerable<IUnique<V>> collection, int capacity = 17) : base(collection, capacity) { }

        public override ICard<V> EmptyCard()
        {
            return new Card<V>();
        }

        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card<V>(key, value);
        }
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card<V>(key, value);
        }
        public override ICard<V> NewCard(V value)
        {
            return new Card<V>(value);
        }
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card<V>[size];
        }
        public override ICard<V>[] EmptyBaseDeck(int size)
        {
            return new Card<V>[size];
        }
    }

}
