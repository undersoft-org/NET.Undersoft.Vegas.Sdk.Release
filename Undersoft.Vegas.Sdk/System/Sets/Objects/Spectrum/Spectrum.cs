/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Spectrum.cs
   
   Data structure based on van Emde Boas tree algorithm
   with constant maximum count of items in universum defined on the beginning. 
   Innovation is that all scopes have one global cluster registry (hash deck).
   Summary scopes(sigma scopes) are also in one global hash deck. 
   Another innovation is that tree branch ends with 4 leafs (values) instead of 2
   which are encoded in to global cluster registry. Achieved complexity of
   collection is Olog^log^(n/2). For dynamic resizing collection
   inside universum are used IDeck Collections assigned by interface 
   When Safe Thread parameter is set to true Board32 is assigned
   otherwise Deck32.        

   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: PhD Radoslaw Rudek, Dariusz Hanc
   @date: (30.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Sets.Spectrum;

    /// <summary>
    /// Defines the <see cref="Spectrum{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class Spectrum<V> : ISpectrum<V>
    {
        #region Fields

        private IList<vEBTreeLevel> levels;
        private IDeck<V> registry;
        private BaseSpectrum root;
        private IDeck<BaseSpectrum> scopes;
        private IDeck<BaseSpectrum> sigmaScopes;
        private int size;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Spectrum{V}"/> class.
        /// </summary>
        public Spectrum() : this(int.MaxValue, false)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Spectrum{V}"/> class.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Spectrum(int size, bool safeThread)
        {
            Initialize(size);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Count.
        /// </summary>
        public int Count => registry.Count;

        /// <summary>
        /// Gets the IndexMax.
        /// </summary>
        public int IndexMax
        {
            get { return root.IndexMax; }
        }

        /// <summary>
        /// Gets the IndexMin.
        /// </summary>
        public int IndexMin
        {
            get { return root.IndexMin; }
        }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public int Size { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <param name="obj">The obj<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Add(int key, V obj)
        {
            if (registry.Add(key, obj))
            {
                root.Add(0, 1, 0, key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Contains(int key)
        {
            return registry.ContainsKey(key);
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="V"/>.</returns>
        public V Get(int key)
        {
            return registry.Get(key);
        }

        /// <summary>
        /// The GetEnumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{BaseCard{V}}"/>.</returns>
        public IEnumerator<BaseCard<V>> GetEnumerator()
        {
            return new SpectrumSeries<V>(this);
        }

        /// <summary>
        /// The Initialize.
        /// </summary>
        /// <param name="range">The range<see cref="int"/>.</param>
        public void Initialize(int range = 0)
        {
            scopes = new Deck64<BaseSpectrum>();
            sigmaScopes = new Deck64<BaseSpectrum>();

            if ((range == 0) || (range > int.MaxValue))
            {
                range = int.MaxValue;

                registry = new Deck64<V>();
            }
            else
            {
                registry = new Deck64<V>(range);
            }
            size = range;

            CreateLevels(range);   //create levels

            root = new ScopeValue(range, scopes, sigmaScopes, levels, 0, 0, 0);
        }

        /// <summary>
        /// The Next.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Next(int key)
        {
            return root.Next(0, 1, 0, key);
        }

        /// <summary>
        /// The Previous.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Previous(int key)
        {
            return root.Previous(0, 1, 0, key);
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Remove(int key)
        {
            if (registry.TryRemove(key))
            {
                root.Remove(0, 1, 0, key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Set(int key, V value)
        {
            return Add(key, value);
        }

        /// <summary>
        /// The TestAdd.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TestAdd(int key)
        {
            root.Add(0, 1, 0, key);
            return true;
        }

        /// <summary>
        /// The TestContains.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TestContains(int key)
        {
            return root.Contains(0, 1, 0, key);
        }

        /// <summary>
        /// The TestRemove.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TestRemove(int key)
        {
            root.Remove(0, 1, 0, key);
            return true;
        }

        /// <summary>
        /// The BuildSigmaScopes.
        /// </summary>
        /// <param name="range">The range<see cref="int"/>.</param>
        /// <param name="level">The level<see cref="byte"/>.</param>
        /// <param name="nodeTypeIndex">The nodeTypeIndex<see cref="byte"/>.</param>
        /// <param name="nodeCounter">The nodeCounter<see cref="int"/>.</param>
        /// <param name="clusterSize">The clusterSize<see cref="int"/>.</param>
        private void BuildSigmaScopes(int range, byte level, byte nodeTypeIndex, int nodeCounter, int clusterSize)
        {
            int parentSqrt = ScopeValue.ParentSqrt(range);

            if (levels == null)
            {
                levels = new List<vEBTreeLevel>();
            }
            if (levels.Count <= level)
            {
                levels.Add(new vEBTreeLevel());
            }
            if (levels[level].Scopes == null)
            {
                levels[level].Scopes = new List<vEBTreeNode>();
                levels[level].Scopes.Add(new vEBTreeNode());
            }
            else
            {
                levels[level].Scopes.Add(new vEBTreeNode());
            }

            levels[level].Scopes[nodeTypeIndex].NodeCounter = nodeCounter;
            levels[level].Scopes[nodeTypeIndex].NodeSize = parentSqrt;

            if (parentSqrt > 4)
            {
                // sigmaNode
                BuildSigmaScopes(parentSqrt, (byte)(level + 1), (byte)(2 * nodeTypeIndex), nodeCounter, parentSqrt);
                // cluster
                BuildSigmaScopes(parentSqrt, (byte)(level + 1), (byte)(2 * nodeTypeIndex + 1), nodeCounter * parentSqrt, parentSqrt);
            }
        }

        /// <summary>
        /// The CreateLevels.
        /// </summary>
        /// <param name="range">The range<see cref="int"/>.</param>
        private void CreateLevels(int range)
        {
            if (levels == null)
            {
                int parentSqrt = ScopeValue.ParentSqrt(size);
                BuildSigmaScopes(range, 0, 0, 1, parentSqrt);
            }

            int baseOffset = 0;
            for (int i = 1; i < levels.Count; i++)
            {
                levels[i].BaseOffset = baseOffset;
                for (int j = 0; j < levels[i].Scopes.Count - 1; j++)
                {
                    levels[i].Scopes[j].IndexOffset = baseOffset;
                    baseOffset += levels[i].Scopes[j].NodeCounter * levels[i].Scopes[j].NodeSize;
                }
            }
        }

        /// <summary>
        /// The GetEnumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SpectrumSeries<V>(this);
        }

        #endregion
    }
}
