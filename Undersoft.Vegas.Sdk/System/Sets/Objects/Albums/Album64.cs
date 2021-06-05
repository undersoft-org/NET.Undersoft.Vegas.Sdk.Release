/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Album64.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Catalog64
    
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
    /// Defines the <see cref="Album64{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class Album64<V> : BaseAlbum<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        public Album64() : base(17, HashBits.bit64)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="ICollection{IUnique{V}}"/>.</param>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        public Album64(ICollection<IUnique<V>> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit64)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="ICollection{V}"/>.</param>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        public Album64(ICollection<V> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit64)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="IEnumerable{IUnique{V}}"/>.</param>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        public Album64(IEnumerable<IUnique<V>> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit64)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="IEnumerable{V}"/>.</param>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        public Album64(IEnumerable<V> collections, int _deckSize = 17) : base(collections, _deckSize, HashBits.bit64)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Album64{V}"/> class.
        /// </summary>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        public Album64(int _deckSize = 17) : base(_deckSize, HashBits.bit64)
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
            return new Card64<V>[size];
        }

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
            return new Card64<V>(value);
        }

        #endregion
    }
}
