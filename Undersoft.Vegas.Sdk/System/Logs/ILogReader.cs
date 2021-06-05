/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.ILogReader.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="ILogReader" />.
    /// </summary>
    public interface ILogReader
    {
        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        /// <param name="olderThen">The olderThen<see cref="DateTime"/>.</param>
        void Clear(DateTime olderThen);

        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="afterDate">The afterDate<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="LogMessage[]"/>.</returns>
        LogMessage[] Read(DateTime afterDate);

        #endregion
    }
}
