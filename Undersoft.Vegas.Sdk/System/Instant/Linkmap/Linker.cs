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
    using System.Collections.Generic;
    using System.Sets;
    using System.Uniques;
    using System.Linq;

    public interface ILinker
    {
        #region Properties

        Links OriginLinks { get; }
        Links TargetLinks { get; }

        #endregion

        #region Methods

        void Clear();

        BranchDeck GetTarget(IFigure origin, string TargetName);

        BranchDeck GetOrigin(IFigure target, string OriginName);

        IDeck<BranchDeck> GetTargets(IFigures origin, string TargetName);

        IDeck<BranchDeck> GetOrigins(IFigures target, string OriginName);

        #endregion
    }

    [Serializable]
    public class Linker : ILinker
    {
        #region Fields

        private static NodeCatalog map = new NodeCatalog(new Links(), PRIMES_ARRAY.Get(9));

        private Links originLinks;
        private Links targetLinks;

        public IFigures Figures { get; set; }

        #endregion

        public Linker()
        {
            originLinks = new Links();
            targetLinks = new Links();
        }

        #region Properties

        public static NodeCatalog Map { get => map; }

        public Links OriginLinks { get => originLinks; }
        public Links TargetLinks { get => targetLinks; }

        #endregion

        #region Methods
      
        public void Clear()
        {
            Map.Flush();
        }

        public Link GetTargetLink(string TargetName)
        {
            return TargetLinks[Figures.Instant.Name + "_" + TargetName];
        }
        public Link GetOriginLink(string OriginName)
        {
            return OriginLinks[OriginName + "_" + Figures.Instant.Name];
        }

        public LinkMember GetTargetMember(string TargetName)
        {
            Link link = GetTargetLink(TargetName);
            if (link != null)
                return link.Target;
            return null;
        }
        public LinkMember GetOriginMember(string OriginName)
        {
            Link link = GetOriginLink(OriginName);
            if (link != null)
                return link.Origin;
            return null;
        }

        public ulong OriginLinkKey(IFigure figure, string OriginName)
        {
            return GetOriginMember(OriginName).FigureLinkKey(figure);
        }
        public ulong TargetLinkKey(IFigure figure, string TargetName)
        {
            return GetTargetMember(TargetName).FigureLinkKey(figure);
        }

        public BranchDeck GetTarget(IFigure figure, string TargetName)
        {
            return map[TargetLinkKey(figure, TargetName)];
        }
        public BranchDeck GetOrigin(IFigure figure, string OriginName)
        {
            return map[OriginLinkKey(figure, OriginName)];
        }

        public IDeck<BranchDeck> GetTargets(IFigures figures,string TargetName)
        {
            return new Deck<BranchDeck>(figures.Select(f => map[TargetLinkKey(f, TargetName)]).ToArray(), 255);
        }
        public IDeck<BranchDeck> GetOrigins(IFigures figures, string OriginName)
        {
            return new Deck<BranchDeck>(figures.Select(f => map[OriginLinkKey(f, OriginName)]).ToArray(), 255);
        }

        #endregion
    }

    public static class LinkerExtension
    {
        public static Link GetTargetLink(this IFigures figures, string TargetName)
        {
            return Linker.Map.Links[figures.Instant.Name + "_" + TargetName];
        }

        public static Link GetOriginLink(this IFigures figures, string OriginName)
        {
            return Linker.Map.Links[OriginName + "_" + figures.Instant.Name];
        }
    }
}
