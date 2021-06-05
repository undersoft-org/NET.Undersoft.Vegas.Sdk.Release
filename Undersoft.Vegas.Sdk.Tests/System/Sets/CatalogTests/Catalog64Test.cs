/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Catalog64Test.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Sets;
    using System.Threading.Tasks;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="Catalog64Test" />.
    /// </summary>
    public class Catalog64Test : BaseCatalogTestHelper
    {
        #region Fields

        public static object holder = new object();
        public static int threadCount = 0;
        public Task[] s1 = new Task[10];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog64Test"/> class.
        /// </summary>
        public Catalog64Test() : base()
        {
            registry = new Catalog64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Catalog64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Catalog64_Concurrent_IndentifierKeys_TestAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Catalog64_Concurrent_IndentifierKeys_TestAsync()
        {
            await catalog64_MultiThread_Test(identifierKeyTestCollection).ConfigureAwait(true);
        }

        /// <summary>
        /// The Catalog64_Concurrent_IntKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Catalog64_Concurrent_IntKeys_Test()
        {
            await catalog64_MultiThread_Test(intKeyTestCollection).ConfigureAwait(true);
        }

        /// <summary>
        /// The Catalog64_Concurrent_LongKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Catalog64_Concurrent_LongKeys_Test()
        {
            await catalog64_MultiThread_Test(longKeyTestCollection).ConfigureAwait(true);
        }

        /// <summary>
        /// The Catalog64_Concurrent_StringKeys_Test.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Catalog64_Concurrent_StringKeys_Test()
        {
            await catalog64_MultiThread_Test(stringKeyTestCollection).ConfigureAwait(true);
        }

        /// <summary>
        /// The Catalog64_IndentifierKeys_Test.
        /// </summary>
        [Fact]
        public void Catalog64_IndentifierKeys_Test()
        {
            BaseCatalog_Integrated_Test(identifierKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Catalog64_IntKeys_Test.
        /// </summary>
        [Fact]
        public void Catalog64_IntKeys_Test()
        {
            BaseCatalog_Integrated_Test(intKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Catalog64_LongKeys_Test.
        /// </summary>
        [Fact]
        public void Catalog64_LongKeys_Test()
        {
            BaseCatalog_Integrated_Test(longKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Catalog64_StringKeys_Test.
        /// </summary>
        [Fact]
        public void Catalog64_StringKeys_Test()
        {
            BaseCatalog_Integrated_Test(stringKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The catalog64_MultiThread_TCallback_Test.
        /// </summary>
        /// <param name="t">The t<see cref="Task[]"/>.</param>
        private void catalog64_MultiThread_TCallback_Test(Task[] t)
        {
            Debug.WriteLine($"Test Finished");
        }

        /// <summary>
        /// The catalog64_MultiThread_Test.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private Task catalog64_MultiThread_Test(IList<KeyValuePair<object, string>> collection)
        {
            Action publicTest = () =>
            {
                int c = 0;
                lock (holder)
                    c = threadCount++;

                BaseCatalog_ThreadIntegrated_Test(collection.Skip(c * 10000).Take(10000).ToArray());
            };


            for (int i = 0; i < 10; i++)
            {
                s1[i] = Task.Factory.StartNew(publicTest);
            }

            return Task.Factory.ContinueWhenAll(s1, new Action<Task[]>(a => { catalog64_MultiThread_TCallback_Test(a); }));
        }

        #endregion
    }
}
