/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.SharedDeckTestHelper.cs.Tests
   
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
    using System.Sets;
    using System.Threading;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="SharedDeckTestHelper" />.
    /// </summary>
    public class SharedDeckTestHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedDeckTestHelper"/> class.
        /// </summary>
        public SharedDeckTestHelper()
        {
            stringKeyTestCollection = PrepareTestListings.prepareStringKeyTestCollection();
            intKeyTestCollection = PrepareTestListings.prepareIntKeyTestCollection();
            longKeyTestCollection = PrepareTestListings.prepareLongKeyTestCollection();
            identifierKeyTestCollection = PrepareTestListings.prepareIdentifierKeyTestCollection();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifierKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> identifierKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the intKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> intKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the longKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> longKeyTestCollection { get; set; }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        public IDeck<string> registry { get; set; }

        /// <summary>
        /// Gets or sets the stringKeyTestCollection.
        /// </summary>
        public IList<KeyValuePair<object, string>> stringKeyTestCollection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The SharedDeck_Integrated_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        public void SharedDeck_Integrated_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            // registry = new Deck<string>();

            Deck_Add_Test(testCollection);
            Deck_Count_Test(100000);
            Deck_First_Test(testCollection[0].Value);
            Deck_Last_Test(testCollection[99999].Value);
            Deck_Get_Test(testCollection);
            Deck_GetCard_Test(testCollection);
            Deck_Remove_Test(testCollection);
            Deck_Count_Test(70000);
            Deck_Enqueue_Test(testCollection);
            Deck_Count_Test(70005);
            Deck_Dequeue_Test(testCollection);
            Deck_Contains_Test(testCollection);
            Deck_ContainsKey_Test(testCollection);
            Deck_Put_Test(testCollection);
            Deck_Count_Test(100000);
            Deck_Clear_Test();
            Deck_Add_V_Test(testCollection);
            Deck_Count_Test(100000);
            Deck_Remove_V_Test(testCollection);
            Deck_Count_Test(70000);
            Deck_Put_V_Test(testCollection);
            Deck_IndexOf_Test(testCollection);
            Deck_GetByIndexer_Test(testCollection);
            Deck_Count_Test(100000);
        }

        /// <summary>
        /// The SharedDeck_ThreadIntegrated_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        public void SharedDeck_ThreadIntegrated_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            SharedDeck_Add_Test(testCollection);
            SharedDeck_Get_Test(testCollection);
            SharedDeck_GetCard_Test(testCollection);
            SharedDeck_Remove_Test(testCollection);
            //  SharedDeck_Enqueue_Test(testCollection);
            //  SharedDeck_Dequeue_Test(testCollection);
            SharedDeck_Contains_Test(testCollection);
            SharedDeck_ContainsKey_Test(testCollection);
            SharedDeck_Put_Test(testCollection);
            //  SharedDeck_Add_V_Test(testCollection);
            //  SharedDeck_Remove_V_Test(testCollection);
            //  SharedDeck_Put_V_Test(testCollection);
            SharedDeck_GetByIndexer_Test(testCollection);

            Debug.WriteLine($"Thread no { testCollection[0].Key }_{ registry.Count } ends");

            Thread.Sleep(1000);
        }

        /// <summary>
        /// The Deck_Add_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Add_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The Deck_Add_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Add_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Value);
            }
        }

        /// <summary>
        /// The Deck_Clear_Test.
        /// </summary>
        private void Deck_Clear_Test()
        {
            registry.Clear();
            Assert.Empty(registry);
        }

        /// <summary>
        /// The Deck_Contains_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Contains_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.Contains(registry.NewCard(item.Key, item.Value)))
                    items.Add(true);
            }
            Assert.Equal(70000, items.Count);
        }

        /// <summary>
        /// The Deck_ContainsKey_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_ContainsKey_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.ContainsKey(item.Key))
                    items.Add(true);
            }
            Assert.Equal(70000, items.Count);
        }

        /// <summary>
        /// The Deck_CopyTo_Test.
        /// </summary>
        private void Deck_CopyTo_Test()
        {
        }

        /// <summary>
        /// The Deck_Count_Test.
        /// </summary>
        /// <param name="count">The count<see cref="int"/>.</param>
        private void Deck_Count_Test(int count)
        {
            Assert.Equal(count, registry.Count);
        }

        /// <summary>
        /// The Deck_Dequeue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Dequeue_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string output = null;
                if (registry.TryDequeue(out output))
                    items.Add(output);
            }
            Assert.Equal(5, items.Count);
        }

        /// <summary>
        /// The Deck_Enqueue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Enqueue_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection.Skip(70000).Take(5))
            {
                if (registry.Enqueue(item.Key, item.Value))
                    items.Add(true);
            }
            Assert.Equal(5, items.Count);
        }

        /// <summary>
        /// The Deck_First_Test.
        /// </summary>
        /// <param name="firstValue">The firstValue<see cref="string"/>.</param>
        private void Deck_First_Test(string firstValue)
        {
            Assert.Equal(registry.First.Next.Value, firstValue);
        }

        /// <summary>
        /// The Deck_Get_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Get_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection)
            {
                string r = registry.Get(item.Key);
                if (r != null)
                    items.Add(r);
            }
            Assert.Equal(100000, items.Count);
        }

        /// <summary>
        /// The Deck_GetByIndexer_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_GetByIndexer_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            int i = 0;
            foreach (var item in testCollection.Take(1000))
            {
                string a = registry[i];
                string b = item.Value;
                i++;
            }
        }

        /// <summary>
        /// The Deck_GetCard_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_GetCard_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<ICard<string>> items = new List<ICard<string>>();
            foreach (var item in testCollection)
            {
                var r = registry.GetCard(item.Key);
                if (r != null)
                    items.Add(r);
            }
            Assert.Equal(100000, items.Count);
        }

        /// <summary>
        /// The Deck_IndexOf_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_IndexOf_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<int> items = new List<int>();
            foreach (var item in testCollection.Skip(5000).Take(100))
            {
                int r = registry.IndexOf(item.Value);
                items.Add(r);
            }
        }

        /// <summary>
        /// The Deck_Last_Test.
        /// </summary>
        /// <param name="lastValue">The lastValue<see cref="string"/>.</param>
        private void Deck_Last_Test(string lastValue)
        {
            Assert.Equal(registry.Last.Value, lastValue);
        }

        /// <summary>
        /// The Deck_Put_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Put_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The Deck_Put_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Put_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Value);
            }
        }

        /// <summary>
        /// The Deck_Remove_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Remove_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(70000))
            {

                var r = registry.Remove(item.Key);
                if (r != null)
                    items.Add(r);
            }
            Assert.Equal(30000, items.Count);
        }

        /// <summary>
        /// The Deck_Remove_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Deck_Remove_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(70000))
            {

                string r = registry.Remove(item.Value);
                items.Add(r);
            }
            Assert.Equal(30000, items.Count);
        }

        /// <summary>
        /// The SharedDeck_Add_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Add_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The SharedDeck_Add_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Add_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Value);
            }
        }

        /// <summary>
        /// The SharedDeck_Contains_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Contains_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.Contains(registry.NewCard(item.Key, item.Value)))
                    items.Add(true);
            }
        }

        /// <summary>
        /// The SharedDeck_ContainsKey_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_ContainsKey_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.ContainsKey(item.Key))
                    items.Add(true);
            }
        }

        /// <summary>
        /// The SharedDeck_Dequeue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Dequeue_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string output = null;
                if (registry.TryDequeue(out output))
                    items.Add(output);
            }
            Assert.Equal(5, items.Count);
        }

        /// <summary>
        /// The SharedDeck_Enqueue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Enqueue_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection.Skip(5000).Take(5))
            {
                if (registry.Enqueue(item.Key, item.Value))
                    items.Add(true);
            }
            Assert.Equal(5, items.Count);
        }

        /// <summary>
        /// The SharedDeck_Get_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Get_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection)
            {
                string r = registry.Get(item.Key);
                if (r != null)
                    items.Add(r);
            }
        }

        /// <summary>
        /// The SharedDeck_GetByIndexer_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_GetByIndexer_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            int i = 0;
            foreach (var item in testCollection)
            {
                string a = registry[i];
                string b = item.Value;
            }
        }

        /// <summary>
        /// The SharedDeck_GetCard_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_GetCard_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<ICard<string>> items = new List<ICard<string>>();
            foreach (var item in testCollection)
            {
                var r = registry.GetCard(item.Key);
                if (r != null)
                    items.Add(r);
            }
        }

        /// <summary>
        /// The SharedDeck_Put_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Put_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The SharedDeck_Put_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Put_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Value);
            }
        }

        /// <summary>
        /// The SharedDeck_Remove_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Remove_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(5000))
            {

                var r = registry.Remove(item.Key);
                if (r != null)
                    items.Add(r);
            }
        }

        /// <summary>
        /// The SharedDeck_Remove_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void SharedDeck_Remove_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(5000))
            {

                string r = registry.Remove(item.Value);
                items.Add(r);
            }
        }

        #endregion
    }
}
