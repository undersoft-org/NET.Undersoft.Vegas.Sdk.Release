using System.Linq;
using System.Collections.Generic;
using System.Uniques;
using System.Threading;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.MultiBoard   
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                        
             
 *********************************************************************************/
namespace System.Sets
{  
    public class MassBoard<V> : BaseMassBoard<V> where V : IUnique                                                    
    {
        #region Constructor

        public MassBoard(int capacity = 9, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }
        public MassBoard(IList<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public MassBoard(IEnumerable<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion

        public override   ICard<V> EmptyCard()
        {
            return new MassCard<V>();
        }

        public override   ICard<V> NewCard(ulong key, V value)
        {
            return new MassCard<V>(key, value);
        }
        public override   ICard<V> NewCard(object key, V value)
        {
            return new MassCard<V>(key, value);
        }
        public override   ICard<V> NewCard(V value)
        {
            return new MassCard<V>(value, value);
        }
        public override   ICard<V> NewCard(ICard<V> card)
        {
            return new MassCard<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MassCard<V>[size];
        }      
    }

}
