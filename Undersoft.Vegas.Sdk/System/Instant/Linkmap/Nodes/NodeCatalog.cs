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

    [Serializable]
    public class NodeCatalog : BaseCatalog<BranchDeck>
    {
        #region Constructors

        public NodeCatalog() : base()
        {
            Links = Linker.Map.Links;
        }
        public NodeCatalog(int capacity) : base(capacity)
        {
            Links = Linker.Map.Links;
        }
        public NodeCatalog(Links links, int capacity) : base(capacity)
        {
            Links = links;
        }

        #endregion

        #region Properties

        public Links Links { get; set; }

        #endregion

        #region Methods

        public override ICard<BranchDeck> EmptyCard()
        {
            return new NodeCard();
        }

        public override ICard<BranchDeck>[] EmptyBaseDeck(int size)
        {
            return new NodeCard[size];
        }

        public override ICard<BranchDeck>[] EmptyCardTable(int size)
        {
            return new NodeCard[size];
        }

        public override ICard<BranchDeck> NewCard(ICard<BranchDeck> card)
        {
            return new NodeCard(card);
        }

        public override ICard<BranchDeck> NewCard(BranchDeck card)
        {
            return new NodeCard(card);
        }

        public override ICard<BranchDeck> NewCard(ulong  key, BranchDeck value)
        {
            return new NodeCard(key, value);
        }

        public override ICard<BranchDeck> NewCard(object key, BranchDeck value)
        {
            return new NodeCard(key, value);
        }

        #endregion
    }
}
