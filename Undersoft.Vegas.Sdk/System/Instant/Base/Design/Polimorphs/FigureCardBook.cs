/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureCardBook.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Instant.FigureBook
    
    Implementation of BaseAlbum abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk             
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Instant
{
    using System.Collections.Generic;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="FigureBaseAlbum" />.
    /// </summary>
    public abstract class FigureBaseAlbum : BaseAlbum<IFigure>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseAlbum"/> class.
        /// </summary>
        public FigureBaseAlbum() : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseAlbum"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="ICollection{IFigure}"/>.</param>
        public FigureBaseAlbum(ICollection<IFigure> collections) : base(collections)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseAlbum"/> class.
        /// </summary>
        /// <param name="collections">The collections<see cref="IEnumerable{IFigure}"/>.</param>
        public FigureBaseAlbum(IEnumerable<IFigure> collections) : base(collections)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureBaseAlbum"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public FigureBaseAlbum(int capacity) : base(capacity)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}[]"/>.</returns>
        public override ICard<IFigure>[] EmptyBaseDeck(int size)
        {
            return new Card<IFigure>[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        public override ICard<IFigure> EmptyCard()
        {
            return new Card<IFigure>();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}[]"/>.</returns>
        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new Card<IFigure>[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new Card<IFigure>(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new Card<IFigure>(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        public override ICard<IFigure> NewCard(ulong key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(IFigure value)
        {
            return InnerAdd(NewCard(value));
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="ICard{IFigure}"/>.</returns>
        protected override ICard<IFigure> InnerPut(IFigure value)
        {
            return InnerPut(NewCard(value));
        }

        #endregion
    }
}
