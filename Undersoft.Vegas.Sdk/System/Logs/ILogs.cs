/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.ILogs.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="ILogs" />.
    /// </summary>
    public interface ILogs
    {
        #region Methods

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="information">The information<see cref="string"/>.</param>
        void Write(int logLevel, Exception exception, string information = null);

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="information">The information<see cref="String"/>.</param>
        void Write(int logLevel, String information);

        #endregion
    }
}
