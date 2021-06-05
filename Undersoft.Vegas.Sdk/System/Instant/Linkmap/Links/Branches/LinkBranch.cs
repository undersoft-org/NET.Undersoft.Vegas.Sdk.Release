using System.Multemic;
using System.Collections.Generic;
using System.Linq;
using System.Uniques;

namespace System.Instant.Linkmap
{
    [Serializable]
    public class LinkBranch : CardList<ICard<IFigure>>, IUnique
    {
        public LinkBranch(LinkMember member, ICard<IFigure> value) : base(5, HashBits.bit64)
        {
            Member = member;
            var card = NewCard(value);
            KeyBlock = card.IdentitiesToKey();
            InnerAdd(card);
        }
        public LinkBranch(LinkMember member, ICard<IFigure> value, int _cardSize) : base(_cardSize, HashBits.bit64)
        {
            Member = member;
            var card = NewCard(value);
            KeyBlock = card.IdentitiesToKey();
            InnerAdd(card);
        }
        public LinkBranch(LinkMember member, ICollection<ICard<IFigure>> collections, int _cardSize = 5) : base(_cardSize, HashBits.bit64)
        {
            Member = member;
            if(collections.Any())
            {
                var card = NewCard(collections.First());
                KeyBlock = card.IdentitiesToKey();
                InnerAdd(card);
            }
            foreach (var card in collections.Skip(1))
                InnerAdd(card);
        }
        public LinkBranch(LinkMember member, IEnumerable<ICard<IFigure>> collections, int _cardSize = 5) : base(_cardSize, HashBits.bit64)
        {
            Member = member;
            if (collections.Any())
            {
                var card = NewCard(collections.First());
                KeyBlock = card.IdentitiesToKey();
                InnerAdd(card);
            }
            foreach (var card in collections.Skip(1))
                InnerAdd(card);
        }

        public override ICard<ICard<IFigure>> EmptyCard()
        {
            return new BranchCard(Member);
        }

        public override ICard<ICard<IFigure>> NewCard(long key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }
        public override ICard<ICard<IFigure>> NewCard(object key, ICard<IFigure> value)
        {
            return new BranchCard(key, value, Member);
        }
        public override ICard<ICard<IFigure>> NewCard(ICard<ICard<IFigure>> value)
        {
            return new BranchCard(value, Member);
        }
        public override ICard<ICard<IFigure>> NewCard(ICard<IFigure> value)
        {
            return new BranchCard(value, Member);
        }

        public override ICard<ICard<IFigure>>[] EmptyCardTable(int size)
        {
            return new BranchCard[size];
        }

        private ICard<ICard<IFigure>>[] cards;
        public ICard<ICard<IFigure>>[] Cards { get => cards; }

        protected override bool InnerAdd(ICard<IFigure> value)
        {
            var card = NewCard(value);
            if (KeyBlock == 0)
                KeyBlock = card.IdentitiesToKey();
            return InnerAdd(card);
        }

        protected override ICard<ICard<IFigure>> InnerPut(ICard<IFigure> value)
        {
            return InnerPut(NewCard(value));
        }

        public LinkMember Member { get; set; }

        public Usid SystemSerialCode { get; set; }

        public IUnique Empty => Usid.Empty;

        public long KeyBlock
        { get => SystemSerialCode.KeyBlock; set => SystemSerialCode.SetHashKey(value); }
        public uint SeedBlock
        { get => Member.SeedBlock; set => Member.SetHashSeed(value); }

        public int CompareTo(IUnique other)
        {
            return SystemSerialCode.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return SystemSerialCode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return SystemSerialCode.GetBytes();
        }

        public long GetHashKey()
        {
            return SystemSerialCode.GetHashKey();
        }

        public byte[] GetKeyBytes()
        {
            return SystemSerialCode.GetKeyBytes();
        }

        public void SetHashKey(long value)
        {
            SystemSerialCode.SetHashKey(value);
        }

        public void SetHashSeed(uint seed)
        {
            Member.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return Member.GetHashSeed();
        }
    }
}
