/*************************************************
   Copyright (c) 2021 Undersoft

   System.IUnique.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="IUnique" />.
    /// </summary>
    public interface IUnique : IEquatable<IUnique>, IComparable<IUnique>
    {
        #region Properties

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        IUnique Empty { get; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        ulong UniqueKey { get; set; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        ulong UniqueSeed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] GetBytes();

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] GetUniqueBytes();

        #endregion
    }
}
