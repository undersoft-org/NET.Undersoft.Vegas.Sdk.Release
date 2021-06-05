using System.Uniques;
using System.Multemic;
using System.Collections.Generic;
using System.Linq;


namespace System.Instant.Linkmap
{
    public interface IFigureLinkmap
    {    
        IDeck<LinkBranch> CreateOriginLinks(IList<FigureLink> links = null);
        IDeck<LinkBranch> CreateTargetLinks(IList<FigureLink> links = null);

        LinkBranches CreateOriginLinks();
        LinkBranches CreateTargetLinks();

        LinkBranches Origins { get; set; }
        LinkBranches Targets { get; set; }

        void ClearLinkmap();
    }

    [JsonObject]
    [Serializable]
    public class FigureLinkmap //: //IFigureLinkmap
    {
        #region NonSerialized
        [NonSerialized] private FigureLinks targetLinks;
        [NonSerialized] private FigureLinks originLinks;
        [NonSerialized] private LinkBranches origins;
        [NonSerialized] private LinkBranches targets;
        #endregion

        public LinkBranches Origins { get => origins; set => origins = value; }
        public LinkBranches Targets   { get => targets;   set => targets = value; }

        private IFigures Figures;

        public FigureLinkmap()
        {

        }
        public FigureLinkmap(IFigures figures)
        {
            Figures = figures;
            targetLinks = new FigureLinks();
            originLinks = new FigureLinks();
            Origins = new LinkBranches(originLinks);
            Targets = new LinkBranches(targetLinks);
        }

        public FigureLinks TargetLinks
        {
            get
            {
                if (Figures.Links != null && Figures.Links.Count != (targetLinks.Count + originLinks.Count))
                {
                    targetLinks.Clear();
                    targetLinks.Add(Figures.Links.AsValues()
                                                            .Cast<FigureLink>()
                                                                .Where(v => ReferenceEquals(v.Origin.Figures, Figures))
                                                                    .OrderBy(o => o.Name).ToList());
                    originLinks.Clear();
                    originLinks.Add(Figures.Links.AsValues()
                                                            .Cast<FigureLink>()
                                                                 .Where(v => ReferenceEquals(v.Target.Figures, Figures))
                                                                    .OrderBy(o => o.Name).ToList());
                    if (targetLinks.Count != targets.Count)
                    {
                        //targets.Remove(targets.AsValues().Where(d => !targetLinks.ContainsKey(d.GetHashKey())).ToArray());
                        //targets.Put(targetLinks.AsValues()
                        //                                    .Where(d => !targets.ContainsKey(d.GetHashKey()))
                        //                                        .Cast<FigureLink>()
                        //                                            .Select(n => (IFigure)new LinkBranch(n.KeyBlock, n)));
                    }
                }
                return targetLinks;
            }
            set
            {
                targetLinks = value;
            }
        }
        public FigureLinks OriginLinks
        {
            get
            {
                if (Figures.Links != null && Figures.Links.Count != (targetLinks.Count + originLinks.Count))
                {
                    targetLinks.Clear();
                    targetLinks.Add(Figures.Links.AsValues()
                                                            .Cast<FigureLink>()
                                                                .Where(v => ReferenceEquals(v.Origin.Figures, Figures))
                                                                    .OrderBy(o => o.Name).ToList());
                    originLinks.Clear();
                    originLinks.Add(Figures.Links.AsValues()
                                                            .Cast<FigureLink>()
                                                                .Where(v => ReferenceEquals(v.Target.Figures, Figures))
                                                                    .OrderBy(o => o.Name).ToList());
                    if (originLinks.Count != origins.Count)
                    {
                        //origins.Remove(origins.AsValues().Where(d => !originLinks.ContainsKey(d.GetHashKey())));
                        //Origins.Put(originLinks.AsValues()
                        //                                    .Where(d => !origins.ContainsKey(d.GetHashKey()))
                        //                                        .Cast<FigureLink>()
                        //                                            .Select(n => (IFigure)new LinkBranch(n.KeyBlock, n)));
                    }
                }
                return originLinks;
            }
            set
            {
                originLinks = value;
            }
        }

        //public IDeck<LinkBranch> CreateTargetLinks(IList<FigureLink> links)
        //{
        //    IDeck<LinkBranch> _targets = new Album<LinkBranch>(links
        //                .Select(x => targets[x.KeyBlock].Targets = CreateTargets(Figures, x)).Cast<LinkBranch>());
        //    return _targets;
        //}
        //public LinkBranches CreateTargetLinks()
        //{
        //    TargetLinks.AsValues().Select(x => targets[x.KeyBlock].Targets = CreateTargets(Figures, (FigureLink)x)).ToArray();
        //    return targets;
        //}

        //public IDeck<LinkBranch> CreateOriginLinks(IList<FigureLink> links)
        //{
        //    IDeck<LinkBranch> _origins = new Album<LinkBranch>(links
        //             .Select(x => origins[x.KeyBlock].Origins = CreateOrigins(Figures, x)).Cast<LinkBranch>());
        //    return _origins;
        //}
        //public LinkBranches CreateOriginLinks()
        //{
        //    OriginLinks.AsValues()
        //            .Select(x => origins[x.KeyBlock].Origins = CreateOrigins(Figures, (FigureLink)x)).ToArray();
        //    return origins;
        //}

        //public LinkBranch CreateOrigins(IFigures figures, FigureLink link)
        //{
        //    IFigures targetFigures = figures;
        //    IFigures originFigures = link.Origin.Figures;

        //    int[] originKeyIndexes = link.Origin.KeyRubrics.Ordinals;
        //    int[] targetKeyIndexes = link.Target.KeyRubrics.Ordinals;

        //    LinkBranches flbs = origins[link.KeyBlock].Targets;
        //    LinkBranches lbs;

        //    if (flbs == null)
        //        lbs = new LinkBranch(link.KeyBlock, link);
        //    else
        //    {
        //        lbs = (LinkBranch)flbs;
        //        lbs.Flush();
        //    }

        //    foreach (IFigure figure in targetFigures)
        //    {
        //        lbs.Put((IUnique<IFigure>)new LinkBranch(targetKeyIndexes.Select(x => figure[x]).ToArray().GetHashKey64(), figure));
        //    }
        //    foreach (IFigure figure in originFigures)
        //    {
        //        ICard<IFigure> lb;
        //        if (lbs.TryGet(originKeyIndexes.Select(x => figure[x]).ToArray().GetHashKey64(), out lb))
        //            ((LinkBranch)lb.Value).Put(lb.Key, figure);
        //    }
        //    return lbs;
        //}
        //public LinkBranch CreateOrigins(IFigures figures, long linkHashKey)
        //{
        //    IFigure fl = OriginLinks[linkHashKey];
        //    if (fl != null)
        //        return CreateOrigins(figures, (FigureLink)fl);
        //    return null;
        //}

        //public LinkBranch CreateTargets(IFigures figures, FigureLink link)
        //{
        //    IFigures originFigures = figures;
        //    IFigures targetFigures = link.Target.Figures;
        //    link.OriginKeys.Update();
        //    link.TargetKeys.Update();
        //    LinkMember originMember = link.Origin;
        //    LinkMember targetMember = link.Target;

        //    IDeck<IFigure> targetLinkBranches = targets[link.KeyBlock].Origins;
        //    LinkBranch tlb;
        //    if (targetLinkBranches == null)
        //    {
        //        tlb = new LinkBranch(link.KeyBlock, link);
        //        targets.Put((IFigure)tlb);
        //    }
        //    else
        //    {
        //        tlb = (LinkBranch)targetLinkBranches;
        //    }

        //    IDeck<IFigure> originLinkBranches = targetFigures.Linkmap.Origins[link.KeyBlock].Origins;
        //    LinkBranch olb;
        //    if (originLinkBranches == null)
        //    {
        //        olb = new LinkBranch(link.KeyBlock, link);
        //        targetFigures.Linkmap.origins.Put((IFigure)olb);
        //    }
        //    else
        //    {
        //        olb = (LinkBranch)originLinkBranches;
        //    }
        //    FigureLinks links = originFigures.Links;
        //    foreach (FigureCard figure in originFigures.AsCards())
        //    {
        //        long key = originMember.GetLinkKey(figure);
        //        var lb = new LinkBranch(key, figure);
        //        tlb.Put(lb);
        //    }
        //    links = targetFigures.Links;
        //    foreach (FigureCard figure in targetFigures.AsCards())
        //    {
        //        long key = targetMember.GetLinkKey(figure);

        //        ICard<IFigure> lb;
        //        if (tlb.TryGet(key, out lb))
        //        {
        //            LinkBranch o = (LinkBranch)lb.Value;
        //            o.Put(lb.Key, figure);
        //        }
        //    }
        //    return tlb;
        //}
        //public LinkBranch CreateTargets(IFigures figures, long linkHashKey)
        //{
        //    IFigure fl = TargetLinks[linkHashKey];
        //    if (fl != null)
        //        return CreateTargets(figures, (FigureLink)fl);
        //    return null;
        //}

        public void ClearLinkmap(IList<FigureLink> relays)
        {
            relays.Select(r => Origins[r.KeyBlock]).Where(p => p != null).Select(s => Origins.Remove((IUnique<LinkBranch>)s)).ToArray();
            relays.Select(r => Targets[r.KeyBlock]).Where(p => p != null).Select(s => Targets.Remove((IUnique<LinkBranch>)s)).ToArray();
        }

        public void ClearLinkmap()
        {
            Origins = null;
            Targets = null;
        }
    }
}
