/*************************************************
   Copyright (c) 2021 Undersoft

   System.SortedListExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections.Generic
{
    using System;

    /// <summary>
    /// Defines the <see cref="SortedListExtension" />.
    /// </summary>
    public static class SortedListExtension
    {
        #region Methods

        /// <summary>
        /// The AddRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void AddRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The AddRangeLog.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedList{T, S}"/>.</returns>
        public static SortedList<T, S> AddRangeLog<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
                else
                    result.Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="S"/>.</returns>
        public static S Get<T, S>(this SortedList<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            S result = default(S);
            if (source.ContainsKey(key))
                result = source[key];

            return result;
        }

        /// <summary>
        /// The GetDictionary.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="SortedList{T, S}"/>.</returns>
        public static SortedList<T, S> GetDictionary<T, S>(this SortedList<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            if (source.ContainsKey(key))
                result.Add(key, source[key]);

            return result;
        }

        /// <summary>
        /// The GetRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedList{T, S}"/>.</returns>
        public static SortedList<T, S> GetRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S> result = new SortedList<T, S>();
            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    result.Add(item.Key, source[item.Key]);
            }
            return result;
        }

        /// <summary>
        /// The GetRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="List{S}"/>.</returns>
        public static List<S> GetRange<T, S>(this SortedList<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            List<S> result = new List<S>();
            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    result.Add(source[item]);
            }
            return result;
        }

        /// <summary>
        /// The GetRangeLog.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedList{T, S}[]"/>.</returns>
        public static SortedList<T, S>[] GetRangeLog<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedList<T, S>[] result = new SortedList<T, S>[2];
            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    result[0].Add(item.Key, source[item.Key]);
                else
                    result[1].Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Put<T, S>(this SortedList<T, S> source, T key, S item)
        {
            if (!source.ContainsKey(key))
                source.Add(key, item);
            else
                source[key] = item;
        }

        /// <summary>
        /// The PutRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void PutRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
                else
                    source[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// The RemoveRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void RemoveRange<T, S>(this SortedList<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    source.Remove(item.Key);
            }
        }

        /// <summary>
        /// The RemoveRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedList{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        public static void RemoveRange<T, S>(this SortedList<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    source.Remove(item);
            }
        }

        #endregion
    }
}
