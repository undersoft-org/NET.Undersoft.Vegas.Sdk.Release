/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.ISleeves.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    /// <summary>
    /// Defines the <see cref="ISleeves" />.
    /// </summary>
    public interface ISleeves : IFigures
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the Instant.
        /// </summary>
        new IInstant Instant { get; set; }

        /// <summary>
        /// Gets or sets the Sleeves.
        /// </summary>
        IFigures Sleeves { get; set; }

        #endregion
    }
}
