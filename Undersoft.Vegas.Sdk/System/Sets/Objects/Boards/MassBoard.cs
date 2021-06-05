/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.MassBoard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.MultiBoard   
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                        
             
 *********************************************************************************/
namespace System.Sets
{
    using System.Collections.Generic;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="MassBoard{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class MassBoard<V> : BaseMassBoard<V> where V : IUnique
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{ICard{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public MassBoard(IEnumerable<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassBoard{V}"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{ICard{V}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public MassBoard(IList<ICard<V>> collection, int capacity = 9, HashBits bits = HashBits.bit64) : this(capacity > collection.Count ? capacity : collection.Count, bits)
        {
            foreach (var c in collection)
                this.Add(c);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MassBoard{V}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public MassBoard(int capacity = 9, HashBits bits = HashBits.bit64) : base(capacity, bits)
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
            return new MassCard<V>();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{V}[]"/>.</returns>
        public override ICard<V>[] EmptyCardTable(int size)
        {
            return new MassCard<V>[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ICard<V> card)
        {
            return new MassCard<V>(card);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(object key, V value)
        {
            return new MassCard<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(ulong key, V value)
        {
            return new MassCard<V>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="ICard{V}"/>.</returns>
        public override ICard<V> NewCard(V value)
        {
            return new MassCard<V>(value, value);
        }

        #endregion
    }
}
