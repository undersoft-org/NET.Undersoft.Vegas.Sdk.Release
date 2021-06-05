/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IMemberRubric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    /// <summary>
    /// Defines the <see cref="IMemberRubric" />.
    /// </summary>
    public interface IMemberRubric
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Editable.
        /// </summary>
        bool Editable { get; set; }

        /// <summary>
        /// Gets or sets the RubricAttributes.
        /// </summary>
        object[] RubricAttributes { get; set; }

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        int RubricId { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the RubricOffset.
        /// </summary>
        int RubricOffset { get; set; }

        /// <summary>
        /// Gets or sets the RubricSize.
        /// </summary>
        int RubricSize { get; set; }

        /// <summary>
        /// Gets or sets the RubricType.
        /// </summary>
        Type RubricType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        bool Visible { get; set; }

        #endregion
    }
}
