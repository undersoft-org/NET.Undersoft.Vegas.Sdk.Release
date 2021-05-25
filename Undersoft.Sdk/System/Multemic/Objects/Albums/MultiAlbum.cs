using System.Collections.Generic;
using System.Uniques;

/*******************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Album
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                        
 ********************************************************************************/
namespace System.Multemic
{
    public class MultiAlbum<V> : MultiCardBook<V> where V : IUnique
    {
        #region Constructor

        public MultiAlbum() : base(17, HashBits.bit64)
        {
        }
        public MultiAlbum(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }
        public MultiAlbum(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {

            foreach (var c in collection)
                this.Add(c);
        }
        public MultiAlbum(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {

            foreach (var c in collection)
                this.Add(c);
        }
        public MultiAlbum(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public MultiAlbum(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion        

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
