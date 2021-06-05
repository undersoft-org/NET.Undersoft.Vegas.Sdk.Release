/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureSort.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System;

    /// <summary>
    /// Defines the <see cref="FigureSort" />.
    /// </summary>
    [Serializable]
    public class FigureSort
    {
        #region Fields

        public IFigures Figures;
        public SortTerms Terms;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureSort"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        public FigureSort(IFigures figures)
        {
            this.Figures = figures;
            Terms = new SortTerms(figures);
        }

        #endregion
    }
}
