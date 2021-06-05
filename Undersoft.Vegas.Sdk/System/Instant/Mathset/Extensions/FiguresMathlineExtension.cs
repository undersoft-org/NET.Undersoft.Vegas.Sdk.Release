/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.FiguresMathlineExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Collections.Generic;
    using System.Instant.Mathset;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FiguresMathsetExtension" />.
    /// </summary>
    public static class FiguresMathsetExtension
    {
        #region Methods

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures Compute(this IFigures figures)
        {
            figures.Computations.Select(c => c.Compute()).ToArray();
            return figures;
        }

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="rubrics">The rubrics<see cref="IList{MemberRubric}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures Compute(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="rubricNames">The rubricNames<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures Compute(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="rubric">The rubric<see cref="MemberRubric"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures Compute(this IFigures figures, MemberRubric rubric)
        {
            IComputation ic = figures.Computations.Where(c => ((Computation)c).ContainsFirst(rubric)).FirstOrDefault();
            if (ic != null)
                ic.Compute();
            return figures;
        }

        /// <summary>
        /// The ComputeParallel.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures ComputeParallel(this IFigures figures)
        {
            figures.Computations.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }

        /// <summary>
        /// The ComputeParallel.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="rubrics">The rubrics<see cref="IList{MemberRubric}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures ComputeParallel(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }

        /// <summary>
        /// The ComputeParallel.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="rubricNames">The rubricNames<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public static IFigures ComputeParallel(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }

        #endregion
    }
}
