/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.ICard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    /// <summary>
    /// Defines the <see cref="ICard{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public interface ICard<V> : IUnique<V>, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Extent.
        /// </summary>
        ICard<V> Extent { get; set; }

        /// <summary>
        /// Gets or sets the Index.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        ulong Key { get; set; }

        /// <summary>
        /// Gets or sets the Next.
        /// </summary>
        ICard<V> Next { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Removed.
        /// </summary>
        bool Removed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int CompareTo(ICard<V> other);

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int CompareTo(object other);

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int CompareTo(ulong key);

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Equals(ICard<V> y);

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Equals(object y);

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Equals(ulong key);

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        int GetHashCode();

        /// <summary>
        /// The GetUniqueType.
        /// </summary>
        /// <returns>The <see cref="Type"/>.</returns>
        Type GetUniqueType();

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        void Set(ICard<V> card);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        void Set(object key, V value);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        void Set(ulong key, V value);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        void Set(V value);

        #endregion
    }
}
