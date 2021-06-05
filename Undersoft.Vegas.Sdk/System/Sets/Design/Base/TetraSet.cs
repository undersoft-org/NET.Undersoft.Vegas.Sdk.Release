using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Uniques;
using System.Extract;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Basedeck.TetraSet
              
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                      
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Sets.Basedeck
{
    public abstract class TetraSet<V> : Uniqueness, ICollection<V>, IList<V>, IDeck<V>, ICollection<ICard<V>>, IList<ICard<V>>,
                                                     IProducerConsumerCollection<V>, IDisposable, ICollection<IUnique<V>>
    {
        #region Globals       

        static protected readonly float CONFLICTS_PERCENT_LIMIT = 0.25f;
        static protected readonly float REMOVED_PERCENT_LIMIT = 0.15f;
        //static protected readonly ulong MAX_BIT_MASK = 0xFFFFFFFFFFFFFFFF;      

        protected ICard<V> first, last;
        protected TetraTable<V> table;
        protected TetraSize tsize;
        protected TetraCount tcount;
        //protected int[] msbId;
        //protected uulong[] mixMask;
        protected int count, conflicts, removed, size, minSize;     

        protected void countIncrement(uint tid)
        {
            count++;
            if ((tcount.Increment(tid) + 3) > size)
                Rehash(tsize.NextSize(tid), tid);
        }
        protected void conflictIncrement(uint tid)
        {
            countIncrement(tid);
            if (++conflicts > (size * CONFLICTS_PERCENT_LIMIT))
                Rehash(tsize.NextSize(tid), tid);
        }
        protected void removedIncrement(uint tid)
        {
            int _tsize = tsize[tid];
            --count;
            tcount.Decrement(tid);
            if (++removed > (_tsize * REMOVED_PERCENT_LIMIT))
            {
                if (_tsize < _tsize / 2)
                    Rehash(tsize.PreviousSize(tid), tid);
                else
                    Rehash(_tsize, tid);
            }
        }
        protected void removedDecrement()
        {
            ++count;
            --removed;
        }

        #endregion

        #region Constructor

        public TetraSet(int capacity = 16, HashBits bits = HashBits.bit64) : base(bits)
        {
            size = capacity;
            minSize = capacity;
            tsize = new TetraSize(capacity);
            tcount = new TetraCount();
            table = new TetraTable<V>(this, capacity);
            first = EmptyCard();
            last = first;
        }
        public TetraSet(IList<ICard<V>> collection, int capacity = 16, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            this.Add(collection);
        }
        public TetraSet(IEnumerable<ICard<V>> collection, int capacity = 16, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            this.Add(collection);
        }

        #endregion

        #region Settings

        public virtual ICard<V> First { get { return first; } }
        public virtual ICard<V> Last { get { return last; } }

        public virtual int Size { get { return size; } }
        public virtual int Count { get => count; }
        public virtual bool IsReadOnly { get; set; }
        public virtual bool IsSynchronized { get; set; }
        public virtual object SyncRoot { get; set; }

        #endregion

        #region Operations

        #region Item

        ICard<V> IList<ICard<V>>.this[int index]
        {
            get => GetCard(index);
            set => GetCard(index).Set(value);
        }
        public virtual         V this[int index]
        {
            get => GetCard(index).Value;
            set => GetCard(index).Value = value;
        }
        protected              V this[ulong hashkey]
        {
            get { return InnerGet(hashkey); }
            set { InnerPut(hashkey, value); }
        }
        public virtual         V this[object key]
        {
            get { return InnerGet(unique.Key(key)); }
            set { InnerPut(unique.Key(key), value); }
        }

        #endregion

        #region Get

        public virtual             V InnerGet(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                        return mem.Value;
                    return default(V);
                }
                mem = mem.Extent;
            }

            return default(V);
        }
        public                          V Get(ulong key)
        {
            return InnerGet(key);
        }
        public virtual                  V Get(object key)
        {
            return InnerGet(unique.Key(key));
        }
        public virtual                  V Get(IUnique<V> key)
        {
            return InnerGet(unique.Key(key));
        }
        public virtual                  V Get(IUnique key)
        {
            return InnerGet(unique.Key(key));
        }

        public virtual       bool InnerTryGet(ulong key, out ICard<V> output)
        {
            output = null;
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];
            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                    {
                        output = mem;
                        return true;
                    }
                    return false;
                }
                mem = mem.Extent;
            }
            return false;
        }
        public virtual            bool TryGet(object key, out ICard<V> output)
        {
            return InnerTryGet(unique.Key(key), out output);
        }
        public virtual            bool TryGet(object key, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(unique.Key(key), out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }
        public                    bool TryGet(ulong key, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(key, out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }

        public virtual  ICard<V> InnerGetCard(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];
            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                        return mem;
                    return null;
                }
                mem = mem.Extent;
            }

            return null;
        }
        public abstract      ICard<V> GetCard(int index);
        public virtual       ICard<V> GetCard(object key)
        {
            return InnerGetCard(unique.Key(key));
        }       
        public               ICard<V> GetCard(ulong key)
        {
            return InnerGetCard(key);
        }

        #endregion

        #region Put

        protected abstract ICard<V> InnerPut(ulong key, V value);
        protected abstract ICard<V> InnerPut(V value);
        protected abstract ICard<V> InnerPut(ICard<V> value);
        public virtual          ICard<V> Put(ulong key, object value)
        {
            return InnerPut(key, (V)value);
        }
        public virtual          ICard<V> Put(ulong key, V value)
        {
            return InnerPut(key, value);
        }
        public virtual          ICard<V> Put(object key, V value)
        {
            return InnerPut(unique.Key(key), value);
        }
        public virtual          ICard<V> Put(object key, object value)
        {
            if (value is V)
                return InnerPut(unique.Key(key), (V)value);
            return null;
        }
        public virtual          ICard<V> Put(ICard<V> _card)
        {
            return InnerPut(_card);
        }
        public virtual              void Put(IList<ICard<V>> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i]);
            }
        }
        public virtual              void Put(IEnumerable<ICard<V>> cards)
        {
            foreach (ICard<V> card in cards)
                InnerPut(card);
        }
        public virtual          ICard<V> Put(V value)
        {
           return InnerPut(value);
        }
        public virtual              void Put(object value)
        {
            if (value is IUnique<V>)
                Put((IUnique<V>)value);
            if (value is V)
                Put((V)value);
            else if (value is ICard<V>)
                Put((ICard<V>)value);
        }
        public virtual              void Put(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Put(cards[i]);

            }
        }
        public virtual              void Put(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Put(card);
        }
        public virtual          ICard<V> Put(IUnique<V> value)
        {
            if (value is ICard<V>)
               return InnerPut((ICard<V>)value);
           return InnerPut(value.UniquesAsKey(), value.Value);
        }
        public virtual              void Put(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        public virtual              void Put(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }

        #endregion

        #region Add

        protected abstract bool InnerAdd(ulong key, V value);
        protected abstract bool InnerAdd(V value);
        protected abstract bool InnerAdd(ICard<V> value);
        public virtual          bool Add(ulong key, object value)
        {
            return InnerAdd(key, (V)value);
        }
        public virtual          bool Add(ulong key, V value)
        {
            return InnerAdd(key, value);
        }
        public virtual          bool Add(object key, V value)
        {
            return InnerAdd(unique.Key(key), value);
        }
        public virtual          void Add(ICard<V> card)
        {
            InnerAdd(card);
        }
        public virtual          void Add(IList<ICard<V>> cardList)
        {
            int c = cardList.Count;
            for (int i = 0; i < c; i++)
            {
                InnerAdd(cardList[i]);
            }
        }
        public virtual          void Add(IEnumerable<ICard<V>> cardTable)
        {
            foreach (ICard<V> card in cardTable)
                Add(card);
        }
        public virtual          void Add(V value)
        {
            InnerAdd(value);
            //ulong key = unique.Key(value);
            //if (key == 0)
            //    key = autoHashKey(value);
            //InnerAdd(key, value);
        }
        public virtual          void Add(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i]);

            }
        }
        public virtual          void Add(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Add(card);
        }
        public virtual          void Add(IUnique<V> value)
        {
            if (value is ICard<V>)
                InnerAdd((ICard<V>)value);
            InnerAdd(unique.Key(value), value.Value);
        }
        public virtual          void Add(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        public virtual          void Add(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        public virtual       bool TryAdd(V value)
        {
            //ulong key = unique.Key(value);
            //if (key == 0)
            //    key = autoHashKey(value);            
            //return InnerAdd(key, value); ;

            return InnerAdd(value);
        }

        public virtual ICard<V> AddNew()
        {
            ICard<V> newCard = NewCard(Unique.NewKey, default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        public virtual ICard<V> AddNew(ulong key)
        {
            ICard<V> newCard = NewCard(key, default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }
        public virtual ICard<V> AddNew(object key)
        {
            ICard<V> newCard = NewCard(unique.Key(key), default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }

        protected abstract void InnerInsert(int index, ICard<V> item);
        public virtual          void Insert(int index, ICard<V> item)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong key = item.Key;
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            ulong pos = (ulong)getPosition(key);
            var _table = table[tid];
            ICard<V> card = _table[pos];
            if (card == null)
            {
                card = NewCard(item);
                _table[pos] = card;
                InnerInsert(index, card);
                countIncrement(tid);
                return;
            }

            for (; ; )
            {
                /// key check
                if (card.Equals(key))
                {
                    /// when card was removed insert 
                    if (card.Removed)
                    {
                        var newcard = NewCard(item);
                        card.Extent = newcard;
                        InnerInsert(index, newcard);
                        conflictIncrement(tid);
                        return;
                    }
                    throw new Exception("Item exist");

                }
                /// check that all conflicts was examinated and local card is the last one  
                if (card.Extent == null)
                {
                    var newcard = NewCard(item);
                    card.Extent = newcard;
                    InnerInsert(index, newcard);
                    conflictIncrement(tid);
                    return;
                }
                card = card.Extent;
            }
        }
        public virtual          void Insert(int index, V item)
        {
            Insert(index, NewCard(unique.Key(item), item));
        }

        #endregion

        #region Queue

        public virtual bool Enqueue(V value)
        {
            return InnerAdd(unique.Key(value), value);
        }
        public virtual bool Enqueue(object key, V value)
        {
            return InnerAdd(unique.Key(key), value);
        }
        public virtual void Enqueue(ICard<V> card)
        {
            InnerAdd(card.Key, card.Value);
        }

        public virtual bool TryTake(out V output)
        {
            return TryDequeue(out output);
        }
        public virtual    V Dequeue()
        {
            V card = default(V);
            TryDequeue(out card);
            return card;
        }

        public virtual bool TryDequeue(out V output)
        {
            var _output = Next(first);
            if (_output != null)
            {
                _output.Removed = true;
                removedIncrement(getTetraId(_output.Key));
                output = _output.Value;
                return true;
            }
            output = default(V);
            return false;
        }
        public virtual bool TryDequeue(out ICard<V> output)
        {
            output = Next(first);
            if (output != null)
            {
                output.Removed = true;
                removedIncrement(getTetraId(output.Key));
                return true;
            }
            return false;
        }
        #endregion

        #region Renew

        private void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                conflicts = 0;
                removed = 0;
                count = 0;
                tcount.ResetAll();
                tsize.ResetAll();
                table = new TetraTable<V>(this, size);
                first = EmptyCard();
                last = first;              
            }
        }

        public virtual void Renew(IEnumerable<V> cards)
        {
            renewClear(minSize);
            Put(cards);
        }
        public virtual void Renew(IList<V> cards)
        {
            int capacity = cards.Count;
            capacity += (int)(capacity * CONFLICTS_PERCENT_LIMIT);
            renewClear(capacity);
            Put(cards);
        }
        public virtual void Renew(IList<ICard<V>> cards)
        {
            int capacity = cards.Count;
            capacity += (int)(capacity * CONFLICTS_PERCENT_LIMIT);
            renewClear(capacity);
            Put(cards);
        }
        public virtual void Renew(IEnumerable<ICard<V>> cards)
        {
            renewClear(minSize);
            Put(cards);
        }
        
        #endregion

        #region Contains

        protected bool InnerContainsKey(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (!mem.Removed && mem.Equals(key))
                {

                    return true;
                }
                mem = mem.Extent;
            }

            return false;
        }
        public virtual bool ContainsKey(object key)
        {
            return InnerContainsKey(unique.Key(key));
        }
        public virtual bool ContainsKey(ulong key)
        {
            return InnerContainsKey(key);
        }
        public virtual bool ContainsKey(IUnique key)
        {
            return InnerContainsKey(unique.Key(key));
        }

        public virtual bool Contains(ICard<V> item)
        {
            return InnerContainsKey(item.Key);
        }
        public virtual bool Contains(IUnique<V> item)
        {
            return InnerContainsKey(unique.Key(item));
        }
        public virtual bool Contains(V item)
        {
            return InnerContainsKey(unique.Key(item));
        }

        #endregion

        #region Remove

        public virtual    V InnerRemove(ulong key)
        {
            uint tid = getTetraId(key);
            int _size = tsize[tid];
            uint pos = (uint)getPosition(key);

            ICard<V> mem = table[tid, pos];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                    {
                        mem.Removed = true;
                        removedIncrement(getTetraId(mem.Key));
                        return mem.Value;
                    }
                    return default(V);
                }

                mem = mem.Extent;
            }
            return default(V);
        }
        public virtual bool Remove(V item)
        {
            return InnerRemove(unique.Key(item)).Equals(default(V)) ? false : true;
        }
        public virtual    V Remove(object key)
        {
            return InnerRemove(unique.Key(key));
        }
        public virtual bool Remove(ICard<V> item)
        {
            return InnerRemove(item.Key).Equals(default(V)) ? false : true;
        }
        public virtual bool Remove(IUnique<V> item)
        {
            return TryRemove(unique.Key(item));
        }
        public virtual bool TryRemove(object key)
        {
            return InnerRemove(unique.Key(key)).Equals(default(V)) ? false : true;
        }
        public virtual void RemoveAt(int index)
        {
            InnerRemove(GetCard(index).Key);
        }

        public virtual void Clear()
        {
            size = minSize;
            conflicts = 0;
            removed = 0;
            count = 0;
            tcount.ResetAll();
            tsize.ResetAll();
            table = new TetraTable<V>(this, size);
            first = EmptyCard();
            last = first;
        }

        public virtual void Flush()
        {
            conflicts = 0;
            removed = 0;
            count = 0;
            tcount.ResetAll();
            tsize.ResetAll();
            table = new TetraTable<V>(this, size);
            first = EmptyCard();
            last = first;         
        }

        #endregion

        #region Collection     

        public virtual void CopyTo(ICard<V>[] array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (ICard<V> ves in this.AsCards().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (ICard<V> ves in this)
                    array[i++] = ves;
        }
        public virtual void CopyTo(Array array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (V ves in this.AsValues().Take(c))
                    array.SetValue(ves, i++);
            }
            else
                foreach (V ves in this.AsValues())
                    array.SetValue(ves, i++);
        }
        public virtual void CopyTo(V[] array, int index)
        {
            int c = count, i = index, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (V ves in this.AsValues().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (V ves in this.AsValues())
                    array[i++] = ves;
        }
        public virtual void CopyTo(IUnique<V>[] array, int arrayIndex)
        {
            int c = count, i = arrayIndex, l = array.Length;
            if (l - i < c)
            {
                c = l - i;
                foreach (ICard<V> ves in this.AsCards().Take(c))
                    array[i++] = ves;
            }
            else
                foreach (ICard<V> ves in this)
                    array[i++] = ves;
        }
        public virtual V[] ToArray()
        {
            return this.AsValues().ToArray();
        }

        public virtual ICard<V> Next(ICard<V> card)
        {
            ICard<V> _card = card.Next;
            if (_card != null)
            {
                if (!_card.Removed)
                    return _card;
                return Next(_card);
            }
            return null;
        }

        public virtual void Resize(int size, uint tid)
        {
            Rehash(size, tid);
        }

        public abstract ICard<V> EmptyCard();

        public abstract ICard<V> NewCard(ulong key, V value);
        public abstract ICard<V> NewCard(object key, V value);
        public abstract ICard<V> NewCard(ICard<V> card);
        public abstract ICard<V> NewCard(V card);

        public abstract ICard<V>[] EmptyCardTable(int size);

        public virtual int IndexOf(ICard<V> item)
        {
            return GetCard(item).Index;
        }
        public virtual int IndexOf(V item)
        {
            return GetCard(item).Index;
        }

        #endregion

        #endregion

        #region Enumerable

        public virtual IEnumerable<V> AsValues()
        {
            return (IEnumerable<V>)this;
        }

        public virtual IEnumerable<ICard<V>> AsCards()
        {
            return (IEnumerable<ICard<V>>)this;
        }

        public virtual IEnumerable<IUnique<V>> AsIdentifiers()
        {
            return (IEnumerable<IUnique<V>>)this;
        }

        public virtual IEnumerator<ICard<V>> GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        IEnumerator<V> IEnumerable<V>.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        IEnumerator<IUnique<V>>
                        IEnumerable<IUnique<V>>.GetEnumerator()
        {
            return new CardKeySeries<V>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        #endregion

        #region Hashtable

        protected static uint  getTetraId(ulong key)
        {
            return (uint)(((long)key & 1L) - (((long)key & -1L) * 2));
        }
        protected ulong getPosition(ulong key)//, int tsize, ulong tid)
        {
            // standard hashmap method to establish position / index in table

            return (key % (uint)(size -1));

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type) 
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

            //return Submix.Map(key, tsize - 1, mixMask[tid], msbId[tid]);
        }
        protected static ulong getPosition(ulong key, int newsize)// ulong newMixMask, int newMsbId)
        {
            // standard hashmap method to establish position / index in table 

            return (key % (uint)(newsize - 1));

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type)
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

          //  return Submix.Map(key, newsize - 1, newMixMask, newMsbId);
        }

        protected virtual void Rehash(int newsize, uint tid)
        {
            int finish = tcount[tid];
            int _tsize = tsize[tid];
            ICard<V>[] newCardTable = EmptyCardTable(newsize);
            ICard<V> card = first;
            card = card.Next;
            if (removed > 0)
            {
                rehashAndReindex(card, newCardTable, newsize, tid);
            }
            else
            {
                rehashOnly(card, newCardTable, newsize, tid);
            }

            table[tid] = newCardTable;
            size = newsize - _tsize;

        }

        private void rehashAndReindex(ICard<V> card, ICard<V>[] newCardTable, int newsize, uint tid)
        {
            int _conflicts = 0;
            int _oldconflicts = 0;
            int _removed = 0;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize);//, newMixMask, newMsbId);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        if (card.Extent != null)
                            _oldconflicts++;

                        card.Extent = null;
                        _newCardTable[pos] = _lastcard = _lastcard.Next = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extent == null)
                            {
                                card.Extent = null; ;
                                _lastcard = _lastcard.Next = mem.Extent = card;
                                _conflicts++;
                                break;
                            }
                            else
                                mem = mem.Extent;
                        }
                    }
                }
                else
                    _removed++;

                card = card.Next;

            } while (card != null);
            conflicts -= _oldconflicts;// _conflicts;
            removed -= _removed;
            first = _firstcard;
            last = _lastcard;
        }

        private void rehashOnly(ICard<V> card, ICard<V>[] newCardTable, int newsize, uint tid)
        {
            int _conflicts = 0;
            int _oldconflicts = 0;
            ICard<V>[] _newCardTable = newCardTable;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize);//, newMixMask, newMsbId);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        if (card.Extent != null)
                            _oldconflicts++;

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

                card = card.Next;

            } while (card != null);
            conflicts -= _oldconflicts;// _conflicts;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    first = null;
                    last = null;                                        
                }

                table.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TetraSet() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

    }

}
