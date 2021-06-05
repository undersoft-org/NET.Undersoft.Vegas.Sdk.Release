/*************************************************
   Copyright (c) 2021 Undersoft

   System.LinqExpressionExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Linq
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #region Enums

    [Serializable]
    public enum SortDirection
    {
        ASC,
        DESC
    }

    #endregion

    public static class LinqExpressionExtension
    {
        #region Methods

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.AndAlso
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static IEnumerable<T> Concentrate<T>(params IEnumerable<T>[] List)
        {
            foreach (IEnumerable<T> element in List)
            {
                foreach (T subelement in element)
                {
                    yield return subelement;
                }
            }
        }

        public static Expression<Func<TElement, bool>> ContainsIn<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        public static void Execute<TSource, TKey>(this IEnumerable<TSource> source, Action<TKey> applyBehavior, Func<TSource, TKey> keySelector)
        {
            foreach (var item in source)
            {
                var target = keySelector(item);
                applyBehavior(target);
            }
        }

        public static Expression<Func<T, bool>> Greater<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.GreaterThan
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static Expression<Func<T, bool>> GreaterOrEqual<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.GreaterThanOrEqual
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        JoinComparerProvider<TInner, TKey> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join(inner.Inner, outerKeySelector, innerKeySelector,
                              resultSelector, inner.Comparer);
        }

        public static Expression<Func<T, bool>> Less<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.LessThan
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static Expression<Func<T, bool>> LessOrEqual<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.LessThanOrEqual
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> _leftside, Expression<Func<T, bool>> _rightside)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>
                (
                Expression.OrElse
                (
                    Expression.Invoke(_leftside, param),
                    Expression.Invoke(_rightside, param)
                ),
                param
                );
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source,
        System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector,
        SortDirection sortOrder, IComparer<TKey> comparer
        )
        {
            if (sortOrder == SortDirection.ASC)
                return source.OrderBy(keySelector);
            else
                return source.OrderByDescending(keySelector);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source,
         System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector,
         SortDirection sortOrder, IComparer<TKey> comparer
         )
        {
            if (sortOrder == SortDirection.ASC)
                return source.OrderBy(keySelector);
            else
                return source.OrderByDescending(keySelector);
        }

        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        public static JoinComparerProvider<T, TKey> WithComparer<T, TKey>(
        this IEnumerable<T> inner, IEqualityComparer<TKey> comparer)
        {
            return new JoinComparerProvider<T, TKey>(inner, comparer);
        }

        private static Expression<Func<TElement, bool>> GetWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            ParameterExpression p = propertySelector.Parameters.Single();
            if (!values.Any())
                return e => false;

            var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        #endregion

        public sealed class JoinComparerProvider<T, TKey>
        {
            #region Constructors

            internal JoinComparerProvider(IEnumerable<T> inner, IEqualityComparer<TKey> comparer)
            {
                Inner = inner;
                Comparer = comparer;
            }

            #endregion

            #region Properties

            public IEqualityComparer<TKey> Comparer { get; private set; }

            public IEnumerable<T> Inner { get; private set; }

            #endregion
        }
    }
}
