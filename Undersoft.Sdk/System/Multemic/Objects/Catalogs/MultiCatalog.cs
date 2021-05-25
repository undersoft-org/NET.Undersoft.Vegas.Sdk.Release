using System.Collections.Generic;
using System.Uniques;
using System.Threading;

/*******************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Catalog   
    
    Default Implementation of Safe-Thread Catalog class
    using 64 bit hash code and long representation;  

    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ********************************************************************************/
namespace System.Multemic
{
    public class MultiCatalog<V> : SharedMultiAlbum<V> where V : IUnique
    {
        public MultiCatalog(int capacity = 17) : base(capacity) { }
        public MultiCatalog(IList<V> collection, int capacity = 17) : base(collection, capacity) { }
        public MultiCatalog(IList<IUnique<V>> collection, int capacity = 17) : base(collection, capacity) { }
        public MultiCatalog(IEnumerable<V> collection, int capacity = 17) : base(collection, capacity) { }
        public MultiCatalog(IEnumerable<IUnique<V>> collection, int capacity = 17) : base(collection, capacity) { }

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
        public override ICard<V>[] EmptyCardList(int size)
        {
            return new MultiCard<V>[size];
        }
    }

}
