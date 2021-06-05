/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Album64_Test.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Tests
{
    using System.Diagnostics;
    using System.Linq;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="Album64_Test" />.
    /// </summary>
    public class Album64_Test : AlbumTestHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Album64_Test"/> class.
        /// </summary>
        public Album64_Test() : base()
        {
            registry = new Album64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Album64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Album64_IndentifierKeys_Test.
        /// </summary>
        [Fact]
        public void Album64_IndentifierKeys_Test()
        {
            Album_Integrated_Test(identifierKeyTestCollection.Take(250000).ToArray());
        }

        /// <summary>
        /// The Album64_IntKeys_Test.
        /// </summary>
        [Fact]
        public void Album64_IntKeys_Test()
        {
            Album_Integrated_Test(intKeyTestCollection.Take(250000).ToArray());
        }

        /// <summary>
        /// The Album64_LongKeys_Test.
        /// </summary>
        [Fact]
        public void Album64_LongKeys_Test()
        {
            Album_Integrated_Test(longKeyTestCollection.Take(250000).ToArray());
        }

        /// <summary>
        /// The Album64_StringKeys_Test.
        /// </summary>
        [Fact]
        public void Album64_StringKeys_Test()
        {
            Album_Integrated_Test(stringKeyTestCollection.Take(250000).ToArray());
        }

        #endregion
    }
}
