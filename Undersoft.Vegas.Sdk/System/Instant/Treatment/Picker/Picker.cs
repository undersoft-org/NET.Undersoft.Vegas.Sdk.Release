﻿using System.Collections.Generic;
using System.Linq;

namespace System.Instant.Treatment
{
    
    public static class Picker
    {                          
        public static IFigures Pickout(this IFigures figures, out bool sorted, out bool filtered, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;

            filtered = (Filter.Terms.Count > 0) ? true : false;
            sorted = (Sort.Terms.Count > 0) ? true : false;
            return ResolvePick(figures, Filter, Sort, stage);
        }
        public static IFigures Pick(this IFigures figures, int stage = 1, FilterTerms filter = null, SortTerms sort = null, bool saveonly = false, bool clearonend = false)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;

            if (filter != null)
            {
                Filter.Terms.AddNewRange(filter.AsEnumerable().ToArray());
            }
            if (sort != null)
            {
                Sort.Terms.AddNewRange(sort.AsEnumerable().ToArray());
            }
            if (!saveonly)
            {
                IFigures result = ResolvePick(figures, Filter, Sort, stage);
                if (clearonend)
                {
                    figures.Filter.Terms.Clear();
                    figures.Filter.Evaluator = null;
                    figures.Picked.Filter = null;
                }
                return result;
            }
            return null;
        }
        public static IFigures Pick(this IFigures figures, List<FilterTerm> filterList, List<SortTerm> sortList, bool saveonly = false, bool clearonend = false, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;
            if (filterList != null)
            {
                Filter.Terms.AddNewRange(filterList);
            }
            if (sortList != null)
            {
                Sort.Terms.AddNewRange(sortList);
            }
            if (!saveonly)
            {
                IFigures result = ResolvePick(figures, Filter, Sort, stage);
                if (clearonend)
                {
                    figures.Filter.Terms.Clear();
                    figures.Filter.Evaluator = null;
                    figures.Picked.Filter = null;
                }
                return result;
            }
            return null;
        }
        public static IFigures Pick(this IFigures figures, IFigure[] appendfigures, int stage = 1)
        {
            FigureFilter Filter = figures.Filter;
            FigureSort Sort = figures.Sort;
            return ResolvePick(figures, Filter, Sort, stage, appendfigures);
        }

        public static IFigure[] Pick(this IFigure[] figureArray, Func<IFigure, bool> evaluator)
        {
            return figureArray.Where(evaluator).ToArray();
        }
        public static IFigures Pick(this IFigures figures, Func<IFigure, bool> evaluator)
        {
            IFigures view = figures.Picked = (IFigures)figures.Type.New();
            view.Add(figures.AsEnumerable().AsQueryable().Where(evaluator));
            return view;
        }

        private static IFigures ResolvePick(IFigures figures, FigureFilter Filter, FigureSort Sort, int stage = 1, IFigure[] appendfigures = null)
        {
            FilterStage filterStage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
            int filtercount = Filter.Terms.AsEnumerable().Where(f => f.Stage.Equals(filterStage)).ToArray().Length;
            int sortcount = Sort.Terms.Count;

            if (filtercount > 0)
                if (sortcount > 0)
                    return ExecutePick(figures, Filter, Sort, stage, appendfigures);
                else
                    return ExecutePick(figures, Filter, null, stage, appendfigures);
            else if (sortcount > 0)
                return ExecutePick(figures, null, Sort, stage, appendfigures);
            else
                return ExecutePick(figures, null, null, stage, appendfigures);

        }
       
        private static IFigures ExecutePick(IFigures _figures, FigureFilter filter, FigureSort sort, int stage = 1, IFigure[] appendfigures = null)
        {
            IFigures table = _figures;
            IFigures figures = null;
            IFigures view = _figures.Picked;           

            if (appendfigures == null)
                if (stage > 1)
                    figures = view;
                else if (stage < 0)
                {
                    figures = _figures;
                    view = _figures.Picked = (IFigures)_figures.Type.New();
                }
                else
                {
                    figures = table;
                }

            if (filter != null && filter.Terms.Count > 0)
            {
                filter.Evaluator = filter.GetExpression(stage).Compile();
                view.Picker = filter.Evaluator;

                if (sort != null && sort.Terms.Count > 0)
                {
                    bool isFirst = true;
                    IEnumerable<IFigure> tsrt = null;
                    IOrderedQueryable<IFigure> ttby = null;
                    if (appendfigures != null)
                        tsrt = appendfigures.AsEnumerable().Where(filter.Evaluator);
                    else
                        tsrt = figures.AsEnumerable().Where(filter.Evaluator);

                    foreach (SortTerm fcs in sort.Terms)
                    {
                        if (isFirst)
                            ttby = tsrt.AsQueryable().OrderBy(o => o[fcs.RubricName], fcs.Direction, Comparer<object>.Default);
                        else
                            ttby = ttby.ThenBy(o => o[fcs.RubricName], fcs.Direction, Comparer<object>.Default);
                        isFirst = false;
                    }

                    if (appendfigures != null)
                        view.Add(ttby.ToArray());
                    else
                    {
                        view.Clear();
                        view.Add(ttby.ToArray());
                    }
                    
                }
                else
                {
                    if (appendfigures != null)
                        view.Add(appendfigures.AsQueryable().Where(filter.Evaluator).ToArray());
                    else
                    {
                        view.Clear();
                        view.Add(figures.AsEnumerable().AsQueryable().Where(filter.Evaluator).ToArray());
                    }
                }
            }
            else if (sort != null && sort.Terms.Count > 0)
            {
                view.Filter = null;
                view.Filter.Evaluator = null;

                bool isFirst = true;
                IOrderedQueryable<IFigure> ttby = null;
            
                foreach (SortTerm fcs in sort.Terms)
                {
                    if (isFirst)
                        if (appendfigures != null)
                            ttby = appendfigures.AsQueryable().OrderBy(o => o[fcs.RubricName], fcs.Direction, Comparer<object>.Default);
                        else
                            ttby = figures.AsEnumerable().AsQueryable().OrderBy(o => o[fcs.RubricName], fcs.Direction, Comparer<object>.Default);
                    else
                        ttby = ttby.ThenBy(o => o[fcs.RubricName], fcs.Direction, Comparer<object>.Default);

                    isFirst = false;
                }

                if (appendfigures != null)
                    view.Add(ttby);
                else
                    view.Add(ttby);
            }
            else
            {
                if (stage < 2)
                {
                    view.Filter = null;
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
                table.Picked = view;
            }
            return view;
        }                
    }
}
