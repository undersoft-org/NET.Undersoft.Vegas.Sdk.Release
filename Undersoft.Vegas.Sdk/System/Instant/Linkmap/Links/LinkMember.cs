/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.LinkMember.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Linking
{
    using System.Extract;
    using System.Uniques;
    using System.Linq;

    [JsonObject]
    [Serializable]
    public class LinkMember : IUnique
    {
        #region Fields

        public int BranchesCount = 0;
        private Ussc serialcode;

        #endregion

        #region Constructors

        public LinkMember()
        {
            KeyRubrics = new MemberRubrics();
        }
        public LinkMember(Link link, LinkSite site) : this()
        {                      
            Site = site;                    
            Link = link;
            UniqueKey = Unique.NewKey.GetBytes().UniqueKey(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
        }
        public LinkMember(IFigures figures, Link link, LinkSite site) : this()
        {
            Figures = figures;
            Name = figures.Type.Name;
            Site = site;
            Rubrics = figures.Rubrics;                        
            Link = link;
            UniqueKey = figures.GetUniqueBytes().UniqueKey(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
        }

        #endregion

        #region Properties

        public IUnique Empty => Ussc.Empty;

        public IFigures Figures { get; set; }

        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        public IRubrics KeyRubrics { get; set; }

        public Link Link { get; set; }

        public string Name { get; set; }

        public IRubrics Rubrics { get; set; }

        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

        public LinkSite Site { get; set; }

        public Ussc SerialCode { get => serialcode; set => serialcode = value; }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public ulong FigureLinkKey(IFigure figure)
        {
            return KeyRubrics.Ordinals.Select(x => figure[x]).ToArray().UniqueKey64(KeyRubrics.BinarySizes, KeyRubrics.BinarySize, UniqueKey);
        }
          
        #endregion 
    }
}
