/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Board64_Test.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="Board64_Test" />.
    /// </summary>
    public class Board64_Test : SharedDeckTestHelper
    {
        #region Fields

        public static int threadCount = 0;
        public object holder = new object();
        public Task[] s1 = new Task[6];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Board64_Test"/> class.
        /// </summary>
        public Board64_Test() : base()
        {
            registry = new Board64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Board64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Board64_Concurrent_IndentifierKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Board64_Concurrent_IndentifierKeys_Test()
        {
            Task t = board64_MultiThread_Test(identifierKeyTestCollection);
            await t.ConfigureAwait(true);
        }

        /// <summary>
        /// The Board64_Concurrent_IntKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Board64_Concurrent_IntKeys_Test()
        {
            Task t = board64_MultiThread_Test(intKeyTestCollection);
            await t.ConfigureAwait(true);
        }

        /// <summary>
        /// The Board64_Concurrent_LongKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Board64_Concurrent_LongKeys_Test()
        {
            Task t = board64_MultiThread_Test(longKeyTestCollection);
            await t.ConfigureAwait(true);
        }

        /// <summary>
        /// The Board64_Concurrent_StringKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Board64_Concurrent_StringKeys_Test()
        {
            Task t = board64_MultiThread_Test(stringKeyTestCollection);
            await t.ConfigureAwait(true);
        }

        /// <summary>
        /// The Board64_IndentifierKeys_Test.
        /// </summary>
        [Fact]
        public void Board64_IndentifierKeys_Test()
        {
            SharedDeck_ThreadIntegrated_Test(identifierKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Board64_IntKeys_Test.
        /// </summary>
        [Fact]
        public void Board64_IntKeys_Test()
        {
            SharedDeck_ThreadIntegrated_Test(intKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Board64_LongKeys_Test.
        /// </summary>
        [Fact]
        public void Board64_LongKeys_Test()
        {
            SharedDeck_ThreadIntegrated_Test(longKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Board64_StringKeys_Test.
        /// </summary>
        [Fact]
        public void Board64_StringKeys_Test()
        {
            SharedDeck_ThreadIntegrated_Test(stringKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The board64_MultiThread_Test.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private Task board64_MultiThread_Test(IList<KeyValuePair<object, string>> collection)
        {
            registry = new Board64<string>();
            Action publicTest = () =>
            {
                int c = 0;
                lock (holder)
                    c = threadCount++;

                SharedDeck_ThreadIntegrated_Test(collection.Skip(c * 10000).Take(10000).ToArray());
            };

            for (int i = 0; i < 6; i++)
            {
                s1[i] = Task.Factory.StartNew(publicTest);

            }

            return Task.Factory.ContinueWhenAll(s1, new Action<Task[]>(a => { publicBoard_MultiThread_TCallback_Test(a); }));
        }

        /// <summary>
        /// The publicBoard_MultiThread_TCallback_Test.
        /// </summary>
        /// <param name="t">The t<see cref="Task[]"/>.</param>
        private void publicBoard_MultiThread_TCallback_Test(Task[] t)
        {
            Debug.WriteLine($"Test Finished");
        }

        #endregion
    }
}
