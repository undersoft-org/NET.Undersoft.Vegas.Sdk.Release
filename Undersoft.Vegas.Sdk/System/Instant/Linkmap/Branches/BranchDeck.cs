/*************************************************
   Copyright (c) 2021 Undersoft

   LinkBranch.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Instant.Linking
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Sets;
    using System.Uniques;

    [Serializable]
    public class BranchDeck : BaseDeck<ICard<IFigure>>, IUnique
    {
        #region Fields

        private Usid serialcode;

        #endregion

        #region Constructors

        public BranchDeck(LinkMember member, ulong linkkey) : base(9)
        {
            Member = member;
            UniqueKey = linkkey;
        }
        public BranchDeck(LinkMember member, ICard<IFigure> value) : base(7)
        {
            Member = member;
            var card = NewCard(value);
            UniqueKey = member.FigureLinkKey(value.Value);
            InnerAdd(card);
        }
        public BranchDeck(LinkMember member, ICard<IFigure> value, int capacity) : base(capacity)
        {
            Member = member;
            var card = NewCard(value);
            UniqueKey = member.FigureLinkKey(value.Value);
            InnerAdd(card);
        }
        public BranchDeck(LinkMember member, ICollection<ICard<IFigure>> collections, int capacity = 7) : base(capacity)
        {
            Member = member;
            if (collections.Any())
            {
                var val = collections.First();
                var card = NewCard(val);
                UniqueKey = member.FigureLinkKey(val.Value);
                InnerAdd(card);
            }
            foreach (var card in collections.Skip(1))
                InnerAdd(card);
        }
        public BranchDeck(LinkMember member, IEnumerable<ICard<IFigure>> collections, int capacity = 7) : base(capacity)
        {
            Member = member;
            if (collections.Any())
            {
                var val = collections.First();
                var card = NewCard(val);
                UniqueKey = member.FigureLinkKey(val.Value);
                InnerAdd(card);
            }
            foreach (var card in collections.Skip(1))
                InnerAdd(card);
        }

        #endregion

        #region Properties

        public IUnique Empty => Usid.Empty;

        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        public LinkMember Member { get; set; }

        public ulong UniqueSeed { get => Member.UniqueSeed; set => Member.UniqueSeed = value; }

        public Usid SerialCode { get => serialcode; set => serialcode = value; }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        public override ICard<ICard<IFigure>> EmptyCard()
        {
            return new BranchCard(Member);
        }

        public override ICard<ICard<IFigure>>[] EmptyCardTable(int size)
        {
            return new BranchCard[size];
        }

        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public override ICard<ICard<IFigure>> NewCard(ICard<ICard<IFigure>> value)
        {
            return new BranchCard(value, Member);
        }

        public override ICard<ICard<IFigure>> NewCard(ICard<IFigure> value)
        {
            return new BranchCard(value, Member);
        }

        public override ICard<ICard<IFigure>> NewCard(ulong  key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }

        public override ICard<ICard<IFigure>> NewCard(object key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }

        protected override bool InnerAdd(ICard<IFigure> value)
        {
            var card = NewCard(value);
            if (UniqueKey == 0)
                UniqueKey = card.UniquesAsKey();
            return InnerAdd(card);
        }

        protected override ICard<ICard<IFigure>> InnerPut(ICard<IFigure> value)
        {
            var card = NewCard(value);
            if (UniqueKey == 0)
                UniqueKey = card.UniquesAsKey();
            return InnerPut(card);
        }

        #endregion
    }
}
