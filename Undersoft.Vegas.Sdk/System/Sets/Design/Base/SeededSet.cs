using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Uniques;
using System.Extract;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Basedeck.KeyedDeck
              
    @author Darius Hanc                                                  
    @project: urcorlib                 
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Sets.Basedeck
{
    public abstract class SeededSet<V> : Uniqueness, ICollection<V>, IList<V>, IMassDeck<V>, ICollection<ICard<V>>, IList<ICard<V>>,
                                                      ICollection<IUnique<V>>, IProducerConsumerCollection<V>, IDisposable where V : IUnique
    {
        #region Globals       

        static protected readonly float RESIZING_VECTOR = 1.766f;
        static protected readonly float CONFLICTS_PERCENT_LIMIT = 0.22f;
        static protected readonly float REMOVED_PERCENT_LIMIT = 0.15f;

        protected ICard<V> first, last;
        protected ICard<V>[] table;
        protected int count, conflicts, removed, minSize, size;
        protected uint maxId;     
        
        protected int nextSize()
        {          
          // return PRIMES_ARRAY.Get(primesId++);
           return (((int)(size * RESIZING_VECTOR))^3); // Evaluate size without primes
        }

        protected int previousSize()
        {
            // return PRIMES_ARRAY.Get--primesId);
            return (int)(size * (1 - REMOVED_PERCENT_LIMIT))^3; // Evaluate size without primes
        }       

        protected void countIncrement()
        {
            if ((++count + 7) > size)               
                Rehash(nextSize());
            // Rehash((((size * 2)^3); // Evaluate size without primes
        }
        protected void conflictIncrement()
        { 
            countIncrement();           
            if (++conflicts > (size * CONFLICTS_PERCENT_LIMIT))                
               Rehash(nextSize());           
        }
        protected void removedIncrement()
        {
            --count;            
            if (++removed > ((size * REMOVED_PERCENT_LIMIT) - 1))
            {
                if (size < (size * 0.5))
                    Rehash(previousSize());
                else
                    Rehash(size);             
            }
        }
        protected void removedDecrement()
        {
            ++count;
            --removed;
        }

        #endregion

        #region Constructor

        public SeededSet(int capacity = 17, HashBits bits = HashBits.bit64) : base(bits)
        {
            size = capacity;
            minSize = capacity;
            maxId = (uint)(size - 1);
            table = EmptyCardTable(capacity);
            first = EmptyCard();
            last = first;
        }
        public SeededSet(IList<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            this.Add(collection);
        }
        public SeededSet(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public SeededSet(IEnumerable<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            this.Add(collection);
        }
        public SeededSet(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }

        #endregion

        #region Settings

        public virtual          ICard<V> First
        { get { return first; } }
        public virtual          ICard<V> Last
        { get { return last; } }

        public virtual              int Size{ get => size; }
        public virtual              int Count { get => count; }
        public virtual             bool IsReadOnly { get; set; }
        public virtual             bool IsSynchronized { get; set; }
        public virtual           object SyncRoot { get; set; }

        #endregion

        #region Operations

        #region Item

        ICard<V>        IList<ICard<V>>.this[int index]
        {
            get => GetCard(index);
            set => GetCard(index).Set(value);
        }
        public virtual              V this[int index]
        {
            get => GetCard(index).Value;
            set => GetCard(index).Value = value;
        }
        protected                   V this[ulong hashkey]
        {
            get { return InnerGet(hashkey); }
            set { InnerPut(hashkey, value); }
        }
        public virtual              V this[object key]
        {
            get
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    return InnerGet(unique.Key(ukey, ukey.UniqueSeed));
                }
                else
                    throw new NotSupportedException();
            }
            set
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    InnerPut(unique.Key(ukey, ukey.UniqueSeed), value);
                }
                else
                    throw new NotSupportedException();
            }
        }
        public virtual              V this[object key, ulong seed]
        {
            get { return InnerGet(unique.Key(key, seed)); }
            set { InnerPut(unique.Key(key, seed), value); }
        }
        public virtual              V this[IUnique key, ulong seed]
        {
            get { return InnerGet(unique.Key(key, seed)); }
            set { InnerPut(unique.Key(key, seed), value); }
        }

        #endregion

        #region Get

        protected virtual         V InnerGet(ulong key)
        {
            ICard<V> mem = table[getPosition(key)];

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
        public virtual            V Get(ulong key)
        {
            return InnerGet(key);
        }
        public virtual            V Get(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGet(unique.Key(ukey, ukey.UniqueSeed));
            }
            else
                throw new NotSupportedException();
        }
        public virtual            V Get(object key, ulong seed)
        {
            return InnerGet(unique.Key(key, seed));
        }
        public virtual            V Get(IUnique key)
        {
            return InnerGet(unique.Key(key, key.UniqueSeed));
        }
        public virtual            V Get(IUnique<V> key)
        {
            return InnerGet(unique.Key(key, key.UniqueSeed));
        }

        protected virtual      bool InnerTryGet(ulong key, out ICard<V> output)
        {
            output = null;
            ICard<V> mem = table[getPosition(key)];
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
        public virtual         bool TryGet(ulong key, out ICard<V> output)
        {
            return InnerTryGet(key, out output);
        }
        public virtual         bool TryGet(object key, out ICard<V> output)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerTryGet(unique.Key(ukey, ukey.UniqueSeed), out output);
            }
            else
                throw new NotSupportedException();           
        }
        public virtual         bool TryGet(object key, out V output)
        {
            if (key is IUnique)
            {
                output = default(V);
                ICard<V> card = null;
                IUnique ukey = (IUnique)key;
                if(InnerTryGet(unique.Key(ukey, ukey.UniqueSeed), out card))
                {
                    output = card.Value;
                    return true;
                }
                return false;
            }
            else
                throw new NotSupportedException();          
        }
        public virtual         bool TryGet(object key, ulong seed, out ICard<V> output)
        {
            return InnerTryGet(unique.Key(key, seed), out output);
        }
        public virtual         bool TryGet(object key, ulong seed, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(unique.Key(key, seed), out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }
        public virtual         bool TryGet(ulong key, out V output)
        {
            ICard<V> card;
            if(InnerTryGet(key, out card))
            {
                output = card.Value;
                return true;
            }
            output = default(V);
            return false;
        }

        protected virtual   ICard<V> InnerGetCard(ulong key)
        {
            ICard<V> mem = table[getPosition(key)];

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
        public virtual      ICard<V> GetCard(ulong key)
        {
            return             InnerGetCard(key);
        }
        public virtual      ICard<V> GetCard(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGetCard(unique.Key(ukey, ukey.UniqueSeed));
            }
            else
                throw new NotSupportedException();
        }
        public virtual      ICard<V> GetCard(object key, ulong seed)
        {
            return InnerGetCard(unique.Key(key, seed));
        }
        public abstract     ICard<V> GetCard(int index);

        #endregion

        #region Put

        protected virtual      ICard<V> InnerPut(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;
            return InnerPut(value);
        }
        protected abstract     ICard<V> InnerPut(ulong key, V value);
        protected virtual      ICard<V> InnerPut(V value, ulong seed)
        {
            value.UniqueSeed = seed;
            return InnerPut(value);
        }
        protected abstract     ICard<V> InnerPut(V value);
        protected abstract     ICard<V> InnerPut(ICard<V> value);

        public virtual              ICard<V> Put(ulong key, object value)
        {
            return InnerPut(key, (V)value);
        }
        public virtual              ICard<V> Put(ulong key, V value)
        {
            return InnerPut(key, value);
        }
        public virtual              ICard<V> Put(object key, V value)
        {
           return InnerPut(unique.Key(key, value.UniqueSeed), value);
        }
        public virtual              ICard<V> Put(object key, ulong seed, V value)
        {
            return InnerPut(unique.Key(key, seed), seed, value);
        }
        public virtual              ICard<V> Put(object key, ulong seed,  object value)
        {
            if (value is V)
            {
                V o = (V)value;
                return InnerPut(unique.Key(key, seed), seed, (V)value);
            }
            return null;
        }
        public virtual              ICard<V> Put(ICard<V> card)
        {
            return InnerPut(card);
        }
        public virtual                  void Put(IList<ICard<V>> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i]);
            }
        }
        public virtual                  void Put(IEnumerable<ICard<V>> cards)
        {
            foreach (ICard<V> card in cards)
                InnerPut(card);
        }
        public virtual              ICard<V> Put(V value)
        {
            return InnerPut(value);
        }
        public virtual                  void Put(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Put(cards[i]);
            }
        }
        public virtual                  void Put(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Put(card);
        }

        public virtual              ICard<V> Put(V value, ulong seed)
        {
           return InnerPut(value, seed);
        }
        public virtual                  void Put(object value, ulong seed)
        {
            if (value is IUnique<V>)
            {
                IUnique<V> v = (IUnique<V>)value;
                Put(v);
            }
            else if (value is V)
                Put((V)value, seed);
        }
        public virtual                  void Put(IList<V> cards, ulong seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i], seed);
            }
        }
        public virtual                  void Put(IEnumerable<V> cards, ulong seed)
        {
            foreach (V card in cards)
                InnerPut(card, seed);
        }
        public virtual              ICard<V> Put(IUnique<V> value)
        {
           return InnerPut(unique.Key(value, value.UniqueSeed), value.Value);
        }
        public virtual                  void Put(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        public virtual                  void Put(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        #endregion

        #region Add

        protected virtual  bool InnerAdd(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;            
            return InnerAdd(value);
        }
        protected abstract bool InnerAdd(ulong key, V value);
        protected virtual  bool InnerAdd(V value, ulong seed)
        {
            value.UniqueSeed = seed;
            return InnerAdd(value);
        }
        protected abstract bool InnerAdd(V value);
        protected abstract bool InnerAdd(ICard<V> value);
        public virtual          bool Add(ulong key, object value)
        {
            V o = (V)value;
            return InnerAdd(key, o.UniqueSeed, o);
        }
        public virtual          bool Add(ulong key, V value)
        {
            return InnerAdd(key, value);
        }
        public virtual          bool Add(object key, V value)
        {            
           return InnerAdd(unique.Key(key, value.UniqueSeed), value);            
        }
        public virtual          bool Add(object key, ulong seed, V value)
        {
            return InnerAdd(unique.Key(key, seed), seed, value);
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
        public virtual          bool Add(V value, ulong seed)
        {
           return InnerAdd(value, seed);
        }
        public virtual          void Add(IList<V> cards, ulong seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i], seed);

            }
        }
        public virtual          void Add(IEnumerable<V> cards, ulong seed)
        {
            foreach (V card in cards)
                Add(card, seed);
        }
        public virtual          void Add(IUnique<V> value)
        {
            InnerAdd(unique.Key(value, value.UniqueSeed), value.Value);
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
           return InnerAdd(value);
        }
        public virtual       bool TryAdd(V value, ulong seed)
        {
            return InnerAdd(value, seed);
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
        public virtual ICard<V> AddNew(object key, ulong seed)
        {
            ICard<V> newCard = NewCard(unique.Key(key, seed), default(V));
            if (InnerAdd(newCard))
                return newCard;
            return null;
        }

        protected abstract void InnerInsert(int index, ICard<V> item);
        public virtual          void Insert(int index, ICard<V> item)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            ulong key = item.Key;
            ulong pos = getPosition(key);

            ICard<V> card = table[pos]; /// local for last removed item finded   
            // add in case when item doesn't exist and there is no conflict                                                      
            if (card == null)
            {
                card = NewCard(item);
                table[pos] = card;
                InnerInsert(index, card);
                countIncrement();
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
                        conflictIncrement();
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
                    conflictIncrement();
                    return;
                }
                card = card.Extent;
            }
        }
        public virtual          void Insert(int index, V item)
        {
            Insert(index, NewCard(item));
        }

        #endregion

        #region Queue

        public virtual       bool Enqueue(V value)
        {
            return TryAdd(value);
        }
        public virtual       bool Enqueue(object key, V value)
        {
            return Add(key, value);
        }
        public virtual       bool Enqueue(V value, ulong seed)
        {
            return InnerAdd(value, seed);
        }
        public virtual       bool Enqueue(object key, ulong seed, V value)
        {
            return Add(key, seed, value);
        }
        public virtual       void Enqueue(ICard<V> card)
        {
            InnerAdd(card);
        }

        public virtual       bool TryTake(out V output)
        {
            return TryDequeue(out output);
        }
        public virtual          V Dequeue()
        {
            V card = default(V);
            TryDequeue(out card);
            return card;
        }

        public virtual       bool TryDequeue(out V output)
        {
            var _output = Next(first);
            if (_output != null)
            {
                _output.Removed = true;
                removedIncrement();
                output = _output.Value;
                return true;
            }
            output = default(V);
            return false;
        }
        public virtual       bool TryDequeue(out ICard<V> output)
        {
            output = Next(first);
            if (output != null)
            {
                output.Removed = true;
                removedIncrement();
                return true;
            }
            return false;
        }
        #endregion

        #region Renew
        protected virtual void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                maxId = (uint)(capacity - 1);
                conflicts = 0;
                removed = 0;
                count = 0;
                table = EmptyCardTable(size);
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

        protected bool      InnerContainsKey(ulong key)
        {
            ICard<V> mem = table[getPosition(key)];

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
        public virtual      bool ContainsKey(object key)
        {
            return InnerContainsKey(unique.Key(key));
        }
        public virtual      bool ContainsKey(object key, ulong seed)
        {
            return InnerContainsKey(unique.Key(key, seed));
        }
        public virtual      bool ContainsKey(ulong key)
        {
            return InnerContainsKey(key);
        }
        public virtual      bool ContainsKey(IUnique key)
        {
            return InnerContainsKey(unique.Key(key));
        }

        public virtual       bool Contains(ICard<V> item)
        {
            return InnerContainsKey(item.Key);
        }
        public virtual       bool Contains(IUnique<V> item)
        {
            return InnerContainsKey(unique.Key(item, item.UniqueSeed));
        }
        public virtual       bool Contains(V item)
        {
            return InnerContainsKey(unique.Key(item));
        }
        public virtual       bool Contains(V item, ulong seed)
        {
            return InnerContainsKey(unique.Key(item, seed));
        }

        #endregion

        #region Remove

        protected virtual V InnerRemove(ulong key)
        {
            ICard<V> mem = table[getPosition(key)];

            while (mem != null)
            {
                if (mem.Equals(key))
                {
                    if (!mem.Removed)
                    {
                        mem.Removed = true;
                        removedIncrement();
                        return mem.Value;
                    }
                    return default(V);
                }

                mem = mem.Extent;
            }
            return default(V);
        }
        public virtual      bool Remove(V item)
        {
            return InnerRemove(unique.Key(item)).Equals(default(V)) ? false : true;
        }
        public virtual         V Remove(object key)
        {
            return InnerRemove(unique.Key(key));
        }
        public virtual         V Remove(object key, ulong seed)
        {
            return InnerRemove(unique.Key(key, seed));
        }
        public virtual      bool Remove(ICard<V> item)
        {
            return InnerRemove(item.Key).Equals(default(V)) ? false : true;
        }
        public virtual      bool Remove(IUnique<V> item)
        {
            return TryRemove(unique.Key(item, item.UniqueSeed));
        }
        public virtual   bool TryRemove(object key)
        {
            return InnerRemove(unique.Key(key)).Equals(default(V)) ? false : true;
        }
        public virtual   bool TryRemove(object key, ulong seed)
        {
            return InnerRemove(unique.Key(key, seed)).Equals(default(V)) ? false : true;
        }
        public virtual    void RemoveAt(int index)
        {
            InnerRemove(GetCard(index).Key);
        }

        public virtual      void Clear()
        {
            size = minSize;
            maxId = (uint)(size - 1);
            conflicts = 0;
            removed = 0;
            count = 0;
            table = EmptyCardTable(size);
            first = EmptyCard();
            last = first;
        }

        public virtual      void Flush()
        {
            conflicts = 0;
            removed = 0;
            count = 0;
            table = null;
            table = EmptyCardTable(size);
            first = EmptyCard();
            last = first;
        }

        #endregion

        #region Collection     

        public virtual          void CopyTo(ICard<V>[] array, int index)
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
        public virtual          void CopyTo(IUnique<V>[] array, int arrayIndex)
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
        public virtual          void CopyTo(Array array, int index)
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
        public virtual          void CopyTo(V[] array, int index)
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
        public virtual           V[] ToArray()
        {
            return this.AsValues().ToArray();
        }
        public virtual      object[] ToObjectArray()
        {
            return this.AsValues().Select((x) => (object)x).ToArray();
        }

        public virtual      ICard<V> Next(ICard<V> card)
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

        public virtual          void Resize(int size)
        {
            Rehash(size);
        }

        public abstract     ICard<V> EmptyCard();

        public virtual       ICard<V> NewCard(ulong key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            value.UniqueKey = key;
            return NewCard(value);
        }
        public abstract      ICard<V> NewCard(ulong key, V value);
        public abstract      ICard<V> NewCard(object key, V value);
        public virtual       ICard<V> NewCard(object key, ulong seed, V value)
        {
            value.UniqueSeed = seed;
            return NewCard(unique.Key(key, seed), value);
        }
        public abstract      ICard<V> NewCard(ICard<V> card);
        public virtual       ICard<V> NewCard(V card, ulong seed)
        {
            card.UniqueSeed = seed;
            return NewCard(card);
        }
        public abstract      ICard<V> NewCard(V card);

        public abstract   ICard<V>[] EmptyCardTable(int size);

        public virtual         int IndexOf(ICard<V> item)
        {
            return GetCard(item).Index;
        }
        public virtual         int IndexOf(V item)
        {
            return GetCard(item).Index;
        }

        #endregion

        #endregion

        #region Enumerable

        public virtual               IEnumerable<V> AsValues()
        {
            return (IEnumerable<V>)this;
        }

        public virtual         IEnumerable<ICard<V>> AsCards()
        {
            return (IEnumerable<ICard<V>>)this;
        }

        public virtual      IEnumerable<IUnique<V>> AsIdentifiers()
        {
            return (IEnumerable<IUnique<V>>)this;
        }

        public virtual         IEnumerator<ICard<V>> GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        public virtual            IEnumerator<ulong> GetKeyEnumerator()
        {
            return new CardUniqueKeySeries<V>(this);
        }

        IEnumerator<V>               IEnumerable<V>.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        IEnumerator<IUnique<V>>
                            IEnumerable<IUnique<V>>.GetEnumerator()
        {
            return new CardKeySeries<V>(this);
        }      

        IEnumerator                     IEnumerable.GetEnumerator()
        {
            return new CardSeries<V>(this);
        }

        #endregion

        #region Hashtable

        protected ulong        getPosition(ulong key)
        {
            // standard hashmap method to establish position / index in table

            return (key % maxId);

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type) 
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

            // return Submix.Map(key, size - 1, mixMask, msbId);           
        }
        protected static ulong getPosition(ulong key, uint tableMaxId)
        {
            // standard hashmap method to establish position / index in table 

             return (key % tableMaxId);

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type)
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

            //return Submix.Map(key, newsize - 1, newMixMask, newMsbId);                    
        }

        protected virtual  void Rehash(int newSize)
        {
            int finish = count;
            int newsize = newSize;
            uint newMaxId = (uint)(newsize - 1);
            ICard<V>[] newCardTable = EmptyCardTable(newsize);
            ICard<V> card = first;
            card = card.Next;
            if (removed > 0)
            {
                rehashAndReindex(card, newCardTable, newMaxId);
            }
            else
            {
                rehash(card, newCardTable, newMaxId);
            }

            table = newCardTable;
            maxId = newMaxId;
            size = newsize;
        }

        private         void rehashAndReindex(ICard<V> card, ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);

                    ICard<V> mem = _newCardTable[pos];

                    if (mem == null)
                    {
                        card.Extent = null;
                        _newCardTable[pos] = _lastcard = _lastcard.Next = card;
                    }
                    else
                    {
                        for (; ; )
                        {
                            if (mem.Extent == null)
                            {
                                card.Extent = null;;
                                _lastcard = _lastcard.Next = mem.Extent = card;
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

            conflicts = _conflicts;
            removed = 0;
            first = _firstcard;
            last = _lastcard;
        }

        private         void rehash(ICard<V> card, ICard<V>[] newCardTable, uint newMaxId)
        {
            int _conflicts = 0;
            uint _newMaxId = newMaxId;
            ICard<V>[] _newCardTable = newCardTable;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, _newMaxId);

                    ICard<V> mem = _newCardTable[pos];

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

                card = card.Next;

            } while (card != null);
            conflicts = _conflicts;
        }

        #region IDisposable Support
        protected bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {                 
                    first = null;
                    last = null;
                }
                table = null;

                disposedValue = true;
            }
        }

         //~KeyedDeck() {
         //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         //  Dispose(false);
         //}

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

    }  
}
