/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.BaseMassBoard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="BaseMassBoard{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public abstract class BaseMassBoard<V> : BaseMassDeck<V> where V : IUnique
    {
        #region Fields

        protected static readonly int WAIT_READ_TIMEOUT = 5000;
        protected static readonly int WAIT_REHASH_TIMEOUT = 5000;
        protected static readonly int WAIT_WRITE_TIMEOUT = 5000;
        public int readers;
        protected ManualResetEventSlim waitRead = new ManualResetEventSlim(true, 128);
        protected ManualResetEventSlim waitRehash = new ManualResetEventSlim(true, 128);
        protected ManualResetEventSlim waitWrite = new ManualResetEventSlim(true, 128);
        protected SemaphoreSlim writePass = new SemaphoreSlim(1);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassBoard(IEnumerable<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassBoard(IEnumerable<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassBoard(IList<IUnique<V>> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassBoard(IList<V> collection, int capacity = 17, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMassBoard{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public BaseMassBoard(int capacity = 17, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        public override void Clear()
        {
            acquireWriter();
            acquireRehash();

            base.Clear();

            releaseRehash();
            releaseWriter();
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="Array"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(Array array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="ICard{V}[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(ICard<V>[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="V[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        public override void CopyTo(V[] array, int index)
        {
            acquireReader();
            base.CopyTo(array, index);
            releaseReader();
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
                acquireReader();
                if (removed > 0)
                {
                    releaseReader();
                    acquireWriter();
                    Reindex();
                    releaseWriter();
                    acquireReader();
                }

                int i = -1;
                int id = index;
                var card = first.Next;
                for (; ; )
                {
                    if (++i == id)
                    {
                        releaseReader();
                        return card;
                    }
                    card = card.Next;
                }
            }
            return null;
        }

        /// <summary>
        /// The IndexOf.
        /// </summary>
        /// <param name="item">The item<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int IndexOf(ICard<V> item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(item);
            releaseReader();
            return id;
        }

        /// <summary>
        /// The IndexOf.
        /// </summary>
        /// <param name="item">The item<see cref="V"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int IndexOf(V item)
        {
            int id = 0;
            acquireReader();
            id = base.IndexOf(item);
            releaseReader();
            return id;
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="item">The item<see cref="ICard{V}"/>.</param>
        public override void Insert(int index, ICard<V> item)
        {
            acquireWriter();
            base.Insert(index, item);
            releaseWriter();
        }

        /// <summary>
        /// The ToArray.
        /// </summary>
        /// <returns>The <see cref="V[]"/>.</returns>
        public override V[] ToArray()
        {
            acquireReader();
            V[] array = base.ToArray();
            releaseReader();
            return array;
        }

        /// <summary>
        /// The TryDequeue.
        /// </summary>
        /// <param name="output">The output<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool TryDequeue(out ICard<V> output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The TryDequeue.
        /// </summary>
        /// <param name="output">The output<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool TryDequeue(out V output)
        {
            acquireWriter();
            var temp = base.TryDequeue(out output);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The acquireReader.
        /// </summary>
        protected void acquireReader()
        {
            Interlocked.Increment(ref readers);
            waitRehash.Reset();
            if (!waitRead.Wait(WAIT_READ_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
        }

        /// <summary>
        /// The acquireRehash.
        /// </summary>
        protected void acquireRehash()
        {
            if (!waitRehash.Wait(WAIT_REHASH_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
            waitRead.Reset();
        }

        /// <summary>
        /// The acquireWriter.
        /// </summary>
        protected void acquireWriter()
        {
            do
            {
                if (!waitWrite.Wait(WAIT_WRITE_TIMEOUT))
                    throw new TimeoutException("Wait write Timeout");
                waitWrite.Reset();
            }
            while (!writePass.Wait(0));
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(key, value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(V value)
        {
            acquireWriter();
            var temp = base.InnerAdd(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerGet.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="V"/>.</returns>
        protected override V InnerGet(ulong key)
        {
            acquireReader();
            var v = base.InnerGet(key);
            releaseReader();
            return v;
        }

        /// <summary>
        /// The InnerGetCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerGetCard(ulong key)
        {
            acquireReader();
            var card = base.InnerGetCard(key);
            releaseReader();
            return card;
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(ICard<V> value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(ulong key, V value)
        {
            acquireWriter();
            var temp = base.InnerPut(key, value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        protected override ICard<V> InnerPut(V value)
        {
            acquireWriter();
            var temp = base.InnerPut(value);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerRemove.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="V"/>.</returns>
        protected override V InnerRemove(ulong key)
        {
            acquireWriter();
            var temp = base.InnerRemove(key);
            releaseWriter();
            return temp;
        }

        /// <summary>
        /// The InnerTryGet.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="output">The output<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerTryGet(ulong key, out ICard<V> output)
        {
            acquireReader();
            var test = base.InnerTryGet(key, out output);
            releaseReader();
            return test;
        }

        /// <summary>
        /// The Rehash.
        /// </summary>
        /// <param name="newsize">The newsize<see cref="int"/>.</param>
        protected override void Rehash(int newsize)
        {
            acquireRehash();
            base.Rehash(newsize);
            releaseRehash();
        }

        /// <summary>
        /// The Reindex.
        /// </summary>
        protected override void Reindex()
        {
            acquireRehash();
            base.Reindex();
            releaseRehash();
        }

        /// <summary>
        /// The releaseReader.
        /// </summary>
        protected void releaseReader()
        {
            if (0 == Interlocked.Decrement(ref readers))
                waitRehash.Set();
        }

        /// <summary>
        /// The releaseRehash.
        /// </summary>
        protected void releaseRehash()
        {
            waitRead.Set();
        }

        /// <summary>
        /// The releaseWriter.
        /// </summary>
        protected void releaseWriter()
        {
            writePass.Release();
            waitWrite.Set();
        }

        #endregion
    }
}
