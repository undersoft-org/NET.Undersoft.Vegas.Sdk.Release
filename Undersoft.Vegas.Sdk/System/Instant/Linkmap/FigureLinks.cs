using System.Multemic;
using System.Collections.Generic;
using System.Linq;
using System.Uniques;

namespace System.Instant.Linkmap
{
    [JsonArray]
    [Serializable]
    public class FigureLinks : SharedAlbum<FigureLink>, IUnique
    {
        public FigureLinks()
        {
            LinkKeys = new Album<int[]>(5);
        }
        public FigureLinks(ICollection<FigureLink> links)
        {
            links.Select(l => l.Links = this).ToArray();
            Add(links);
        }

        public FigureLink this[string linkName]
        {
            get
            {
                var lgcy = this[linkName];                
                if (lgcy != null && lgcy.Links == null)
                    lgcy.Links = this;
                return lgcy;
            }

            set
            {
                value.Links = this;
                this[linkName] = value;
            }
        }
        public new FigureLink this[int linkid]
        {
            get
            {
                var lgcy = this[linkid];
                if (lgcy != null && lgcy.Links == null)
                    lgcy.Links = this;
                return lgcy;
            }

            set
            {
                value.Links = this;
                this[linkid] = value;
            }
        }

        public ICollection<FigureLink> Collect(ICollection<string> LinkNames = null)
        {
            if (LinkNames != null)
                return LinkNames.Select(l => this[l]).Where(f => f != null).ToArray();
            else
                return this.Cast<FigureLink>().ToArray();
        }
        public ICollection<FigureLink> Collect(ICollection<FigureLink> links)
        {
            if (links != null)
                return links.Select(l => this[l.Name]).Where(f => f != null).ToArray();
            else
                return this.Cast<FigureLink>().ToArray();
        }

        public FigureLink GetByTarget(string TargetName)
        {
            return this.Cast<FigureLink>().Where(c => c.TargetName == TargetName).FirstOrDefault();           
        }
        public FigureLink GetByOrigin(string OriginName)
        {
            return this.Cast<FigureLink>().Where(c => c.OriginName == OriginName).FirstOrDefault();
        }

        public IDeck<int[]> LinkKeys { get; set; }

        public override ICard<FigureLink> EmptyCard()
        {
            return new Card64<FigureLink>();
        }

        public override ICard<FigureLink> NewCard(long key, FigureLink value)
        {
            return new Card64<FigureLink>(key, value);
        }
        public override ICard<FigureLink> NewCard(object key, FigureLink value)
        {
            return new Card64<FigureLink>(key, value);
        }
        public override ICard<FigureLink> NewCard(ICard<FigureLink> value)
        {
            return new Card64<FigureLink>(value);
        }
        public override ICard<FigureLink> NewCard(FigureLink value)
        {
            return new Card64<FigureLink>(value);
        }

        public override ICard<FigureLink>[] EmptyCardTable(int size)
        {
            return new Card64<FigureLink>[size];
        }

        public override ICard<FigureLink>[] EmptyCardList(int size)
        {
            cards = new Card64<FigureLink>[size];
            return cards;
        }

        private ICard<FigureLink>[] cards;
        public ICard<FigureLink>[] Cards { get => cards; }

        protected override bool InnerAdd(FigureLink value)
        {
            return InnerAdd(NewCard(value));
        }
        protected override ICard<FigureLink> InnerPut(FigureLink value)
        {
            return InnerPut(NewCard(value));
        }

        private Ussn systemSerialCode;
        public Ussn SystemSerialCode
        { get => systemSerialCode; set => systemSerialCode = value; }

        public IUnique Empty => Ussn.Empty;

        public long KeyBlock
        { get => SystemSerialCode.KeyBlock; set => systemSerialCode.KeyBlock = value; }
        public uint SeedBlock
        {
            get => systemSerialCode.SeedBlock;
            set => systemSerialCode.SeedBlock = value;
        }

        public int CompareTo(IUnique other)
        {
            return systemSerialCode.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return systemSerialCode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return systemSerialCode.GetBytes();
        }

        public long GetHashKey()
        {
            return systemSerialCode.GetHashKey();
        }

        public byte[] GetKeyBytes()
        {
            return systemSerialCode.GetKeyBytes();
        }

        public void SetHashKey(long value)
        {
            systemSerialCode.SetHashKey(value);
        }

        public void SetHashSeed(uint seed)
        {
            systemSerialCode.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return systemSerialCode.GetHashSeed();
        }
    }
  
    public enum LinkSite
    {
        Origin,
        Target
    }
}
