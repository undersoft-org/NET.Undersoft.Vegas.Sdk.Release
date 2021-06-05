using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Uniques;
using System.Extract;

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Basedeck.Hashdeck
              
    @author Darius Hanc                                                  
    @project: urcorlib                 
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Multemic.Basedeck
{
    public abstract class Seeddeck<V> : HashKey, ICollection<V>, IList<V>, IMultiDeck<V>, ICollection<ICard<V>>, IList<ICard<V>>,
                                                 ICollection<IUnique<V>>, IProducerConsumerCollection<V>, IDisposable where V : IUnique
    {
        #region Globals       

        static protected readonly float RESIZING_VECTOR = 1.766f;
        static protected readonly float CONFLICTS_PERCENT_LIMIT = 0.22f;
        static protected readonly float REMOVED_PERCENT_LIMIT = 0.15f;
        static protected readonly ulong MAX_BIT_MASK = 0xFFFFFFFFFFFFFFFF;
        static protected readonly uint  AUTO_KEY_SEED = (uint)DateTime.Now.ToBinary().GetHashKey32();

        protected ICard<V> first, last;
        protected ICard<V>[] table;
        protected int count, conflicts, removed, minSize, size, msbId;
        protected ulong mixMask;      
        
        protected int nextSize()
        {          
          // return SIZE_PRIMES.Table[primesId++];
           return (((int)(size * RESIZING_VECTOR))^3); // Evaluate size without primes
        }

        protected int previousSize()
        {
          // return SIZE_PRIMES.Table[--primesId];
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

        protected long autoHashKey()
        {
            return base.GetHashKey(DateTime.Now.Ticks.GetBytes(), AUTO_KEY_SEED);
        }
        protected long autoHashKey(uint seed)
        {
            return base.GetHashKey(DateTime.Now.Ticks.GetBytes(), seed);
        }

        #endregion

        #region Constructor

        public Seeddeck(int capacity = 17, HashBits bits = HashBits.bit64) : base(bits)
        {
            size = capacity;
            minSize = capacity;
            table = EmptyCardTable(capacity);
            first = EmptyCard();
            last = first;
            mixMask = Submix.Mask((ulong)capacity);
            msbId = Submix.MsbId(capacity);
        }
        public Seeddeck(IList<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            this.Add(collection);
        }
        public Seeddeck(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        public Seeddeck(IEnumerable<ICard<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            this.Add(collection);
        }
        public Seeddeck(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
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
        protected                   V this[long hashkey]
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
                    return InnerGet(base.GetHashKey(ukey, ukey.SeedBlock));
                }
                else
                    throw new NotSupportedException();
            }
            set
            {
                if (key is IUnique)
                {
                    IUnique ukey = (IUnique)key;
                    InnerPut(base.GetHashKey(ukey, ukey.SeedBlock), value);
                }
                else
                    throw new NotSupportedException();
            }
        }
        public virtual              V this[object key, uint seed]
        {
            get { return InnerGet(base.GetHashKey(key, seed)); }
            set { InnerPut(base.GetHashKey(key, seed), value); }
        }

        #endregion

        #region Get

        protected virtual         V InnerGet(long key)
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
        public virtual            V Get(long key)
        {
            return InnerGet(key);
        }
        public virtual            V Get(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGet(base.GetHashKey(ukey, ukey.SeedBlock));
            }
            else
                throw new NotSupportedException();
        }
        public virtual            V Get(object key, uint seed)
        {
            return InnerGet(base.GetHashKey(key, seed));
        }
        public virtual            V Get(IUnique key)
        {
            return InnerGet(base.GetHashKey(key, key.SeedBlock));
        }
        public virtual            V Get(IUnique<V> key)
        {
            return InnerGet(base.GetHashKey(key, key.SeedBlock));
        }

        protected virtual      bool InnerTryGet(long key, out ICard<V> output)
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
        public virtual         bool TryGet(long key, out ICard<V> output)
        {
            return InnerTryGet(key, out output);
        }
        public virtual         bool TryGet(object key, out ICard<V> output)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerTryGet(base.GetHashKey(ukey, ukey.SeedBlock), out output);
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
                if(InnerTryGet(base.GetHashKey(ukey, ukey.SeedBlock), out card))
                {
                    output = card.Value;
                    return true;
                }
                return false;
            }
            else
                throw new NotSupportedException();          
        }
        public virtual         bool TryGet(object key, uint seed, out ICard<V> output)
        {
            return InnerTryGet(base.GetHashKey(key, seed), out output);
        }
        public virtual         bool TryGet(object key, uint seed, out V output)
        {
            output = default(V);
            ICard<V> card = null;
            if (InnerTryGet(base.GetHashKey(key, seed), out card))
            {
                output = card.Value;
                return true;
            }
            return false;
        }
        public virtual         bool TryGet(long key, out V output)
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

        protected virtual   ICard<V> InnerGetCard(long key)
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
        public virtual      ICard<V> GetCard(long key)
        {
            return             InnerGetCard(key);
        }
        public virtual      ICard<V> GetCard(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                return InnerGetCard(base.GetHashKey(ukey, ukey.SeedBlock));
            }
            else
                throw new NotSupportedException();
        }
        public virtual      ICard<V> GetCard(object key, uint seed)
        {
            return InnerGetCard(base.GetHashKey(key, seed));
        }
        public abstract     ICard<V> GetCard(int index);

        #endregion

        #region Put

        protected virtual      ICard<V> InnerPut(long key, uint seed, V value)
        {
            value.SeedBlock = seed;
            value.KeyBlock = key;
            return InnerPut(value);
        }
        protected abstract     ICard<V> InnerPut(long key, V value);
        protected virtual      ICard<V> InnerPut(V value, uint seed)
        {
            value.SeedBlock = seed;
            return InnerPut(value);
        }
        protected abstract     ICard<V> InnerPut(V value);
        protected abstract     ICard<V> InnerPut(ICard<V> value);

        public virtual      ICard<V> Put(long key, object value)
        {
            return InnerPut(key, (V)value);
        }
        public virtual      ICard<V> Put(long key, V value)
        {
            return InnerPut(key, value);
        }
        public virtual      ICard<V> Put(object key, V value)
        {
           return InnerPut(base.GetHashKey(key, value.GetHashSeed()), value);
        }
        public virtual      ICard<V> Put(object key, uint seed, V value)
        {
            return InnerPut(base.GetHashKey(key, seed), seed, value);
        }
        public virtual      ICard<V> Put(object key, uint seed,  object value)
        {
            if (value is V)
            {
                V o = (V)value;
                return InnerPut(base.GetHashKey(key, seed), seed, (V)value);
            }
            return null;
        }
        public virtual      ICard<V> Put(ICard<V> card)
        {
            return InnerPut(card);
        }
        public virtual          void Put(IList<ICard<V>> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i]);
            }
        }
        public virtual          void Put(IEnumerable<ICard<V>> cards)
        {
            foreach (ICard<V> card in cards)
                InnerPut(card);
        }
        public virtual      ICard<V> Put(V value)
        {
            return InnerPut(value);
        }
        public virtual          void Put(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Put(cards[i]);
            }
        }
        public virtual          void Put(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Put(card);
        }

        public virtual      ICard<V> Put(V value, uint seed)
        {
           return InnerPut(value, seed);
        }
        public virtual          void Put(object value, uint seed)
        {
            if (value is IUnique<V>)
            {
                IUnique<V> v = (IUnique<V>)value;
                Put(v);
            }
            else if (value is V)
                Put((V)value, seed);
        }
        public virtual          void Put(IList<V> cards, uint seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                InnerPut(cards[i], seed);
            }
        }
        public virtual          void Put(IEnumerable<V> cards, uint seed)
        {
            foreach (V card in cards)
                InnerPut(card, seed);
        }
        public virtual      ICard<V> Put(IUnique<V> value)
        {
           return InnerPut(base.GetHashKey(value, value.GetHashSeed()), value.Value);
        }
        public virtual          void Put(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        public virtual          void Put(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Put(item);
            }
        }
        #endregion

        #region Add

        protected virtual     bool InnerAdd(long key, uint seed, V value)
        {
            value.SeedBlock = seed;
            value.KeyBlock = key;            
            return InnerAdd(value);
        }
        protected abstract    bool InnerAdd(long key, V value);
        protected virtual     bool InnerAdd(V value, uint seed)
        {
            value.SeedBlock = seed;
            return InnerAdd(value);
        }
        protected abstract    bool InnerAdd(V value);
        protected abstract    bool InnerAdd(ICard<V> value);
        public virtual        bool Add(long key, object value)
        {
            V o = (V)value;
            return InnerAdd(key, o.SeedBlock, o);
        }
        public virtual        bool Add(long key, V value)
        {
            return InnerAdd(key, value);
        }
        public virtual        bool Add(object key, V value)
        {            
           return InnerAdd(base.GetHashKey(key, value.GetHashSeed()), value);            
        }
        public virtual        bool Add(object key, uint seed, V value)
        {
            return InnerAdd(base.GetHashKey(key, seed), seed, value);
        }
        public virtual        void Add(ICard<V> card)
        {
            InnerAdd(card);
        }
        public virtual        void Add(IList<ICard<V>> cardList)
        {
            int c = cardList.Count;
            for (int i = 0; i < c; i++)
            {
                InnerAdd(cardList[i]);
            }
        }
        public virtual        void Add(IEnumerable<ICard<V>> cardTable)
        {
            foreach (ICard<V> card in cardTable)
                Add(card);
        }
        public virtual        void Add(V value)
        {
           InnerAdd(value);
        }
        public virtual        void Add(IList<V> cards)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i]);

            }
        }
        public virtual        void Add(IEnumerable<V> cards)
        {
            foreach (V card in cards)
                Add(card);
        }
        public virtual        bool Add(V value, uint seed)
        {
           return InnerAdd(value, seed);
        }
        public virtual        void Add(IList<V> cards, uint seed)
        {
            int c = cards.Count;
            for (int i = 0; i < c; i++)
            {
                Add(cards[i], seed);

            }
        }
        public virtual        void Add(IEnumerable<V> cards, uint seed)
        {
            foreach (V card in cards)
                Add(card, seed);
        }
        public virtual        void Add(IUnique<V> value)
        {
            InnerAdd(base.GetHashKey(value, value.GetHashSeed()), value.Value);
        }
        public virtual        void Add(IList<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        public virtual        void Add(IEnumerable<IUnique<V>> value)
        {
            foreach (IUnique<V> item in value)
            {
                Add(item);
            }
        }
        public virtual        bool TryAdd(V value)
        {
           return InnerAdd(value);
        }
        public virtual        bool TryAdd(V value, uint seed)
        {
            return InnerAdd(value, seed);
        }

        public virtual     ICard<V> AddNew()
        {
            if (InnerAdd(autoHashKey(), AUTO_KEY_SEED, default(V)))
                return Last;
            return null;
        }
        public virtual     ICard<V> AddNew(uint seed)
        {
            if (InnerAdd(autoHashKey(seed), default(V)))
                return Last;
            return null;
        }
        public virtual     ICard<V> AddNew(long key)
        {
            if (InnerAdd(key, default(V)))
                return Last;
            return null;
        }
        public virtual     ICard<V> AddNew(object key)
        {
            if (key is IUnique)
            {
                IUnique ukey = (IUnique)key;
                if (InnerAdd(base.GetHashKey(ukey, ukey.GetHashSeed()), default(V)))
                    return Last;
                return null;
            }
            else
                throw new NotSupportedException();
        }
        public virtual     ICard<V> AddNew(object key, uint seed)
        {
            if (InnerAdd(base.GetHashKey(key, seed), seed, default(V)))
                return Last;
            return null;
        }

        protected abstract    void InnerInsert(int index, ICard<V> item);
        public virtual        void Insert(int index, ICard<V> item)
        {
            // get position index in table, which is an absolute value from key %(modulo) size. Simply it is rest from dividing key and size                           
            long key = item.Key;
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
        public virtual        void Insert(int index, V item)
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
        public virtual       bool Enqueue(V value, uint seed)
        {
            return InnerAdd(value, seed);
        }
        public virtual       bool Enqueue(object key, uint seed, V value)
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
        private void renewClear(int capacity)
        {
            if (capacity != size || count > 0)
            {
                size = capacity;
                conflicts = 0;
                removed = 0;
                count = 0;
                table = EmptyCardTable(size);
                first = EmptyCard();
                last = first;
                mixMask = Submix.Mask((ulong)minSize);
                msbId = Submix.MsbId(minSize);
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

        protected bool      InnerContainsKey(long key)
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
            return InnerContainsKey(base.GetHashKey(key));
        }
        public virtual      bool ContainsKey(object key, uint seed)
        {
            return InnerContainsKey(base.GetHashKey(key, seed));
        }
        public virtual      bool ContainsKey(long key)
        {
            return InnerContainsKey(key);
        }
        public virtual      bool ContainsKey(IUnique key)
        {
            return InnerContainsKey(key.GetHashKey());
        }

        public virtual       bool Contains(ICard<V> item)
        {
            return InnerContainsKey(item.Key);
        }
        public virtual       bool Contains(IUnique<V> item)
        {
            return InnerContainsKey(base.GetHashKey(item, item.GetHashSeed()));
        }
        public virtual       bool Contains(V item)
        {
            return InnerContainsKey(base.GetHashKey(item));
        }
        public virtual       bool Contains(V item, uint seed)
        {
            return InnerContainsKey(base.GetHashKey(item, seed));
        }

        #endregion

        #region Remove

        protected virtual      V InnerRemove(long key)
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
            return InnerRemove(base.GetHashKey(item)).Equals(default(V)) ? false : true;
        }
        public virtual         V Remove(object key)
        {
            return InnerRemove(base.GetHashKey(key));
        }
        public virtual         V Remove(object key, uint seed)
        {
            return InnerRemove(base.GetHashKey(key, seed));
        }
        public virtual      bool Remove(ICard<V> item)
        {
            return InnerRemove(item.Key).Equals(default(V)) ? false : true;
        }
        public virtual      bool Remove(IUnique<V> item)
        {
            return TryRemove(base.GetHashKey(item, item.GetHashSeed()));
        }
        public virtual      bool TryRemove(object key)
        {
            return InnerRemove(base.GetHashKey(key)).Equals(default(V)) ? false : true;
        }
        public virtual      bool TryRemove(object key, uint seed)
        {
            return InnerRemove(base.GetHashKey(key, seed)).Equals(default(V)) ? false : true;
        }
        public virtual      void RemoveAt(int index)
        {
            InnerRemove(GetCard(index).Key);
        }

        public virtual      void Clear()
        {
            size = minSize;
            conflicts = 0;
            removed = 0;
            count = 0;
            table = EmptyCardTable(size);
            first = EmptyCard();
            last = first;
            mixMask = Submix.Mask((ulong)minSize);
            msbId = Submix.MsbId(minSize);
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
            mixMask = Submix.Mask((ulong)size);
            msbId = Submix.MsbId(size);
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

        public virtual       ICard<V> NewCard(long key, uint seed, V value)
        {
            value.SeedBlock = seed;
            value.KeyBlock = key;
            return NewCard(value);
        }
        public abstract      ICard<V> NewCard(long key, V value);
        public abstract      ICard<V> NewCard(object key, V value);
        public virtual       ICard<V> NewCard(object key, uint seed, V value)
        {
            value.SeedBlock = seed;
            return NewCard(base.GetHashKey(key, seed), value);
        }
        public abstract      ICard<V> NewCard(ICard<V> card);
        public virtual       ICard<V> NewCard(V card, uint seed)
        {
            card.SeedBlock = seed;
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

        public virtual            IEnumerator<long> GetKeyEnumerator()
        {
            return new CardKeyBlockSeries<V>(this);
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

        protected       ulong getPosition(long key)
        {
            // standard hashmap method to establish position / index in table

            // return ((ulong)key % (uint)size);

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type) 
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

            return Submix.Map(key, size - 1, mixMask, msbId);           
        }
        protected       ulong getPosition(long key, int newsize, ulong newMixMask, int newMsbId)
        {
            // standard hashmap method to establish position / index in table 

            // return ((ulong)key % (uint)newsize);

            // author's algorithm to establish position / index in table            
            // based on most significant bit - BSR (or equivalent depending on the cpu type)
            // alsow project must be compiled in x64 format (default) for x86 format proper C lib compilation of BitScan.dll is needed       

            return Submix.Map(key, newsize - 1, newMixMask, newMsbId);                    
        }

        protected virtual  void Rehash(int newSize)
        {
            int finish = count;
            int newsize = newSize;
            ICard<V>[] newcardTable = EmptyCardTable(newsize);
            ICard<V> card = first;
            card = card.Next;
            if (removed > 0)
            {
                rehashAndReindex(card, newcardTable, newsize);
            }
            else
            {
                rehash(card, newcardTable, newsize);
            }

            table = newcardTable;
            size = newsize;
        }

        private         void rehashAndReindex(ICard<V> card, ICard<V>[] newcardTable, int newSize)
        {
            int _conflicts = 0;
            int newsize = newSize;
            ulong newMixMask = Submix.Mask((ulong)newsize);
            int newMsbId = Submix.MsbId(newsize);
            ICard<V> _firstcard = EmptyCard();
            ICard<V> _lastcard = _firstcard;
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize, newMixMask, newMsbId);

                    ICard<V> mem = newcardTable[pos];

                    if (mem == null)
                    {
                        card.Extent = null;
                        newcardTable[pos] = _lastcard = _lastcard.Next = card;
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
            mixMask = newMixMask;
            msbId = newMsbId;
        }

        private         void rehash(ICard<V> card, ICard<V>[] newcardTable, int newSize)
        {
            int _conflicts = 0;
            int newsize = newSize;
            ulong newMixMask = Submix.Mask((ulong)newsize);
            int newMsbId = Submix.MsbId(newsize);
            do
            {
                if (!card.Removed)
                {
                    ulong pos = getPosition(card.Key, newsize, newMixMask, newMsbId);

                    ICard<V> mem = newcardTable[pos];

                    if (mem == null)
                    {
                        card.Extent = null;
                        newcardTable[pos] = card;
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
            mixMask = newMixMask;
            msbId = newMsbId;

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

         //~Hashdeck() {
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
