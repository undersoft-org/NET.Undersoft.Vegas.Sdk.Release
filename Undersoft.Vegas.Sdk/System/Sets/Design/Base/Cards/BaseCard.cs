/******************************************
   Copyright (c) 2021 Undersoft

   System.Sets.BaseCard.cs
    
    Card abstract class. 
    Reference type of common used 
    value type Bucket in Hashtables.
    Include properties: 
    Key - long abstract property to implement different
          type fields with hashkey like long, int etc.
    Value - Generic type property to store collection item.
    Next - for one site list implementation. 
    Extent - for one site list hash conflict items
    Removed - flag for removed items to skip before
              removed items limit exceed and rehash
              process executed
        
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 ****************************************/

namespace System.Sets
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="BaseCard{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    public abstract class BaseCard<V> : IEquatable<ICard<V>>, IEquatable<object>, IEquatable<ulong>, IComparable<object>,
                                    IComparable<ulong>, IComparable<ICard<V>>, ICard<V>
    {
        #region Fields

        protected V value;
        private bool disposedValue = false;
        private ICard<V> extent;
        private ICard<V> next;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard{V}"/> class.
        /// </summary>
        public BaseCard()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard{V}"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        public BaseCard(ICard<V> value)
        {
            Set(value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard{V}"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public BaseCard(object key, V value)
        {
            Set(key, value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard{V}"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public BaseCard(ulong key, V value)
        {
            Set(key, value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard{V}"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        public BaseCard(V value)
        {
            Set(value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public virtual IUnique Empty => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the Extent.
        /// </summary>
        public virtual ICard<V> Extent { get => extent; set => extent = value; }

        /// <summary>
        /// Gets or sets the Index.
        /// </summary>
        public virtual int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public abstract ulong Key { get; set; }

        /// <summary>
        /// Gets or sets the Next.
        /// </summary>
        public virtual ICard<V> Next { get => next; set => next = value; }

        /// <summary>
        /// Gets or sets a value indicating whether Removed.
        /// </summary>
        public virtual bool Removed { get; set; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public virtual ulong UniqueKey { get => Key; set => Key = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public virtual ulong UniqueSeed { get => 0; set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public V Value { get => value; set => this.value = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public virtual int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public virtual int CompareTo(IUnique other)
        {
            return (int)(Key - other.UniqueKey);
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public abstract int CompareTo(object other);

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public virtual int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool Equals(ICard<V> y)
        {
            return this.Equals(y.Key);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool Equals(IUnique other)
        {
            return Key == other.UniqueKey;
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override abstract bool Equals(object y);

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool Equals(ulong key)
        {
            return Key == key;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public abstract byte[] GetBytes();

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override abstract int GetHashCode();

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public abstract byte[] GetUniqueBytes();

        /// <summary>
        /// The GetUniqueType.
        /// </summary>
        /// <returns>The <see cref="Type"/>.</returns>
        public virtual Type GetUniqueType()
        {
            return this.GetType();
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        public abstract void Set(ICard<V> card);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public abstract void Set(object key, V value);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public virtual void Set(ulong key, V value)
        {
            this.value = value;
            Key = key;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        public abstract void Set(V value);

        /// <summary>
        /// The UniqueOrdinals.
        /// </summary>
        /// <returns>The <see cref="int[]"/>.</returns>
        public virtual int[] UniqueOrdinals()
        {
            return null;
        }

        /// <summary>
        /// The UniquesAsKey.
        /// </summary>
        /// <returns>The <see cref="ulong"/>.</returns>
        public virtual ulong UniquesAsKey()
        {
            return Key;
        }

        /// <summary>
        /// The UniqueValues.
        /// </summary>
        /// <returns>The <see cref="object[]"/>.</returns>
        public virtual object[] UniqueValues()
        {
            return new object[] { Key };
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Value = default(V);
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}
