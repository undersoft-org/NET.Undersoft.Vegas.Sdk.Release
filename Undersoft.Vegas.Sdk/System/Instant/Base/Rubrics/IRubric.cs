/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IRubric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;

    /// <summary>
    /// Defines the <see cref="IRubric" />.
    /// </summary>
    public interface IRubric : IMemberRubric, IUnique
    {
        #region Properties

        /// <summary>
        /// Gets or sets the AggregateLinkId.
        /// </summary>
        int AggregateLinkId { get; set; }

        /// <summary>
        /// Gets or sets the AggregateLinks.
        /// </summary>
        Links AggregateLinks { get; set; }

        /// <summary>
        /// Gets or sets the AggregateOperand.
        /// </summary>
        AggregateOperand AggregateOperand { get; set; }

        /// <summary>
        /// Gets or sets the AggregateOrdinal.
        /// </summary>
        int AggregateOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the AggregateRubric.
        /// </summary>
        IRubric AggregateRubric { get; set; }

        /// <summary>
        /// Gets or sets the IdentityOrder.
        /// </summary>
        short IdentityOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsAutoincrement.
        /// </summary>
        bool IsAutoincrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsColossus.
        /// </summary>
        bool IsColossus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDBNull.
        /// </summary>
        bool IsDBNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsIdentity.
        /// </summary>
        bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsKey.
        /// </summary>
        bool IsKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Required.
        /// </summary>
        bool Required { get; set; }

        /// <summary>
        /// Gets or sets the SummaryOperand.
        /// </summary>
        AggregateOperand SummaryOperand { get; set; }

        /// <summary>
        /// Gets or sets the SummaryOrdinal.
        /// </summary>
        int SummaryOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the SummaryRubric.
        /// </summary>
        IRubric SummaryRubric { get; set; }

        #endregion
    }
}
