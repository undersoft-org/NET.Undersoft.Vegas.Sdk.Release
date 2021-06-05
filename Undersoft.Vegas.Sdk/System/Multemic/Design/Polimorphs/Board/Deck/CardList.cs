﻿using System.Collections.Generic;
using System.Uniques;
using System.Multemic.Basedeck;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Deck    
        
    @author Darius Hanc                                                  
    @project  NETStandard.Undersoft.Library                               
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Multemic
{
    public abstract class CardList<V> : Hashdeck<V>
    {
        #region Globals       

        protected      ICard<V> addNew(long key, V value)
        {
            var newcard = NewCard(key, value);
            last.Next = newcard;
            last = newcard;
            return newcard;
        }
        protected      ICard<V> addNew(ICard<V> value)
        {
            last.Next = value;
            last = value;
            return value;
        }

        #endregion

        #region Constructor

        public CardList(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }
        public CardList(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public CardList(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public CardList(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public CardList(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion

        #region Operations

        public override    ICard<V> GetCard(int index)
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

        protected override ICard<V> InnerPut(long key, V value)
        {
            // get position index in table using own native alghoritm - submix                             
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; // get item by position   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = addNew(key, value);
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
                    var newcard = addNew(key, value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                card = card.Extent;
            }
        }
        protected override ICard<V> InnerPut(V value)
        {
            long key = base.GetHashKey(value);
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = addNew(key, value);
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
                    var newcard = addNew(key, value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                card = card.Extent;
            }
        }
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            long key = value.Key;
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = addNew(value);
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
                    var newcard = addNew(value);
                    card.Extent = newcard;
                    conflictIncrement();
                    return newcard;
                }
                card = card.Extent;
            }
        }

        protected override    bool InnerAdd(long key, V value)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = addNew(key, value);
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
                    card.Extent = addNew(key, value);
                    conflictIncrement();
                    return true;
                }
                card = card.Extent;
            }
        }
        protected override    bool InnerAdd(V value)
        {
            long key = base.GetHashKey(value);
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = addNew(key, value);
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
                    card.Extent = addNew(key, value);
                    conflictIncrement();
                    return true;
                }
                card = card.Extent;
            }
        }
        protected override    bool InnerAdd(ICard<V> value)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size  
            long key = value.Key;
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                table[pos] = addNew(value);
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
                    card.Extent = addNew(value);
                    conflictIncrement();
                    return true;
                }
                card = card.Extent;
            }
        }

        protected override    void InnerInsert(int index, ICard<V> item)
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

        protected virtual     void Reindex()
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
