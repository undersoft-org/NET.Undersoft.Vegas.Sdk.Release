/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.ILogHandler.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="ILogHandler" />.
    /// </summary>
    public interface ILogHandler
    {
        #region Methods

        /// <summary>
        /// The Clean.
        /// </summary>
        /// <param name="olderThen">The olderThen<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Clean(DateTime olderThen);

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="information">The information<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Write(string information);

        #endregion
    }
}
