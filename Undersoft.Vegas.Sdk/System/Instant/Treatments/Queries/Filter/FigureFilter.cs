/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureFilter.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="FigureFilter" />.
    /// </summary>
    [Serializable]
    public class FigureFilter
    {
        #region Fields

        [NonSerialized] public Func<IFigure, bool> Evaluator;
        [NonSerialized] private QueryExpression expression;
        [NonSerialized] private IFigures figures;
        [NonSerialized] private FilterTerms termsBuffer;
        [NonSerialized] private FilterTerms termsReducer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureFilter"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        public FigureFilter(IFigures figures)
        {
            this.figures = figures;
            expression = new QueryExpression();
            Reducer = new FilterTerms(figures);
            Terms = new FilterTerms(figures);
            termsBuffer = expression.Conditions;
            termsReducer = new FilterTerms(figures);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        /// <summary>
        /// Gets or sets the Reducer.
        /// </summary>
        public FilterTerms Reducer { get; set; }

        /// <summary>
        /// Gets or sets the Terms.
        /// </summary>
        public FilterTerms Terms { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The GetExpression.
        /// </summary>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        /// <returns>The <see cref="Expression{Func{IFigure, bool}}"/>.</returns>
        public Expression<Func<IFigure, bool>> GetExpression(int stage = 1)
        {
            termsReducer.Clear();
            termsReducer.Add(Reducer.AsEnumerable().Concat(Terms.AsEnumerable()).ToArray());
            expression.Conditions = termsReducer;
            termsBuffer = termsReducer;
            return expression.CreateExpression(stage);
        }

        /// <summary>
        /// The Query.
        /// </summary>
        /// <param name="toQuery">The toQuery<see cref="ICollection{IFigure}"/>.</param>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        /// <returns>The <see cref="IFigure[]"/>.</returns>
        public IFigure[] Query(ICollection<IFigure> toQuery, int stage = 1)
        {
            termsReducer.Clear();
            termsReducer.Add(Reducer.AsEnumerable().Concat(Terms.AsEnumerable()).ToArray());
            expression.Conditions = termsReducer;
            termsBuffer = termsReducer;
            return toQuery.AsQueryable().Where(expression.CreateExpression(stage).Compile()).ToArray();
        }

        /// <summary>
        /// The Query.
        /// </summary>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        /// <returns>The <see cref="IFigure[]"/>.</returns>
        public IFigure[] Query(int stage = 1)
        {
            termsReducer.Clear();
            termsReducer.Add(Reducer.AsEnumerable().Concat(Terms.AsEnumerable()).ToArray());
            expression.Conditions = termsReducer;
            termsBuffer = termsReducer;
            return figures.AsEnumerable().AsQueryable().Where(expression.CreateExpression(stage).Compile()).ToArray();
        }

        #endregion
    }
}
