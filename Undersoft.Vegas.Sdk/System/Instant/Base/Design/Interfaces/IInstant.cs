/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IInstant.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    /// <summary>
    /// Defines the <see cref="IInstant" />.
    /// </summary>
    public interface IInstant
    {
        #region Properties

        /// <summary>
        /// Gets or sets the BaseType.
        /// </summary>
        Type BaseType { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the Rubrics.
        /// </summary>
        IRubrics Rubrics { get; }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        Type Type { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The New.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        object New();

        #endregion
    }
}
