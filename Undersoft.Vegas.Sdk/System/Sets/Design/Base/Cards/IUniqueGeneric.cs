/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.IUniqueGeneric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets
{
    /// <summary>
    /// Defines the <see cref="IUnique{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public interface IUnique<V> : IUnique
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        V Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The UniqueOrdinals.
        /// </summary>
        /// <returns>The <see cref="int[]"/>.</returns>
        int[] UniqueOrdinals();

        /// <summary>
        /// The UniquesAsKey.
        /// </summary>
        /// <returns>The <see cref="ulong"/>.</returns>
        ulong UniquesAsKey();

        /// <summary>
        /// The UniqueValues.
        /// </summary>
        /// <returns>The <see cref="object[]"/>.</returns>
        object[] UniqueValues();

        #endregion
    }
}
