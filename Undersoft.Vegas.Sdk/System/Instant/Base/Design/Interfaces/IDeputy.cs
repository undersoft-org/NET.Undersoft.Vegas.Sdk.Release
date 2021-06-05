/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IDeputy.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="IDeputy" />.
    /// </summary>
    public interface IDeputy : IFigure
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Info.
        /// </summary>
        MethodInfo Info { get; set; }

        /// <summary>
        /// Gets or sets the Parameters.
        /// </summary>
        ParameterInfo[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the ParameterValues.
        /// </summary>
        object[] ParameterValues { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Execute.
        /// </summary>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Execute(params object[] parameters);

        #endregion
    }
}
