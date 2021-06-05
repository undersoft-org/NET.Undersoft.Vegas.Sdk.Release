/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.LogFormatter.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="LogFormatter" />.
    /// </summary>
    public static class LogFormatter
    {
        #region Methods

        /// <summary>
        /// The Format.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="information">The information<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Format(int logLevel, Exception exception, string information = null)
        {
            return $"{logLevel.ToString()}" + "#Exception#" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                                         + "#" + DateTime.Now.Millisecond.ToString()
                                                                         + "#" + exception.Message
                                                                         + "\r\n" + exception.Source
                                                                         + "\r\n" + exception.StackTrace
                                                                         + ((information != null) ? "\r\n"
                                                                         + "#Information#" + information : "");
        }

        /// <summary>
        /// The Format.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="information">The information<see cref="String"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Format(int logLevel, String information)
        {
            return $"{logLevel.ToString()}#Information#{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}#"
                                                   + $"{DateTime.Now.Millisecond.ToString()}#"
                                                   + $"{information}";
        }

        #endregion
    }
}
