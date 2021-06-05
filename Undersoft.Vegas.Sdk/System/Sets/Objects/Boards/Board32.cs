using System.Uniques;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Board32
    
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Sets
{  
    public class Board32<V> : BaseBoard<V>
    {        
        public Board32(int _deckSize = 9, HashBits bits = HashBits.bit64) : base(_deckSize, bits)
        {
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
    }   
}
