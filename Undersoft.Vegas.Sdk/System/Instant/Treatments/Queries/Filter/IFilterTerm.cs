/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IFilterTerm.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    /// <summary>
    /// Defines the <see cref="IFilterTerm" />.
    /// </summary>
    public interface IFilterTerm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Logic.
        /// </summary>
        LogicType Logic { get; set; }

        /// <summary>
        /// Gets or sets the Operand.
        /// </summary>
        OperandType Operand { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the Stage.
        /// </summary>
        FilterStage Stage { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        object Value { get; set; }

        #endregion
    }
}
