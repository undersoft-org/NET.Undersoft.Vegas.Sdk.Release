/*************************************************
   Copyright (c) 2021 Undersoft

   System.ConcurrentDictionaryExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections.Generic
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Defines the <see cref="ConcurrentDictionaryExtension" />.
    /// </summary>
    public static class ConcurrentDictionaryExtension
    {
        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="ConcurrentDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="ConcurrentDictionary{T, S}"/>.</returns>
        public static ConcurrentDictionary<T, S> Add<T, S>(this ConcurrentDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            ConcurrentDictionary<T, S> result = new ConcurrentDictionary<T, S>();
            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.AddOrUpdate(item.Key, item.Value, (k, v) => item.Value);
                else
                    result.AddOrUpdate(item.Key, item.Value, (k, v) => item.Value);
            }
            return result;
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="ConcurrentDictionary{T, Dictionary{T, S}}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="key2">The key2<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Put<T, S>(this ConcurrentDictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

            if (!source.TryAdd(key, new Dictionary<T, S>() { { key2, item } }))
            {
                if (!source[key].ContainsKey(key2))
                    source[key].Add(key2, item);
                else
                    source[key][key2] = item;
            }
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="ConcurrentDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void Put<T, S>(this ConcurrentDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source.AddOrUpdate(item.Key, item.Value, (k, v) => v = item.Value);
            }
        }

        #endregion
    }
}
