/*************************************************
   Copyright (c) 2021 Undersoft

   System.SortedDictionaryExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections.Generic
{
    /// <summary>
    /// Defines the <see cref="SortedDictionaryExtension" />.
    /// </summary>
    public static class SortedDictionaryExtension
    {
        #region Methods

        /// <summary>
        /// The AddDictionary.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void AddDictionary<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// The AddOrUpdate.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <param name="func">The func<see cref="Func{T, S, S}"/>.</param>
        /// <returns>The <see cref="S"/>.</returns>
        public static S AddOrUpdate<T, S>(this SortedDictionary<T, S> source, T Key, S Value, Func<T, S, S> func)
        {
            if (Key == null || Value == null)
                throw new ArgumentNullException("Collection is null");

            if (source.ContainsKey(Key))
                return source[Key] = func(Key, Value);
            else
            {
                source.Add(Key, Value);
                return Value;
            }
        }

        /// <summary>
        /// The AddOrUpdateRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void AddOrUpdateRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source.AddOrUpdate(item.Key, item.Value, (k, v) => v = item.Value);
            }
        }

        /// <summary>
        /// The AddRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void AddRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedDictionary{T, S}"/>.</returns>
        public static SortedDictionary<T, S> AddRangeLog<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="S"/>.</returns>
        public static S Get<T, S>(this SortedDictionary<T, S> source, T key)
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="SortedDictionary{T, S}"/>.</returns>
        public static SortedDictionary<T, S> GetDictionary<T, S>(this SortedDictionary<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
            if (source.ContainsKey(key))
                result.Add(key, source[key]);

            return result;
        }

        /// <summary>
        /// The GetRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedDictionary{T, S}"/>.</returns>
        public static SortedDictionary<T, S> GetRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S> result = new SortedDictionary<T, S>();
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="List{S}"/>.</returns>
        public static List<S> GetRange<T, S>(this SortedDictionary<T, S> source, IList<T> collection)
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="SortedDictionary{T, S}[]"/>.</returns>
        public static SortedDictionary<T, S>[] GetRangeLog<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            SortedDictionary<T, S>[] result = new SortedDictionary<T, S>[2];
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
        /// <param name="source">The source<see cref="SortedDictionary{T, Dictionary{T, S}}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="key2">The key2<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Put<T, S>(this SortedDictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

            if (!source.ContainsKey(key))
                source.Add(key, new Dictionary<T, S>() { { key2, item } });
            else if (!source[key].ContainsKey(key2))
                source[key].Add(key2, item);
            else
                source[key][key2] = item;
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Put<T, S>(this SortedDictionary<T, S> source, T key, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void PutRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void RemoveRange<T, S>(this SortedDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        public static void RemoveRange<T, S>(this SortedDictionary<T, S> source, IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                if (source.ContainsKey(item))
                    source.Remove(item);
            }
        }

        /// <summary>
        /// The TryAdd.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool TryAdd<T, S>(this SortedDictionary<T, S> source, T Key, S Value)
        {
            if (Key == null || Value == null)
                throw new ArgumentNullException("Collection is null");

            if (source.ContainsKey(Key))
                return false;
            else
                source.Add(Key, Value);
            return true;
        }

        /// <summary>
        /// The TryRemove.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="SortedDictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool TryRemove<T, S>(this SortedDictionary<T, S> source, T Key, out S Value)
        {
            if (Key == null)
                throw new ArgumentNullException("Collection is null");

            if (source.TryGetValue(Key, out Value))
            {
                source.Remove(Key);
                return true;
            }
            return false;
        }

        #endregion
    }
}
