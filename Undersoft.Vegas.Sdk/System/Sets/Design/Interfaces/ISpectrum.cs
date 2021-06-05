/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.ISpectrum.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/*********************************************************************************       
    Copyright (c) 2020 Undersoft

    System.Sets.ISpectrum
    
    @authors Darius Hanc & PhD Radek Rudek 
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                           
    
 **********************************************************************************/
namespace System.Sets
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ISpectrum{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public interface ISpectrum<V> : IEnumerable<BaseCard<V>>
    {
        #region Properties

        /// <summary>
        /// Gets the Count.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        int Size { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Add(int key, V value);

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Contains(int key);

        /// <summary>
        /// The Next.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int Next(int key);

        /// <summary>
        /// The Previous.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int Previous(int key);

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Remove(int key);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Set(int key, V value);

        #endregion
    }
}
