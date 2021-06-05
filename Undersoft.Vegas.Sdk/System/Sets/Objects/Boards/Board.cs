/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Board.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Board   
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                        
             
 *********************************************************************************/
namespace System.Sets
{
    using System.Collections.Generic;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Board{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class Board<V> : BaseBoard<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Board{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{ICard{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public Board(IEnumerable<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Board{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{ICard{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public Board(IList<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Board{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public Board(int capacity = 9, HashBits bits = HashBits.bit64) : base(capacity, bits)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> EmptyCard()
        {
            return new Card64<V>();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}[]"/>.</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new Card64<V>[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new Card64<V>(card);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new Card64<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new Card64<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new Card64<V>(value, value);
        }

        #endregion
    }
}
