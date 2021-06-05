/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.ISortTerm.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ISortTerm" />.
    /// </summary>
    public interface ISortTerm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Direction.
        /// </summary>
        SortDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        int RubricId { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        string RubricName { get; set; }

        #endregion
    }
}
