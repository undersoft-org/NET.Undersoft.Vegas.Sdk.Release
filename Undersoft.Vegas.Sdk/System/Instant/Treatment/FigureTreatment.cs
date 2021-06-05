using System.Linq;
using System.Instant.Linkmap;

namespace System.Instant.Treatment
{
    public class FigureTreatment
    {
        private IFigures figures;

        public FigureTreatment(IFigures Figures)
        {
            figures = Figures;
        }

        private MemberRubrics  replicateRubrics;
        public  MemberRubrics  ReplicateRubrics
        {
            get
            {
                if (replicateRubrics == null)
                {
                    if (aggregateRubrics == null)
                        AggregateRubricsUpdate();
                    else
                        ReplicateRubricsUpdate();
                }
                return replicateRubrics;
            }
        }

        public MemberRubrics   ReplicateRubricsUpdate()
        {
            replicateRubrics = new MemberRubrics();
            replicateRubrics.Put(aggregateRubrics.AsValues().Where(p => p.AggregateOperand == AggregateOperand.Bind));
            return replicateRubrics;
        }

        private MemberRubrics  aggregateRubrics;
        public  MemberRubrics  AggregateRubrics
        {
            get
            {
                if (aggregateRubrics == null)
                {
                    AggregateRubricsUpdate();
                }
                return aggregateRubrics;
            }
        }

        public MemberRubrics   AggregateRubricsUpdate()
        {
            AggregateOperand parsed = new AggregateOperand();
            FigureLinks targetLinks = figures.Linkmap.TargetLinks;
            aggregateRubrics = new MemberRubrics();
            MemberRubric[] _aggregateRubrics = figures.Rubrics.AsValues()
                                                               .Where(c => (c.RubricName.Split('#').Length > 1) ||
                                                                  (c.AggregatePattern != null &&
                                                                  c.AggregateOperand != AggregateOperand.None) ||
                                                                  c.AggregateOperand != AggregateOperand.None).ToArray();
            foreach (MemberRubric c in _aggregateRubrics)
            {
                c.AggregatePattern = (c.AggregatePattern != null) ? c.AggregatePattern : (c.AggregateOperand != AggregateOperand.None) ? new MemberRubric(c) { RubricName = c.RubricName } : new MemberRubric(c) { RubricName = c.RubricName.Split('#')[1] };
                c.AggregateOperand = c.AggregateOperand != AggregateOperand.None ? c.AggregateOperand : (Enum.TryParse(c.RubricName.Split('#')[0], true, out parsed)) ? parsed : AggregateOperand.None;
                c.AggregateIndex = (targetLinks.Cast<FigureLink>().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                              .Where(ct => ct.RubricName == ((c.AggregatePattern != null) ?
                                              c.AggregatePattern.RubricName :
                                              c.RubricName.Split('#')[1])).Any()).Any()) ?
                             targetLinks.Cast<FigureLink>().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                              .Where(ct => ct.RubricName == ((c.AggregatePattern != null) ?
                                              c.AggregatePattern.RubricName :
                                              c.RubricName.Split('#')[1])).Any()).ToArray().Select(ix => targetLinks.IndexOf(ix)).ToArray()
                                              : null;
                c.AggregateOrdinal = targetLinks.Cast<FigureLink>().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                    .Where(ct => ct.RubricName == ((c.AggregatePattern != null) ?
                                     c.AggregatePattern.RubricName :
                                     c.RubricName.Split('#')[1])).Any())
                                     .Select(cr => cr.Target.Figures.Rubrics.AsValues()
                                    .Where(ct => ct.RubricName == ((c.AggregatePattern != null) ?
                                     c.AggregatePattern.RubricName :
                                     c.RubricName.Split('#')[1]))
                                     .Select(o => o.RubricId).FirstOrDefault()).ToArray();
            }

            aggregateRubrics.Put(_aggregateRubrics);
            aggregateRubrics.AsValues().Where(j => j.AggregateIndex != null).Select(p => p.AggregateLinks = new FigureLinks(targetLinks.Cast<FigureLink>().Where((x, y) => p.AggregateIndex.Contains(y)).ToArray()));

            ReplicateRubricsUpdate();

            return aggregateRubrics;
        }

        private MemberRubrics  summaryRubrics;
        public  IRubrics  SummaryRubrics
        {
            get
            {
                if (summaryRubrics == null)
                {
                    SummaryRubricsUpdate();
                }
                return summaryRubrics;
            }
        }

        public  MemberRubrics  SummaryRubricsUpdate()
        {
            AggregateOperand parsed = new AggregateOperand();
            summaryRubrics = new MemberRubrics();
            Figure summaryFigure = new Figure(figures.Rubrics.AsValues().Where(c =>
                                               (c.RubricName.Split('=').Length > 1 || 
                                               (c.SummaryOperand != AggregateOperand.None))).Select(c =>
                                               (new MemberRubric(c)
                                               {
                                                   SummaryPattern = (c.SummaryPattern != null) ? c.SummaryPattern :
                                                                    (c.RubricName.Split('=').Length > 1) ?
                                                                    new MemberRubric(c) { RubricName = c.RubricName.Split('=')[1] } : null,
                                                   SummaryOperand = (Enum.TryParse(c.RubricName.Split('=')[0], true, out parsed)) ? parsed : c.SummaryOperand
                                               })).ToArray(), "Summary_" + figures.GetType().Name);
            figures.Summary = summaryFigure.Generate();
            summaryRubrics = (MemberRubrics)summaryFigure.Rubrics;
            return summaryRubrics;
        }
    }
}