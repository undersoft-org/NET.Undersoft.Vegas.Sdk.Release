/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.LogHandler.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Instant;

    /// <summary>
    /// Defines the <see cref="LogHandler" />.
    /// </summary>
    public class LogHandler : ILogHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogHandler"/> class.
        /// </summary>
        /// <param name="writeMethod">The writeMethod<see cref="IDeputy"/>.</param>
        /// <param name="cleanMethod">The cleanMethod<see cref="IDeputy"/>.</param>
        public LogHandler(IDeputy writeMethod, IDeputy cleanMethod = null)
        {
            writer = writeMethod;
            cleaner = cleanMethod;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cleaner.
        /// </summary>
        internal IDeputy cleaner { get; set; }

        /// <summary>
        /// Gets or sets the writer.
        /// </summary>
        internal IDeputy writer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Clean.
        /// </summary>
        /// <param name="olderThen">The olderThen<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Clean(DateTime olderThen)
        {
            return (bool)cleaner.Execute(olderThen);
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="information">The information<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Write(string information)
        {
            return (bool)writer.Execute(information);
        }

        #endregion
    }
}
