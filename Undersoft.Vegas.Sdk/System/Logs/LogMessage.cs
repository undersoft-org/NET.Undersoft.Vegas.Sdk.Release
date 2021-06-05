/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.LogMessage.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="LogMessage" />.
    /// </summary>
    public class LogMessage
    {
        #region Fields

        private static long autoId = DateTime.Now.Ticks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        public LogMessage()
        {
            Id = Interlocked.Increment(ref autoId);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Millis.
        /// </summary>
        public int Millis { get; set; }

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
