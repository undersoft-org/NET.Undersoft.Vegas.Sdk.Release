/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Board32.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Board32
    
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Sets
{
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Board32{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class Board32<V> : BaseBoard<V>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Board32{V}"/> class.
        /// </summary>
        /// <param name="_deckSize">The _deckSize<see cref="int"/>.</param>
        /// <param name="bits">The bits<see cref="HashBits"/>.</param>
        public Board32(int _deckSize = 9, HashBits bits = HashBits.bit64) : base(_deckSize, bits)
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
