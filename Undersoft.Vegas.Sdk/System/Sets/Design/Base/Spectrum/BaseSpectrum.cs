/*********************************************************************************       
    Copyright (c) 2020 Undersoft

    System.Sets.BaseSpectrum
    
    @authors Darius Hanc & PhD Radek Rudek 
    @project Undersoft.Vagas.Sdk                                    
    @version 0.8.D (Feb 7, 2020)                                           
    @licence MIT
 **********************************************************************************/

namespace System.Sets
{
    /// <summary>
    /// Defines the <see cref="BaseSpectrum" />.
    /// </summary>
    public abstract class BaseSpectrum
    {
        #region Properties

        /// <summary>
        /// Gets the IndexMax.
        /// </summary>
        public abstract int IndexMax { get; }

        /// <summary>
        /// Gets the IndexMin.
        /// </summary>
        public abstract int IndexMin { get; }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public abstract int Size { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        public abstract void Add(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        public abstract void Add(int x);

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Contains(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Contains(int x);

        /// <summary>
        /// The FirstAdd.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        public abstract void FirstAdd(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The FirstAdd.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        public abstract void FirstAdd(int x);

        /// <summary>
        /// The Next.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public abstract int Next(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The Next.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public abstract int Next(int x);

        /// <summary>
        /// The Previous.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public abstract int Previous(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The Previous.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public abstract int Previous(int x);

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="baseOffset">The baseOffset<see cref="int"/>.</param>
        /// <param name="offsetFactor">The offsetFactor<see cref="int"/>.</param>
        /// <param name="indexOffset">The indexOffset<see cref="int"/>.</param>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Remove(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Remove(int x);

        #endregion
    }
}
