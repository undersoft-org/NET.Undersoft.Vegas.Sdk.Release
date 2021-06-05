/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.BaseMassAlbum.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Sets.Basedeck;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="BaseMassAlbum{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public abstract class BaseMassAlbum<V> : SeededSet<V>, IEnumerable<V>, IEnumerable, ICollection<V> where V : IUnique
    {
        #region Fields

        protected ICard<V>[] list;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        public BaseMassAlbum() : base(17, HashBits.bit64)
        {
            list = EmptyBaseDeck(17);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassAlbum(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            list = EmptyBaseDeck(capacity);
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassAlbum(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            list = EmptyBaseDeck(capacity);
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassAlbum(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            list = EmptyBaseDeck(capacity);
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassAlbum(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            list = EmptyBaseDeck(capacity);
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassAlbum{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassAlbum(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            list = EmptyBaseDeck(capacity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the First.
        /// </summary>
        public override ICard<V> First
        {
            get { return first; }
        }

        /// <summary>
        /// Gets the Last.
        /// </summary>
        public override ICard<V> Last
        {
            get { return list[(count + removed) - 1]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            list = EmptyBaseDeck(minSize);
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="Array"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(Array array, int index)
        {
            int c = count, i = index, l = array.Length;

            if (l - i < c) c = l - i;

            for (int j = 0; j < c; j++)
                array.SetValue(GetCard(j).Value, i++);
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="ICard{V}[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(ICard<V>[] array, int index)
        {
            int c = count, i = index, l = array.Length;

            if (l - i < c) c = l - i;

            for (int j = 0; j < c; j++)
            {
                array[i++] = GetCard(j);
            }
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="V[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(V[] array, int index)
        {
            int c = count, i = index, l = array.Length;

            if (l - i < c) c = l - i;

            for (int j = 0; j < c; j++)
                array[i++] = GetCard(j).Value;
        }

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}[]"/>.</returns>
        public abstract ICard<V>[] EmptyBaseDeck(int size);

        /// <summary>
        /// The Flush.
        /// </summary>
        public override void Flush()
        {
            base.Flush();
            list = EmptyBaseDeck(size);
        }

        /// <summary>
        /// The GetCard.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> GetCard(int index)
        {
            if (index < count)
            {
                if (removed > 0)
                    Reindex();

                return list[index];
            }
            throw new IndexOutOfRangeException("Index out of range");
        }

        /// <summary>
        /// The Next.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> Next(ICard<V> card)
        {
            ICard<V> _card = list[card.Index + 1];
            if (_card != null)
            {
                if (!_card.Removed)
                    return _card;
                return Next(_card);
            }
            return null;
        }

        /// <summary>
        /// The ToArray.
        /// </summary>
        /// <returns>The <see cref="V[]"/>.</returns>
        public override V[] ToArray()
        {
            V[] array = new V[count];
            CopyTo(array, 0);
            return array;
        }

        /// <summary>
        /// The createNew.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected ICard<V> createNew(ICard<V> card)
        {
            int id = count + removed;
            card.Index = id;
            list[id] = card;
            return card;
        }

        /// <summary>
        /// The createNew.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected ICard<V> createNew(ulong key, V value)
        {
            int id = count + removed;
            var newcard = NewCard(key, value);
            newcard.Index = id;
            list[id] = newcard;
            return newcard;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    first = null;
                    last = null;
                }
                table = null;
                list = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(ICard<V> value)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size  
            ulong key = value.Key;
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = createNew(value);
                countIncrement();
                return true;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed decrese counter 
                    if (card.Removed)
                    {
                        /// update card have same key with new value 
                        card.Removed = false;
                        card.Value = value.Value;
                        removedDecrement();
                        return true;
                    }
                    return false;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    card.Extent = createNew(value);
                    conflictIncrement();
                    return true;
                }
                card = card.Extent;
            }
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(ulong key, V value)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key by size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos];
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = createNew(key, value);
                countIncrement();
                return true;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed decrese counter 
                    if (card.Removed)
                    {
                        /// update card have same key with new value 
                        card.Removed = false;
                        card.Value = value;
                        removedDecrement();
                        return true;
                    }
                    return false;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    /// assign new card as extent reference and increase conflicts
                    card.Extent = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
                /// assign next conflict card to local card
                card = card.Extent;
            }
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(V value)
        {
            ulong key = unique.Key(value, value.UniqueSeed);
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key by size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos];
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = createNew(key, value);
                countIncrement();
                return true;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed decrese counter 
                    if (card.Removed)
                    {
                        /// update card have same key with new value 
                        card.Removed = false;
                        card.Value = value;
                        removedDecrement();
                        return true;
                    }
                    return false;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    /// assign new card as extent reference and increase conflicts
                    card.Extent = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
                /// assign next conflict card to local card
                card = card.Extent;
            }
        }

        /// <summary>
        /// The InnerInsert.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="item">The item<see cref="ICard{V}"/>.</param>
        protected override void InnerInsert(int index, ICard<V> item)
        {

            int c = count - index;
            if (c > 0)
            {
                // take card at index to replace - removed counter must be 0 otherwise reindex with insert;
                if (removed > 0)
                    reindexWithInsert(index, item);
                else
                {

                    var replaceCard = GetCard(index);

                    while (replaceCard != null)
                    {
                        int id = ++replaceCard.Index;
                        var _replaceCard = list[id];
                        list[id] = replaceCard;
                        replaceCard = _replaceCard;
                    }

                    item.Index = index;
                    list[index] = item;
                }
            }
            else
            {
                // insert card at last position - there is no need to reindex;  
                int id = count + removed;
                item.Index = id;
                list[id] = item;
            }
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            ulong key = value.Key;
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = createNew(value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// update card have same key with new value 
                    card.Value = value.Value;
                    /// when card was removed decrese counter 
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    return card;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    var newcard = createNew(value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                card = card.Extent;
            }
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(ulong key, V value)
        {
            ulong pos = getPosition(key);
            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed decrese counter 
                    card.Value = value;
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    /// update card have same key with new value 

                    return card;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    var newcard = createNew(key, value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                /// assign next conflict card to local card
                card = card.Extent;
            }
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(V value)
        {
            ulong key = unique.Key(value, value.UniqueSeed);
            ulong pos = getPosition(key);
            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = createNew(key, value);
                table[pos] = card;
                countIncrement();
                return card;
            }

            // loop over conflicts assigned as reference in extent field                      
            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed decrese counter 
                    card.Value = value;
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    /// update card have same key with new value 

                    return card;
                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    var newcard = createNew(key, value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                /// assign next conflict card to local card
                card = card.Extent;
            }
        }

        /// <summary>
        /// The Rehash.
        /// </summary>
        /// <param name="newsize">The newsize<see cref="int"/>.</param>
        protected override void Rehash(int newsize)
        {
            int finish = count;
            int _newsize = newsize; //+ (int)(newsize * REMOVED_PERCENT_LIMIT) + 10;
            uint newMaxId = (uint)(_newsize - 1);
            ICard<V>[] newCardTable = EmptyCardTable(_newsize);
            if (removed != 0)
            {
                ICard<V>[] newBaseDeck = EmptyBaseDeck(_newsize);
                rehashAndReindex(newCardTable, newBaseDeck, newMaxId);
                list = newBaseDeck;
            }
            else
            {
                ICard<V>[] newBaseDeck = EmptyBaseDeck(_newsize);
                rehash(newCardTable, newMaxId);
                Array.Copy(list, 0, newBaseDeck, 0, finish);
                list = newBaseDeck;
            }
            table = newCardTable;
            maxId = newMaxId;
            size = newsize;
        }

        /// <summary>
        /// The Reindex.
        /// </summary>
        protected virtual void Reindex()
        {
            ICard<V> card = null;
            int total = count + removed;
            int _counter = 0;
            for (int i = 0; i < total; i++)
            {
                card = list[i];
                if (card != null && !card.Removed)
                {
                    card.Index = _counter;
                    list[_counter++] = card;
                }

            }
            removed = 0;
        }

        /// <summary>
        /// The renewClear.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        protected override void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                maxId = (uint)(capacity - 1);
                conflicts = 0;
                removed = 0;
                count = 0;
                table = EmptyCardTable(size);
                list = EmptyBaseDeck(size);
                first = EmptyCard();
                last = first;
            }
        }

        /// <summary>
        /// The rehash.
        /// </summary>
        /// <param name="newCardTable">The newCardTable<see cref="ICard{V}[]"/>.</param>
        /// <param name="newMaxId">The newMaxId<see cref="uint"/>.</param>
        private void rehash(ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            int total = count + removed;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> card = null;
            ICard<V> mem = null;
            for (int i = 0; i < total; i++)
            {
                card = list[i];
                if (card != null && !card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);
                    mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        card.Extent = null;
                        _newCardTable[pos] = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extent == null)
                            {
                                card.Extent = null;
                                mem.Extent = card;
                                _conflicts++;
                                break;
                            }
                            else
                                mem = mem.Extent;
                        }
                    }
                }
            }
            conflicts = _conflicts;
        }

        /// <summary>
        /// The rehashAndReindex.
        /// </summary>
        /// <param name="newCardTable">The newCardTable<see cref="ICard{V}[]"/>.</param>
        /// <param name="newBaseDeck">The newBaseDeck<see cref="ICard{V}[]"/>.</param>
        /// <param name="newMaxId">The newMaxId<see cref="uint"/>.</param>
        private void rehashAndReindex(ICard<V>[] newCardTable, ICard<V>[] newBaseDeck, uint newMaxId)
        {
            int _conflicts = 0;
            int _counter = 0;
            int total = count + removed;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V>[] _newBaseDeck = newBaseDeck;
            ICard<V> card = null;
            ICard<V> mem = null;
            for (int i = 0; i < total; i++)
            {
                card = list[i];

                if (card != null && !card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);

                    mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        card.Extent = null;
                        card.Index = _counter;
                        _newCardTable[pos] = card;
                        _newBaseDeck[_counter++] = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extent == null)
                            {
                                card.Extent = null;
                                mem.Extent = card;
                                card.Index = _counter;
                                _newBaseDeck[_counter++] = card;
                                _conflicts++;
                                break;
                            }
                            else
                                mem = mem.Extent;
                        }
                    }
                }
            }
            conflicts = _conflicts;
            removed = 0;
        }

        /// <summary>
        /// The reindexWithInsert.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="item">The item<see cref="ICard{V}"/>.</param>
        private void reindexWithInsert(int index, ICard<V> item)
        {
            ICard<V> card = null;
            int _counter = 0;
            int total = count + removed;
            for (int i = 0; i < total; i++)
            {
                card = list[i];
                if (card != null && !card.Removed)
                {
                    card.Index = _counter;
                    list[_counter++] = card;
                    if (_counter == index)
                    {
                        item.Index = _counter;
                        list[_counter++] = item;
                    }
                }

            }
            removed = 0;
        }

        #endregion
    }
}
