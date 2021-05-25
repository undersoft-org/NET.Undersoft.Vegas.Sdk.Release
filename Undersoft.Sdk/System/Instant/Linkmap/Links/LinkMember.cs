using System.Linq;
using System.Uniques;
using System.Extract;
using System.Multemic;

namespace System.Instant.Linkmap
{
    [JsonObject]
    [Serializable]
    public class LinkMember : IUnique
    {
        public LinkMember()
        {
            KeyRubrics = new MemberRubrics();
        }
        public LinkMember(IFigures figures, FigureLink link, LinkSite site) : this()
        {
            Figures = figures;
            Name = figures.Type.Name;
            Site = site;
            Rubrics = figures.Rubrics;
            KeyRubrics = new MemberRubrics();
            Link = link;
            byte[] keybytes = new long[] { figures.KeyBlock, link.KeyBlock }.GetBytes();
            SetHashKey(keybytes.GetHashKey64());
            SetHashSeed((uint)keybytes.GetHashKey32());
        }

        public string Name { get; set; }

        public IFigures Figures { get; set; }

        public FigureLink Link { get; set; }

        public LinkSite Site { get; set; }

        public int BranchesCount = 0;

        public IRubrics Rubrics { get; set; }
    
        public IRubrics KeyRubrics { get; set; }

        public int[] GetLinkOrdinals()
        {
            return KeyRubrics.Ordinals;
        }

        public object[] GetLinkValues(IFigure figure)
        {
            return KeyRubrics.Ordinals.Select(x => figure[x]).ToArray();
        }

        public long GetLinkKey(IFigure figure)
        {
            return KeyRubrics.Ordinals.Select(x => figure[x]).ToArray().GetHashKey64();
        }

        public Ussc SystemSerialCode { get; set; }

        public IUnique Empty => Ussc.Empty;

        public long KeyBlock
        { get => SystemSerialCode.KeyBlock; set => SystemSerialCode.SetHashKey(value); }
        public uint SeedBlock
        { get => SystemSerialCode.SeedBlock; set => SystemSerialCode.SetHashSeed(value); }

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
            SystemSerialCode.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return SystemSerialCode.GetHashSeed();
        }
    }


}
