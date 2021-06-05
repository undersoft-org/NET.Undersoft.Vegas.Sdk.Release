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

    /// <summary>
    /// Defines the <see cref="BranchDeck" />.
    /// </summary>
    [Serializable]
    public class BranchDeck : BaseDeck<ICard<IFigure>>, IUnique
    {
        #region Fields

        private Usid serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDeck"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="LinkMember"/>.</param>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        public BranchDeck(LinkMember member, ICard<IFigure> value) : base(7)
        {
            Member = member;
            var card = NewCard(value);
            UniqueKey = member.FigureLinkKey(value.Value);
            InnerAdd(card);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDeck"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="LinkMember"/>.</param>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
        public BranchDeck(LinkMember member, ICard<IFigure> value, int capacity) : base(capacity)
        {
            Member = member;
            var card = NewCard(value);
            UniqueKey = member.FigureLinkKey(value.Value);
            InnerAdd(card);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDeck"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="LinkMember"/>.</param>
        /// <param name="collections">The collections<see cref="ICollection{ICard{IFigure}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDeck"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="LinkMember"/>.</param>
        /// <param name="collections">The collections<see cref="IEnumerable{ICard{IFigure}}"/>.</param>
        /// <param name="capacity">The capacity<see cref="int"/>.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDeck"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="LinkMember"/>.</param>
        /// <param name="linkkey">The linkkey<see cref="ulong"/>.</param>
        public BranchDeck(LinkMember member, ulong linkkey) : base(9)
        {
            Member = member;
            UniqueKey = linkkey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Usid.Empty;

        /// <summary>
        /// Gets or sets the Member.
        /// </summary>
        public LinkMember Member { get; set; }

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Usid SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => Member.UniqueSeed; set => Member.UniqueSeed = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
        public override ICard<ICard<IFigure>> EmptyCard()
        {
            return new BranchCard(Member);
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}[]"/>.</returns>
        public override ICard<ICard<IFigure>>[] EmptyCardTable(int size)
        {
            return new BranchCard[size];
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{ICard{IFigure}}"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
        public override ICard<ICard<IFigure>> NewCard(ICard<ICard<IFigure>> value)
        {
            return new BranchCard(value, Member);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
        public override ICard<ICard<IFigure>> NewCard(ICard<IFigure> value)
        {
            return new BranchCard(value, Member);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
        public override ICard<ICard<IFigure>> NewCard(object key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
        public override ICard<ICard<IFigure>> NewCard(ulong key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }

        /// <summary>
        /// The InnerAdd.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected override bool InnerAdd(ICard<IFigure> value)
        {
            var card = NewCard(value);
            if (UniqueKey == 0)
                UniqueKey = card.UniquesAsKey();
            return InnerAdd(card);
        }

        /// <summary>
        /// The InnerPut.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{IFigure}"/>.</param>
        /// <returns>The <see cref="ICard{ICard{IFigure}}"/>.</returns>
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
