using System.Linq;
using System.Collections.Generic;
using System.Uniques;
using System.Threading;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.MultiBoard   
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                        
             
 *********************************************************************************/
namespace System.Multemic
{  
    public class MultiBoard<V> : SharedMultiDeck<V> where V : IUnique                                                    
    {
        #region Constructor

        public MultiBoard(int capacity = 9, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }
        public MultiBoard(IList<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public MultiBoard(IEnumerable<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion

        public override   ICard<V> EmptyCard()
        {
            return new MultiCard<V>();
        }

        public override   ICard<V> NewCard(long key, V value)
        {
            return new MultiCard<V>(key, value);
        }
        public override   ICard<V> NewCard(object key, V value)
        {
            return new MultiCard<V>(key, value);
        }
        public override   ICard<V> NewCard(V value)
        {
            return new MultiCard<V>(value, value);
        }
        public override   ICard<V> NewCard(ICard<V> card)
        {
            return new MultiCard<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MultiCard<V>[size];
        }      
    }

}
