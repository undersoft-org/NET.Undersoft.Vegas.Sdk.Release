/*************************************************
   Copyright (c) 2021 Undersoft

   System.Logs.Logs.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (01.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Instant;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="Logs" />.
    /// </summary>
    public static class Logs
    {
        #region Fields

        private static readonly int BACK_LOG_DAYS = -1;
        private static readonly int BACK_LOG_HOURS = -1;
        private static readonly int BACK_LOG_MINUTES = -1;
        private static int _logLevel = 2;
        private static DateTime clearLogTime;
        private static Thread logger;
        private static ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
        private static bool threadLive;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="Logs"/> class.
        /// </summary>
        static Logs()
        {
            clearLogTime = DateTime.Now.AddDays(BACK_LOG_DAYS).AddHours(BACK_LOG_HOURS).AddMinutes(BACK_LOG_MINUTES);
            threadLive = true;
            logger = new Thread(new ThreadStart(logging));
            logger.Start();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        private static ILogHandler handler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="information">The information<see cref="string"/>.</param>
        public static void Add(int logLevel, Exception exception, string information = null)
        {
            if (_logLevel >= logLevel)
            {
                logQueue.Enqueue(LogFormatter.Format(logLevel, exception, information));
            }
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="information">The information<see cref="String"/>.</param>
        public static void Add(int logLevel, String information)
        {
            if (_logLevel >= logLevel)
            {
                logQueue.Enqueue(LogFormatter.Format(logLevel, information));
            }
        }

        /// <summary>
        /// The ClearLog.
        /// </summary>
        public static void ClearLog()
        {
            try
            {
                if (handler != null)
                {
                    handler.Clean(clearLogTime);
                    clearLogTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The CreateHandler.
        /// </summary>
        /// <param name="writeEvent">The writeEvent<see cref="IDeputy"/>.</param>
        /// <param name="cleanEvent">The cleanEvent<see cref="IDeputy"/>.</param>
        public static void CreateHandler(IDeputy writeEvent, IDeputy cleanEvent = null)
        {
            handler = new LogHandler(writeEvent, cleanEvent);
        }

        /// <summary>
        /// The SetLogLevel.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        public static void SetLogLevel(int logLevel)
        {
            _logLevel = logLevel;
        }

        /// <summary>
        /// The Start.
        /// </summary>
        public static void Start()
        {
            threadLive = true;
            _logLevel = 2;
            logger.Start();
        }

        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        public static void Start(int logLevel)
        {
            SetLogLevel(logLevel);
            if (!threadLive)
            {
                threadLive = true;
                logger.Start();
            }
        }

        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="int"/>.</param>
        /// <param name="writeEvent">The writeEvent<see cref="IDeputy"/>.</param>
        /// <param name="cleanEvent">The cleanEvent<see cref="IDeputy"/>.</param>
        public static void Start(int logLevel, IDeputy writeEvent, IDeputy cleanEvent = null)
        {
            CreateHandler(writeEvent, cleanEvent);
            SetLogLevel(logLevel);
            if (!threadLive)
            {
                threadLive = true;
                logger.Start();
            }
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        public static void Stop()
        {
            logger.Join();
            threadLive = false;
        }

        /// <summary>
        /// The logging.
        /// </summary>
        private static void logging()
        {
            while (threadLive)
            {
                try
                {
                    Thread.Sleep(2000);
                    int count = logQueue.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string message;
                        if (logQueue.TryDequeue(out message))
                        {
                            if (handler != null)
                                handler.Write(message);
                            else
                                Debug.WriteLine(message);
                        }
                    }
                    if (DateTime.Now.Day != clearLogTime.Day)
                    {
                        if (DateTime.Now.Hour != clearLogTime.Hour)
                        {
                            if (DateTime.Now.Minute != clearLogTime.Minute)
                            {
                                ClearLog();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}
