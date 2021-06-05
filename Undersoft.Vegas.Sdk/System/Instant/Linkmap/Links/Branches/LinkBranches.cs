using System.Uniques;
using System.Multemic;

namespace System.Instant.Linkmap
{
    [Serializable]
    public class LinkBranches : SharedMultiAlbum<LinkBranch>
    {
        public LinkBranches(int capacity = 17) : base(capacity)
        {
        }

        public LinkBranches(FigureLinks links, int capacity = 17) : base(capacity)
        {
            Links = links;
        }

        public override ICard<LinkBranch>[] EmptyCardList(int size)
        {
            return new LinkCard[size];
        }

        public override ICard<LinkBranch> EmptyCard()
        {
            return new LinkCard();
        }

        public override ICard<LinkBranch> NewCard(long key, LinkBranch value)
        {
            return new LinkCard(key, value);
        }

        public override ICard<LinkBranch> NewCard(object key, LinkBranch value)
        {
            return new LinkCard(key, value);
        }

        public override ICard<LinkBranch> NewCard(ICard<LinkBranch> card)
        {
            return new LinkCard(card);
        }

        public override ICard<LinkBranch> NewCard(LinkBranch card)
        {
            return new LinkCard(card);
        }

        public override ICard<LinkBranch>[] EmptyCardTable(int size)
        {
            return new LinkCard[size];
        }

        public FigureLinks Links { get; set; }

    }

}
