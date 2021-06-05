/*************************************************
   Copyright (c) 2021 Undersoft

   System.DictionaryExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections.Generic
{
    /// <summary>
    /// Defines the <see cref="DictionaryExtension" />.
    /// </summary>
    public static class DictionaryExtension
    {
        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, Dictionary{T, S}}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="key2">The key2<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Add<T, S>(this Dictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
        {
            if (key == null || item == null)
                throw new ArgumentNullException("Collection is null");

            source.Add(key, new Dictionary<T, S>() { { key2, item } });
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="Dictionary{T, S}"/>.</param>
        public static void Add<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
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
        /// The Add.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void Add<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
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
        /// The Add.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="IDictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void Add<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection)
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
        /// The AddWithResult.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="Dictionary{T, S}"/>.</returns>
        public static Dictionary<T, S> AddWithResult<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            Dictionary<T, S> result = new Dictionary<T, S>();
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="S"/>.</returns>
        public static S Get<T, S>(this Dictionary<T, S> source, T key)
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <returns>The <see cref="Dictionary{T, S}"/>.</returns>
        public static Dictionary<T, S> GetDictionary<T, S>(this Dictionary<T, S> source, T key)
        {
            if (key == null)
                throw new ArgumentNullException("Collection is null");
            Dictionary<T, S> result = new Dictionary<T, S>();
            if (source.ContainsKey(key))
                result.Add(key, source[key]);

            return result;
        }

        /// <summary>
        /// The GetRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="Dictionary{T, S}"/>.</returns>
        public static Dictionary<T, S> GetRange<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            Dictionary<T, S> result = new Dictionary<T, S>();
            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                    result.Add(item.Key, source[item.Key]);
            }
            return result;
        }

        /// <summary>
        /// The GetRangeLog.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        /// <returns>The <see cref="Dictionary{T, S}[]"/>.</returns>
        public static Dictionary<T, S>[] GetRangeLog<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            Dictionary<T, S>[] result = new Dictionary<T, S>[2];
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
        /// The GetRangeValues.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="List{S}"/>.</returns>
        public static List<S> GetRangeValues<T, S>(this Dictionary<T, S> source, IList<T> collection)
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
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, Dictionary{T, S}}"/>.</param>
        /// <param name="key">The key<see cref="T"/>.</param>
        /// <param name="key2">The key2<see cref="T"/>.</param>
        /// <param name="item">The item<see cref="S"/>.</param>
        public static void Put<T, S>(this Dictionary<T, Dictionary<T, S>> source, T key, T key2, S item)
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void Put<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
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
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        public static void Put<T, S>(this Dictionary<T, S> source, T Key, S Value)
        {
            if (Key == null || Value == null)
                throw new ArgumentNullException("Collection is null");

            if (source.ContainsKey(Key))
                source[Key] = Value;
            else
                source.Add(Key, Value);
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <param name="func">The func<see cref="Func{T, S, S}"/>.</param>
        /// <returns>The <see cref="S"/>.</returns>
        public static S Put<T, S>(this Dictionary<T, S> source, T Key, S Value, Func<T, S, S> func)
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
        /// The PutRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void PutRange<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source.Put(item.Key, item.Value, (k, v) => v = item.Value);
            }
        }

        /// <summary>
        /// The RemoveRange.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="S">.</typeparam>
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IDictionary{T, S}"/>.</param>
        public static void RemoveRange<T, S>(this Dictionary<T, S> source, IDictionary<T, S> collection)
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="collection">The collection<see cref="IList{T}"/>.</param>
        public static void RemoveRange<T, S>(this Dictionary<T, S> source, IList<T> collection)
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool TryAdd<T, S>(this Dictionary<T, S> source, T Key, S Value)
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
        /// <param name="source">The source<see cref="Dictionary{T, S}"/>.</param>
        /// <param name="Key">The Key<see cref="T"/>.</param>
        /// <param name="Value">The Value<see cref="S"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool TryRemove<T, S>(this Dictionary<T, S> source, T Key, out S Value)
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
