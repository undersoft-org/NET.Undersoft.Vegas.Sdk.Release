/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Linkmap.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Linking
{
    using System.Linq;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="ILinker" />.
    /// </summary>
    public interface ILinker
    {
        #region Properties

        /// <summary>
        /// Gets the OriginLinks.
        /// </summary>
        Links OriginLinks { get; }

        /// <summary>
        /// Gets the TargetLinks.
        /// </summary>
        Links TargetLinks { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        void Clear();

        /// <summary>
        /// The GetOrigin.
        /// </summary>
        /// <param name="target">The target<see cref="IFigure"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="BranchDeck"/>.</returns>
        BranchDeck GetOrigin(IFigure target, string OriginName);

        /// <summary>
        /// The GetOrigins.
        /// </summary>
        /// <param name="target">The target<see cref="IFigures"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="IDeck{BranchDeck}"/>.</returns>
        IDeck<BranchDeck> GetOrigins(IFigures target, string OriginName);

        /// <summary>
        /// The GetTarget.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigure"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="BranchDeck"/>.</returns>
        BranchDeck GetTarget(IFigure origin, string TargetName);

        /// <summary>
        /// The GetTargets.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigures"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="IDeck{BranchDeck}"/>.</returns>
        IDeck<BranchDeck> GetTargets(IFigures origin, string TargetName);

        #endregion
    }
    /// <summary>
    /// Defines the <see cref="Linker" />.
    /// </summary>
    [Serializable]
    public class Linker : ILinker
    {
        #region Fields

        private static NodeCatalog map = new NodeCatalog(new Links(), PRIMES_ARRAY.Get(9));
        private Links originLinks;
        private Links targetLinks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Linker"/> class.
        /// </summary>
        public Linker()
        {
            originLinks = new Links();
            targetLinks = new Links();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Map.
        /// </summary>
        public static NodeCatalog Map { get => map; }

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures { get; set; }

        /// <summary>
        /// Gets the OriginLinks.
        /// </summary>
        public Links OriginLinks { get => originLinks; }

        /// <summary>
        /// Gets the TargetLinks.
        /// </summary>
        public Links TargetLinks { get => targetLinks; }

        #endregion

        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        public void Clear()
        {
            Map.Flush();
        }

        /// <summary>
        /// The GetOrigin.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="BranchDeck"/>.</returns>
        public BranchDeck GetOrigin(IFigure figure, string OriginName)
        {
            return map[OriginLinkKey(figure, OriginName)];
        }

        /// <summary>
        /// The GetOriginLink.
        /// </summary>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="Link"/>.</returns>
        public Link GetOriginLink(string OriginName)
        {
            return OriginLinks[OriginName + "_" + Figures.Instant.Name];
        }

        /// <summary>
        /// The GetOriginMember.
        /// </summary>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="LinkMember"/>.</returns>
        public LinkMember GetOriginMember(string OriginName)
        {
            Link link = GetOriginLink(OriginName);
            if (link != null)
                return link.Origin;
            return null;
        }

        /// <summary>
        /// The GetOrigins.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="IDeck{BranchDeck}"/>.</returns>
        public IDeck<BranchDeck> GetOrigins(IFigures figures, string OriginName)
        {
            return new Deck<BranchDeck>(figures.Select(f => map[OriginLinkKey(f, OriginName)]).ToArray(), 255);
        }

        /// <summary>
        /// The GetTarget.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="BranchDeck"/>.</returns>
        public BranchDeck GetTarget(IFigure figure, string TargetName)
        {
            return map[TargetLinkKey(figure, TargetName)];
        }

        /// <summary>
        /// The GetTargetLink.
        /// </summary>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="Link"/>.</returns>
        public Link GetTargetLink(string TargetName)
        {
            return TargetLinks[Figures.Instant.Name + "_" + TargetName];
        }

        /// <summary>
        /// The GetTargetMember.
        /// </summary>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="LinkMember"/>.</returns>
        public LinkMember GetTargetMember(string TargetName)
        {
            Link link = GetTargetLink(TargetName);
            if (link != null)
                return link.Target;
            return null;
        }

        /// <summary>
        /// The GetTargets.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="IDeck{BranchDeck}"/>.</returns>
        public IDeck<BranchDeck> GetTargets(IFigures figures, string TargetName)
        {
            return new Deck<BranchDeck>(figures.Select(f => map[TargetLinkKey(f, TargetName)]).ToArray(), 255);
        }

        /// <summary>
        /// The OriginLinkKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public ulong OriginLinkKey(IFigure figure, string OriginName)
        {
            return GetOriginMember(OriginName).FigureLinkKey(figure);
        }

        /// <summary>
        /// The TargetLinkKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public ulong TargetLinkKey(IFigure figure, string TargetName)
        {
            return GetTargetMember(TargetName).FigureLinkKey(figure);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="LinkerExtension" />.
    /// </summary>
    public static class LinkerExtension
    {
        #region Methods

        /// <summary>
        /// The GetOriginLink.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="OriginName">The OriginName<see cref="string"/>.</param>
        /// <returns>The <see cref="Link"/>.</returns>
        public static Link GetOriginLink(this IFigures figures, string OriginName)
        {
            return Linker.Map.Links[OriginName + "_" + figures.Instant.Name];
        }

        /// <summary>
        /// The GetTargetLink.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="TargetName">The TargetName<see cref="string"/>.</param>
        /// <returns>The <see cref="Link"/>.</returns>
        public static Link GetTargetLink(this IFigures figures, string TargetName)
        {
            return Linker.Map.Links[figures.Instant.Name + "_" + TargetName];
        }

        #endregion
    }
}
