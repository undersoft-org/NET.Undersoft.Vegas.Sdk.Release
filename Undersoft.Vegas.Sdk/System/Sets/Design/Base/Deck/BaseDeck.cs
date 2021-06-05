/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.BaseDeck.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    using System.Collections.Generic;
    using System.Sets.Basedeck;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="BaseDeck{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public abstract class BaseDeck<V> : KeyedSet<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseDeck(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseDeck(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseDeck(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseDeck(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseDeck(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

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

                int i = -1;
                int id = index;
                var card = first.Next;
                for (; ; )
                {
                    if (++i == id)
                        return card;
                    card = card.Next;
                }
            }
            return null;
        }

        /// <summary>
        /// The createNew.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected ICard<V> createNew(ICard<V> value)
        {
            last.Next = value;
            last = value;
            return value;
        }

        /// <summary>
        /// The createNew.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected ICard<V> createNew(ulong key, V value)
        {
            var newcard = NewCard(key, value);
            last.Next = newcard;
            last = newcard;
            return newcard;
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
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
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
                    card.Extent = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
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
            ulong key = unique.Key(value);
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
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
                    card.Extent = createNew(key, value);
                    conflictIncrement();
                    return true;
                }
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
            if (index < count - 1)
            {
                if (index == 0)
                {
                    item.Index = 0;
                    item.Next = first.Next;
                    first.Next = item;
                }
                else
                {

                    ICard<V> prev = GetCard(index - 1);
                    ICard<V> next = prev.Next;
                    prev.Next = item;
                    item.Next = next;
                    item.Index = index;
                }
            }
            else
            {
                last = last.Next = item;
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
                    /// when card was removed decrese counter 
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    /// update card have same key with new value 
                    card.Value = value.Value;
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
            // get position index in table using own native alghoritm - submix                             
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; // get item by position   
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
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    /// update card have same key with new value 
                    card.Value = value;
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
            ulong key = unique.Key(value);
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
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
                    if (card.Removed)
                    {
                        card.Removed = false;
                        removedDecrement();
                    }
                    /// update card have same key with new value 
                    card.Value = value;
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
                card = card.Extent;
            }
        }

        /// <summary>
        /// The Reindex.
        /// </summary>
        protected virtual void Reindex()
        {
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            ICard<V> card = first.Next;
            do
            {
                if (!card.Removed)
                {
                    _lastcard = _lastcard.Next = card;
                }

                card = card.Next;

            } while (card != null);
            removed = 0;
            first = _firstcard;
            last = _lastcard;
        }

        #endregion
    }
}
