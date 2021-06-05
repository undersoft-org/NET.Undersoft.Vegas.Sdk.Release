/*************************************************
   Copyright (c) 2021 Undersoft

   System.ComparersExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections.Generic
{
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="HashCode32Comparer" />.
    /// </summary>
    [Serializable]
    public class HashCode32Comparer : IComparer<int>
    {
        #region Methods

        /// <summary>
        /// The Compare.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <param name="y">The y<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Compare(int x, int y)
        {
            return x - y;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(int obj)
        {
            return obj;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="HashCode32Equality" />.
    /// </summary>
    [Serializable]
    public class HashCode32Equality : IEqualityComparer<int>
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <param name="y">The y<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(int x, int y)
        {
            return x == y;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(int obj)
        {
            return obj;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="HashCode64Equality" />.
    /// </summary>
    [Serializable]
    public class HashCode64Equality : IEqualityComparer<long>
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="long"/>.</param>
        /// <param name="y">The y<see cref="long"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(long x, long y)
        {
            return x == y;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="long"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public unsafe int GetHashCode(long obj)
        {
            unchecked
            {
                byte* pkey = ((byte*)&obj);
                return (((17 + *(int*)(pkey + 4)) * 23) + *(int*)(pkey)) * 23;
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="IntArrayEquality" />.
    /// </summary>
    [Serializable]
    public class IntArrayEquality : IEqualityComparer<int[]>
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="int[]"/>.</param>
        /// <param name="y">The y<see cref="int[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(int[] x, int[] y)
        {
            return x.SequenceEqual(y);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="int[]"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(int[] obj)
        {
            unchecked
            {
                return obj.Select(o => o).Aggregate(17, (a, b) => (a + b) * 23); ;
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="ParellelHashCode32Equality" />.
    /// </summary>
    [Serializable]
    public class ParellelHashCode32Equality : IEqualityComparer<ParallelQuery<IEnumerable<int>>>
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="ParallelQuery{IEnumerable{int}}"/>.</param>
        /// <param name="y">The y<see cref="ParallelQuery{IEnumerable{int}}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(ParallelQuery<IEnumerable<int>> x, ParallelQuery<IEnumerable<int>> y)
        {
            return x.SelectMany(a => a.Select(b => b)).SequenceEqual(y.SelectMany(a => a.Select(b => b)));
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="ParallelQuery{IEnumerable{int}}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(ParallelQuery<IEnumerable<int>> obj)
        {
            unchecked
            {
                return obj.SelectMany(o => o.Select(x => x)).Aggregate(17, (a, b) => (a + b) * 23);
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="ParellelHashCode64Equality" />.
    /// </summary>
    [Serializable]
    public class ParellelHashCode64Equality : IEqualityComparer<ParallelQuery<IEnumerable<long>>>
    {
        #region Methods

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="ParallelQuery{IEnumerable{long}}"/>.</param>
        /// <param name="y">The y<see cref="ParallelQuery{IEnumerable{long}}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(ParallelQuery<IEnumerable<long>> x, ParallelQuery<IEnumerable<long>> y)
        {
            return x.SelectMany(a => a.Select(b => b)).SequenceEqual(y.SelectMany(a => a.Select(b => b)));
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="ParallelQuery{IEnumerable{long}}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(ParallelQuery<IEnumerable<long>> obj)
        {
            unchecked
            {
                return obj.SelectMany(o => o.Select(x => x)).Aggregate(17, (a, b) => (a + (int)b) * 23);
            }
        }

        #endregion
    }
}
