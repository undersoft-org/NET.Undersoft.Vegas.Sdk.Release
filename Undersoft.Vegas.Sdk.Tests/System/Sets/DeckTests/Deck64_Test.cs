/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Deck64_Test.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Sets;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="Deck64_Test" />.
    /// </summary>
    public class Deck64_Test : DeckTestHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck64_Test"/> class.
        /// </summary>
        public Deck64_Test() : base()
        {
            registry = new Deck64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Deck64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Deck64_IndentifierKeys_Test.
        /// </summary>
        [Fact]
        public void Deck64_IndentifierKeys_Test()
        {
            Deck_Integrated_Test(identifierKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Deck64_IntKeys_Test.
        /// </summary>
        [Fact]
        public void Deck64_IntKeys_Test()
        {
            Deck_Integrated_Test(intKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Deck64_LongKeys_Test.
        /// </summary>
        [Fact]
        public void Deck64_LongKeys_Test()
        {
            Deck_Integrated_Test(longKeyTestCollection.Take(100000).ToArray());
        }

        /// <summary>
        /// The Deck64_StringKeys_Test.
        /// </summary>
        [Fact]
        public void Deck64_StringKeys_Test()
        {
            Deck_Integrated_Test(stringKeyTestCollection.Take(100000).ToArray());
        }

        #endregion
    }
}
