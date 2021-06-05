/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.LinkNodes.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Linking
{
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="NodeCatalog" />.
    /// </summary>
    [Serializable]
    public class NodeCatalog : BaseCatalog<BranchDeck>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCatalog"/> class.
        /// </summary>
        public NodeCatalog() : base()
        {
            Links = Linker.Map.Links;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCatalog"/> class.
        /// </summary>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public NodeCatalog(int capacity) : base(capacity)
        {
            Links = Linker.Map.Links;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCatalog"/> class.
        /// </summary>
        /// <param name="links">The links<see cref="Links"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public NodeCatalog(Links links, int capacity) : base(capacity)
        {
            Links = links;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Links.
        /// </summary>
        public Links Links { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}[]"/>.</returns>
        public override ICard<BranchDeck>[] EmptyBaseDeck(int size)
        {
            return new NodeCard[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{BranchDeck}"/>.</returns>
        public override ICard<BranchDeck> EmptyCard()
        {
            return new NodeCard();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}[]"/>.</returns>
        public override ICard<BranchDeck>[] EmptyCardTable(int size)
        {
            return new NodeCard[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="card">The card<see cref="BranchDeck"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}"/>.</returns>
        public override ICard<BranchDeck> NewCard(BranchDeck card)
        {
            return new NodeCard(card);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{BranchDeck}"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}"/>.</returns>
        public override ICard<BranchDeck> NewCard(ICard<BranchDeck> card)
        {
            return new NodeCard(card);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="BranchDeck"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}"/>.</returns>
        public override ICard<BranchDeck> NewCard(object key, BranchDeck value)
        {
            return new NodeCard(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="BranchDeck"/>.</param>
        /// <returns>The <see cref="ICard{BranchDeck}"/>.</returns>
        public override ICard<BranchDeck> NewCard(ulong key, BranchDeck value)
        {
            return new NodeCard(key, value);
        }

        #endregion
    }
}
