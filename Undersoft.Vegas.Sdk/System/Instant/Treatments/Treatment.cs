using System.Linq;
using System.Instant.Linking;

namespace System.Instant.Treatments
{
    public class Treatment
    {
        private IFigures figures;

        public Treatment(IFigures Figures)
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
                        UpdateAggregation();
                    else
                        UpdateReplication();
                }
                return aggregateRubrics;
            }
        }

        public MemberRubrics   UpdateReplication()
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
                    UpdateAggregation();
                }
                return aggregateRubrics;
            }
        }

        public MemberRubrics   UpdateAggregation()
        {
            AggregateOperand parsed = new AggregateOperand();
            Links targetLinks = figures.Linker.TargetLinks;
            aggregateRubrics = new MemberRubrics();
            MemberRubric[] _aggregateRubrics = figures.Rubrics.AsValues()
                                                               .Where(c => (c.RubricName.Split('#').Length > 1) ||
                                                                  (c.AggregateRubric != null &&
                                                                  c.AggregateOperand != AggregateOperand.None) ||
                                                                  c.AggregateOperand != AggregateOperand.None).ToArray();
            foreach (MemberRubric c in _aggregateRubrics)
            {
                c.AggregateRubric = (c.AggregateRubric != null) ? 
                                      c.AggregateRubric : 
                                     (c.AggregateOperand != AggregateOperand.None) ? 
                                            new MemberRubric(c) { RubricName = c.RubricName } :
                                            new MemberRubric(c) { RubricName = c.RubricName.Split('#')[1] };

                c.AggregateOperand = c.AggregateOperand != AggregateOperand.None ?
                                     c.AggregateOperand : 
                                     (Enum.TryParse(c.RubricName.Split('#')[0], true, out parsed)) ?
                                     parsed : AggregateOperand.None;

                c.AggregateLinkId = (targetLinks.AsValues().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                              .Where(ct => ct.RubricName == ((c.AggregateRubric != null) ?
                                              c.AggregateRubric.RubricName :
                                              c.RubricName.Split('#')[1])).Any()).Any()) ?
                                              targetLinks.AsValues().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                              .Where(ct => ct.RubricName == ((c.AggregateRubric != null) ?
                                              c.AggregateRubric.RubricName :
                                              c.RubricName.Split('#')[1])).Any()).ToArray()
                                              .Select(ix => targetLinks.IndexOf(ix)).FirstOrDefault()
                                              : -1;

                c.AggregateOrdinal = targetLinks.AsValues().Where(cr => cr.Target.Figures.Rubrics.AsValues()
                                    .Where(ct => ct.RubricName == ((c.AggregateRubric != null) ?
                                     c.AggregateRubric.RubricName :
                                     c.RubricName.Split('#')[1])).Any())
                                     .Select(cr => cr.Target.Figures.Rubrics.AsValues()
                                    .Where(ct => ct.RubricName == ((c.AggregateRubric != null) ?
                                     c.AggregateRubric.RubricName :
                                     c.RubricName.Split('#')[1]))
                                     .Select(o => o.RubricId).FirstOrDefault()).FirstOrDefault();
            }

            aggregateRubrics.Put(_aggregateRubrics);
            aggregateRubrics.AsValues().Where(j => j.AggregateLinkId > -1)
                                            .Select(p => p.AggregateLinks = 
                                               new Links(targetLinks.AsCards().Where((x, y) =>
                                                p.AggregateLinkId == x.Index).Select(v => v.Value).ToArray()));

            UpdateReplication();

            return aggregateRubrics;
        }

        private MemberRubrics  summaryRubrics;
        public  IRubrics       SummaryRubrics
        {
            get
            {
                if (summaryRubrics == null)
                {
                    UpdateSummation();
                }
                return summaryRubrics;
            }
        }

        public  MemberRubrics  UpdateSummation()
        {
            AggregateOperand parsed = new AggregateOperand();
            summaryRubrics = new MemberRubrics();
            Figure summaryFigure = new Figure(figures.Rubrics.AsValues().Where(c =>
                                               (c.RubricName.Split('=').Length > 1 || 
                                               (c.SummaryOperand != AggregateOperand.None))).Select(c =>
                                               (new MemberRubric(c)
                                               {
                                                   SummaryRubric = (c.SummaryRubric != null) ? c.SummaryRubric :
                                                                    (c.RubricName.Split('=').Length > 1) ?
                                                                    new MemberRubric(c) { RubricName = c.RubricName.Split('=')[1] } : null,
                                                   SummaryOperand = (Enum.TryParse(c.RubricName.Split('=')[0], true, out parsed)) ? parsed : c.SummaryOperand
                                               })).ToArray(), "Summary_" + figures.GetType().Name);
            figures.Summary = summaryFigure.Combine();
            summaryRubrics = (MemberRubrics)summaryFigure.Rubrics;
            return summaryRubrics;
        }
    }
}