/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Queryator.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Queries
    {
        #region Methods

        public static IFigure[] Query(this IFigure[] figureArray, Func<IFigure, bool> queryFormula)
        {
            return figureArray.Where(queryFormula).ToArray();
        }

        public static IFigures Query(this IFigures figures, Func<IFigure, bool> queryFormula)
        {
            IFigures view = figures.View;
            view.Flush();
            view.Add(figures.Where(queryFormula));
            return view;
        }

        public static IFigures Query(this IFigures figures, IFigure[] appendfigures, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;
            return ResolveQuery(figures, Filter, Sort, stage, appendfigures);
        }

        public static IFigures Query(this IFigures figures, int stage = 1, FilterTerms filter = null, SortTerms sort = null, bool saveonly = false, bool clearonend = false)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;

            if (filter != null)
            {
                Filter.Terms.Renew(filter.ToArray());
            }
            if (sort != null)
            {
                Sort.Terms.Renew(sort.ToArray());
            }
            if (!saveonly)
            {
                IFigures result = ResolveQuery(figures, Filter, Sort, stage);
                if (clearonend)
                {
                    figures.Filter.Terms.Clear();
                    figures.Filter.Evaluator = null;
                    figures.View.QueryFormula = null;
                }
                return result;
            }
            return null;
        }

        public static IFigures Query(this IFigures figures, IList<FilterTerm> filterList, IList<SortTerm> sortList, bool saveonly = false, bool clearonend = false, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;
            if (filterList != null)
            {
                Filter.Terms.Renew(filterList);
            }
            if (sortList != null)
            {
                Sort.Terms.Renew(sortList);
            }
            if (!saveonly)
            {
                IFigures result = ResolveQuery(figures, Filter, Sort, stage);
                if (clearonend)
                {
                    figures.Filter.Terms.Clear();
                    figures.Filter.Evaluator = null;
                    figures.View.QueryFormula = null;
                }
                return result;
            }
            return null;
        }

        public static IFigures Query(this IFigures figures, out bool sorted, out bool filtered, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;

            filtered = (Filter.Terms.Count > 0) ? true : false;
            sorted = (Sort.Terms.Count > 0) ? true : false;
            return ResolveQuery(figures, Filter, Sort, stage);
        }

        private static IFigures ExecuteQuery(IFigures figures, FigureFilter filter, FigureSort sort, int stage = 1, IFigure[] appendfigures = null)
        {
            IFigures table = figures;
            IFigures _figures = null;
            IFigures view = figures.View;

            if (appendfigures == null)
                if (stage > 1)
                    _figures = view;
                else if (stage < 0)
                {
                    _figures = figures;
                    view = figures.View;
                    view.Flush();
                }
                else
                {
                    _figures = table;
                }

            if (filter != null && filter.Terms.Count > 0)
            {
                filter.Evaluator = filter.GetExpression(stage).Compile();
                view.QueryFormula = filter.Evaluator;

                if (sort != null && sort.Terms.Count > 0)
                {
                    bool isFirst = true;
                    IEnumerable<IFigure> ief = null;
                    IOrderedQueryable<IFigure> ioqf = null;
                    if (appendfigures != null)
                        ief = appendfigures.Where(filter.Evaluator);
                    else
                        ief = _figures.Where(filter.Evaluator);

                    foreach (SortTerm fcs in sort.Terms)
                    {
                        if (isFirst)
                            ioqf = ief
                                    .AsQueryable()
                                            .OrderBy(o => 
                                                    o[fcs.RubricName], 
                                                    fcs.Direction, 
                                                    Comparer<object>.Default);
                        else
                            ioqf = ioqf
                                    .ThenBy(o => 
                                            o[fcs.RubricName], 
                                            fcs.Direction, 
                                            Comparer<object>.Default);
                        isFirst = false;
                    }

                    if (appendfigures != null)
                        view.Add(ioqf.ToArray());
                    else
                    {
                        view.Flush();
                        view.Add(ioqf.ToArray());
                    }

                }
                else
                {
                    if (appendfigures != null)
                        view.Add(appendfigures.Where(filter.Evaluator).ToArray());
                    else
                    {
                        view.Flush();
                        view.Add(figures.Where(filter.Evaluator).ToArray());
                    }
                }
            }
            else if (sort != null && sort.Terms.Count > 0)
            {
                view.QueryFormula = null;
                view.Filter.Evaluator = null;

                bool isFirst = true;
                IOrderedQueryable<IFigure> ioqf = null;

                foreach (SortTerm fcs in sort.Terms)
                {
                    if (isFirst)
                        if (appendfigures != null)
                            ioqf = appendfigures
                                        .AsQueryable()
                                                .OrderBy(o => 
                                                         o[fcs.RubricName], 
                                                         fcs.Direction, 
                                                         Comparer<object>.Default);
                        else
                            ioqf = _figures
                                        .AsQueryable()
                                                .OrderBy(o => 
                                                         o[fcs.RubricName], 
                                                         fcs.Direction, 
                                                         Comparer<object>.Default);
                    else
                        ioqf = ioqf
                                 .ThenBy(o => 
                                         o[fcs.RubricName], 
                                         fcs.Direction, 
                                         Comparer<object>.Default);

                    isFirst = false;
                }

                if (appendfigures != null)
                    view.Add(ioqf);
                else
                    view.Add(ioqf);
            }
            else
            {
                if (stage < 2)
                {
                    view.QueryFormula = null;
                    view.Filter.Evaluator = null;
                }

                if (appendfigures != null)
                    view.Add(appendfigures);
                else
                    view.Add(figures);
            }

            //  view.PagingDetails.ComputePageCount(view.Count);
            if (stage > 0)
            {
                table.View = view;
            }
            return view;
        }

        private static IFigures ResolveQuery(IFigures figures, FigureFilter Filter, FigureSort Sort, int stage = 1, IFigure[] appendfigures = null)
        {
            FilterStage filterStage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
            int filtercount = Filter.Terms.Where(f => f.Stage.Equals(filterStage)).ToArray().Length;
            int sortcount = Sort.Terms.Count;

            if (filtercount > 0)
                if (sortcount > 0)
                    return ExecuteQuery(figures, Filter, Sort, stage, appendfigures);
                else
                    return ExecuteQuery(figures, Filter, null, stage, appendfigures);
            else if (sortcount > 0)
                return ExecuteQuery(figures, null, Sort, stage, appendfigures);
            else
                return ExecuteQuery(figures, null, null, stage, appendfigures);
        }

        #endregion
    }
}
