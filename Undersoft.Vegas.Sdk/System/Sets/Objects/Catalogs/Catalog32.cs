/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Catalog32.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Catalog32
    
    Implementation of Safe-Thread Album abstract class
    using 32 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                      
 
 ******************************************************************/
namespace System.Sets
{
    using System.Collections.Generic;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Catalog32{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class Catalog32<V> : BaseCatalog<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public Catalog32(IEnumerable<IUnique<V>> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public Catalog32(IEnumerable<V> collection, int capacity = 17) : this(capacity)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{IUnique{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public Catalog32(IList<IUnique<V>> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{V}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public Catalog32(IList<V> collection, int capacity = 17) : this(capacity > collection.Count ? capacity : collection.Count)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog32{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public Catalog32(int capacity = 17) : base(capacity, HashBits.bit32)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}[]"/>.</returns>
        public override ICard<V>[] EmptyBaseDeck(int size)
        {
            return new Card32<V>[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> EmptyCard()
        {
            return new Card32<V>();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}[]"/>.</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card32<V>[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card32<V>(card);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card32<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card32<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new Card32<V>(value);
        }

        #endregion
    }
}
