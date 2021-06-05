/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IComputation.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    /// <summary>
    /// Defines the <see cref="IComputation" />.
    /// </summary>
    public interface IComputation : IUnique
    {
        #region Methods

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <returns>The <see cref="IFigures"/>.</returns>
        IFigures Compute();

        #endregion
    }
}
