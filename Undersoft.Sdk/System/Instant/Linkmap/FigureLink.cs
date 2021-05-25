using System.Uniques;

namespace System.Instant.Linkmap
{
    [JsonObject]
    [Serializable]
    public class FigureLink : IUnique
    {
        [NonSerialized] public FigureLinks Links;

        public FigureLink()
        {
            Name = "FiguresLink#" + DateTime.Now.ToBinary().ToString();
            Origin = new LinkMember();
            Origin.Site = LinkSite.Origin;
            Target = new LinkMember();
            Target.Site = LinkSite.Target;
            SetHashKey(Name.GetHashKey64());
        }
        public FigureLink(IFigures origin, IFigures target)
        {
            Name = origin.Type.Name + "_" + target.Type.Name;
            Origin = new LinkMember(origin, this, LinkSite.Origin);         
            Target = new LinkMember(target, this, LinkSite.Target);
            SetHashKey(Name.GetHashKey64());
            origin.Links.Put(this);
            target.Links.Put(this);
        }

        public string Name { get; set; }

        public LinkMember Origin { get; set; }
        public LinkMember Target { get; set; }

        public string OriginName
        {
            get { return Origin.Name; }
            set
            {
                if (Links != null)
                {
                    var links = Links[Name];
                    if (links != null)
                    {
                        IFigures figures = links.Origin.Figures;
                        Origin.Figures = figures;
                        Origin.Name = figures.Type.Name;
                        Origin.Rubrics = figures.Rubrics;
                        Origin.KeyRubrics = new MemberRubrics();
                    }
                    Target.Name = value;
                }
            }
        }

        public IRubrics OriginRubrics
        {
            get
            {
                return Origin.Rubrics;
            }
            set
            {
                Origin.Rubrics = value;
            }
        }
        public IRubrics OriginKeys
        {
            get
            {
                return Origin.KeyRubrics;
            }
            set
            {
                Origin.KeyRubrics = value;
            }
        }

        public string TargetName
        {
            get { return Target.Name; }
            set
            {
                if (Links != null)
                {
                    var links = Links[Name];
                    if (links != null)
                    {
                        IFigures figures = links.Target.Figures;
                        Target.Figures = figures;
                        Target.Name = figures.Type.Name;
                        Target.Rubrics = figures.Rubrics;
                        Target.KeyRubrics = new MemberRubrics();
                    }
                }
                Target.Name = value;
            }
        }

        public IRubrics TargetRubrics
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics = value;
            }
        }
        public IRubrics TargetKeys
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics = value;
            }
        }

        private Ussc systemSerialCode;
        public Ussn SystemSerialCode
        {
            get => new Ussn(systemSerialCode.KeyBlock, systemSerialCode.SeedBlock);
            set
            {
                systemSerialCode.KeyBlock = value.KeyBlock;
                systemSerialCode.SeedBlock = value.SeedBlock;
            }
        }

        public IUnique Empty => Ussn.Empty;

        public long KeyBlock
        { get => systemSerialCode.KeyBlock; set => systemSerialCode.KeyBlock = value; }

        public object[] ValueArray
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public uint SeedBlock { get => systemSerialCode.SeedBlock; set => systemSerialCode.SeedBlock = value; }

        public object this[int fieldId]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public object this[string propertyName]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
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
}
