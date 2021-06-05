using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Data;

namespace System.Instant.Treatments
{
    [Serializable]
    public class FigureSort
    {
        public IFigures Figures;

        public SortTerms Terms;

        public FigureSort(IFigures figures)
        {
            this.Figures = figures;
            Terms = new SortTerms(figures);
        }

    }
}
