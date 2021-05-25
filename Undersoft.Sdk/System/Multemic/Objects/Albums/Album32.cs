using System.Uniques;
using System.Collections.Generic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Catalog32   
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                            
 
 ******************************************************************/
namespace System.Multemic
{  
    public class Album32<V> : CardBook<V>
    {
        public Album32() : base(17, HashBits.bit32)
        { 
        } 
        public Album32(int _deckSize = 17) : base(_deckSize, HashBits.bit32)
        {
        }
        public Album32(ICollection<IUnique<V>> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit32)
        {
        }
        public Album32(ICollection<V> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit32)
        {
        }
        public Album32(IEnumerable<IUnique<V>> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit32)
        {
        }
        public Album32(IEnumerable<V> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit32)
        {
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
        public override ICard<V> NewCard(V value)
        {
            return new Card32<V>(value);
        }

        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card32<V>(card);
        }

        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card32<V>[size];
        }

        public override ICard<V>[] EmptyCardList(int size)
        {
            return new Card32<V>[size];
        }
    }
}
