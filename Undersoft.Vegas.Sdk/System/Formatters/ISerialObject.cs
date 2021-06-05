/*************************************************
   Copyright (c) 2021 Undersoft

   System.ISerialObject.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="ISerialObject" />.
    /// </summary>
    public interface ISerialObject
    {
        #region Methods

        /// <summary>
        /// The Locate.
        /// </summary>
        /// <param name="path">The path<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Locate(object path = null);

        /// <summary>
        /// The Merge.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Merge(object source, string name = null);

        #endregion
    }
}
