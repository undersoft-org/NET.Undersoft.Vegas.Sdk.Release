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
    using System.Linq;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="LinkMember" />.
    /// </summary>
    [JsonObject]
    [Serializable]
    public class LinkMember : IUnique
    {
        #region Fields

        public int BranchesCount = 0;
        private Ussc serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember"/> class.
        /// </summary>
        public LinkMember()
        {
            KeyRubrics = new MemberRubrics();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="link">The link<see cref="Link"/>.</param>
        /// <param name="site">The site<see cref="LinkSite"/>.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMember"/> class.
        /// </summary>
        /// <param name="link">The link<see cref="Link"/>.</param>
        /// <param name="site">The site<see cref="LinkSite"/>.</param>
        public LinkMember(Link link, LinkSite site) : this()
        {
            Site = site;
            Link = link;
            UniqueKey = Unique.NewKey.GetBytes().UniqueKey(link.UniqueKey);
            UniqueSeed = link.UniqueKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Ussc.Empty;

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the KeyRubrics.
        /// </summary>
        public IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the Link.
        /// </summary>
        public Link Link { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Rubrics.
        /// </summary>
        public IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussc SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the Site.
        /// </summary>
        public LinkSite Site { get; set; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }

        /// <summary>
        /// The FigureLinkKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public ulong FigureLinkKey(IFigure figure)
        {
            return KeyRubrics.Ordinals.Select(x => figure[x]).ToArray().UniqueKey64(KeyRubrics.BinarySizes, KeyRubrics.BinarySize, UniqueKey);
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

        #endregion
    }
}
