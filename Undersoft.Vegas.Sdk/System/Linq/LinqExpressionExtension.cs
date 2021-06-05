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

    /// <summary>
    /// Defines the <see cref="LinqExpressionExtension" />.
    /// </summary>
    public static class LinqExpressionExtension
    {
        #region Methods

        /// <summary>
        /// The And.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The Concentrate.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="List">The List<see cref="IEnumerable{T}[]"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>.</returns>
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

        /// <summary>
        /// The ContainsIn.
        /// </summary>
        /// <typeparam name="TElement">.</typeparam>
        /// <typeparam name="TValue">.</typeparam>
        /// <param name="valueSelector">The valueSelector<see cref="Expression{Func{TElement, TValue}}"/>.</param>
        /// <param name="values">The values<see cref="IEnumerable{TValue}"/>.</param>
        /// <returns>The <see cref="Expression{Func{TElement, bool}}"/>.</returns>
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

        /// <summary>
        /// The Execute.
        /// </summary>
        /// <typeparam name="TSource">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        /// <param name="source">The source<see cref="IEnumerable{TSource}"/>.</param>
        /// <param name="applyBehavior">The applyBehavior<see cref="Action{TKey}"/>.</param>
        /// <param name="keySelector">The keySelector<see cref="Func{TSource, TKey}"/>.</param>
        public static void Execute<TSource, TKey>(this IEnumerable<TSource> source, Action<TKey> applyBehavior, Func<TSource, TKey> keySelector)
        {
            foreach (var item in source)
            {
                var target = keySelector(item);
                applyBehavior(target);
            }
        }

        /// <summary>
        /// The Greater.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The GreaterOrEqual.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The Join.
        /// </summary>
        /// <typeparam name="TOuter">.</typeparam>
        /// <typeparam name="TInner">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        /// <typeparam name="TResult">.</typeparam>
        /// <param name="outer">The outer<see cref="IEnumerable{TOuter}"/>.</param>
        /// <param name="inner">The inner<see cref="JoinComparerProvider{TInner, TKey}"/>.</param>
        /// <param name="outerKeySelector">The outerKeySelector<see cref="Func{TOuter, TKey}"/>.</param>
        /// <param name="innerKeySelector">The innerKeySelector<see cref="Func{TInner, TKey}"/>.</param>
        /// <param name="resultSelector">The resultSelector<see cref="Func{TOuter, TInner, TResult}"/>.</param>
        /// <returns>The <see cref="IEnumerable{TResult}"/>.</returns>
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

        /// <summary>
        /// The Less.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The LessOrEqual.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The Or.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="_leftside">The _leftside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="_rightside">The _rightside<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
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

        /// <summary>
        /// The OrderBy.
        /// </summary>
        /// <typeparam name="TSource">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        /// <param name="source">The source<see cref="IQueryable{TSource}"/>.</param>
        /// <param name="keySelector">The keySelector<see cref="System.Linq.Expressions.Expression{Func{TSource, TKey}}"/>.</param>
        /// <param name="sortOrder">The sortOrder<see cref="SortDirection"/>.</param>
        /// <param name="comparer">The comparer<see cref="IComparer{TKey}"/>.</param>
        /// <returns>The <see cref="IOrderedQueryable{TSource}"/>.</returns>
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

        /// <summary>
        /// The ThenBy.
        /// </summary>
        /// <typeparam name="TSource">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        /// <param name="source">The source<see cref="IOrderedQueryable{TSource}"/>.</param>
        /// <param name="keySelector">The keySelector<see cref="System.Linq.Expressions.Expression{Func{TSource, TKey}}"/>.</param>
        /// <param name="sortOrder">The sortOrder<see cref="SortDirection"/>.</param>
        /// <param name="comparer">The comparer<see cref="IComparer{TKey}"/>.</param>
        /// <returns>The <see cref="IOrderedQueryable{TSource}"/>.</returns>
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

        /// <summary>
        /// The WhereIn.
        /// </summary>
        /// <typeparam name="TElement">.</typeparam>
        /// <typeparam name="TValue">.</typeparam>
        /// <param name="source">The source<see cref="IQueryable{TElement}"/>.</param>
        /// <param name="propertySelector">The propertySelector<see cref="Expression{Func{TElement, TValue}}"/>.</param>
        /// <param name="values">The values<see cref="IEnumerable{TValue}"/>.</param>
        /// <returns>The <see cref="IQueryable{TElement}"/>.</returns>
        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        /// <summary>
        /// The WhereIn.
        /// </summary>
        /// <typeparam name="TElement">.</typeparam>
        /// <typeparam name="TValue">.</typeparam>
        /// <param name="source">The source<see cref="IQueryable{TElement}"/>.</param>
        /// <param name="propertySelector">The propertySelector<see cref="Expression{Func{TElement, TValue}}"/>.</param>
        /// <param name="values">The values<see cref="TValue[]"/>.</param>
        /// <returns>The <see cref="IQueryable{TElement}"/>.</returns>
        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        /// <summary>
        /// The WithComparer.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        /// <param name="inner">The inner<see cref="IEnumerable{T}"/>.</param>
        /// <param name="comparer">The comparer<see cref="IEqualityComparer{TKey}"/>.</param>
        /// <returns>The <see cref="JoinComparerProvider{T, TKey}"/>.</returns>
        public static JoinComparerProvider<T, TKey> WithComparer<T, TKey>(
        this IEnumerable<T> inner, IEqualityComparer<TKey> comparer)
        {
            return new JoinComparerProvider<T, TKey>(inner, comparer);
        }

        /// <summary>
        /// The GetWhereInExpression.
        /// </summary>
        /// <typeparam name="TElement">.</typeparam>
        /// <typeparam name="TValue">.</typeparam>
        /// <param name="propertySelector">The propertySelector<see cref="Expression{Func{TElement, TValue}}"/>.</param>
        /// <param name="values">The values<see cref="IEnumerable{TValue}"/>.</param>
        /// <returns>The <see cref="Expression{Func{TElement, bool}}"/>.</returns>
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

        /// <summary>
        /// Defines the <see cref="JoinComparerProvider{T, TKey}" />.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="TKey">.</typeparam>
        public sealed class JoinComparerProvider<T, TKey>
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="JoinComparerProvider{T, TKey}"/> class.
            /// </summary>
            /// <param name="inner">The inner<see cref="IEnumerable{T}"/>.</param>
            /// <param name="comparer">The comparer<see cref="IEqualityComparer{TKey}"/>.</param>
            internal JoinComparerProvider(IEnumerable<T> inner, IEqualityComparer<TKey> comparer)
            {
                Inner = inner;
                Comparer = comparer;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the Comparer.
            /// </summary>
            public IEqualityComparer<TKey> Comparer { get; private set; }

            /// <summary>
            /// Gets the Inner.
            /// </summary>
            public IEnumerable<T> Inner { get; private set; }

            #endregion
        }
    }
}
