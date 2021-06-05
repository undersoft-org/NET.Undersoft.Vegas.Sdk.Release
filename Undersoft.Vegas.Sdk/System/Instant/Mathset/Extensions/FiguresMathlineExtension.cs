using System.Linq;
using System.Collections.Generic;
using System.Instant.Mathset;

namespace System.Instant
{      
    public static class FiguresMathsetExtension
    {
        public static IFigures Compute(this IFigures figures)
        {
            figures.Computations.Select(c => c.Compute()).ToArray();
            return figures;
        }
        public static IFigures Compute(this IFigures figures, MemberRubric rubric)
        {
            IComputation ic = figures.Computations.Where(c => ((Computation)c).ContainsFirst(rubric)).FirstOrDefault();
            if (ic != null)
                ic.Compute();
            return figures;
        }
        public static IFigures Compute(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }
        public static IFigures Compute(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }

        public static IFigures ComputeParallel(this IFigures figures)
        {
            figures.Computations.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }
        public static IFigures ComputeParallel(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }
        public static IFigures ComputeParallel(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }
    }
 
}
