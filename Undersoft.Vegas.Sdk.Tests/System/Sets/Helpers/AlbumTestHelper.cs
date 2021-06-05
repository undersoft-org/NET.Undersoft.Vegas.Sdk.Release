/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.AlbumTestHelper.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Sets;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="AlbumTestHelper" />.
    /// </summary>
    public class AlbumTestHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumTestHelper"/> class.
        /// </summary>
        public AlbumTestHelper()
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
        /// The Album_Integrated_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        public void Album_Integrated_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            Album_Add_Test(testCollection);
            Album_Count_Test(250000);
            Album_First_Test(testCollection[0].Value);
            Album_Last_Test(testCollection[249999].Value);
            Album_Get_Test(testCollection);
            Album_GetCard_Test(testCollection);
            Album_Remove_Test(testCollection);
            Album_Count_Test(220000);
            Album_Enqueue_Test(testCollection);
            Album_Count_Test(220005);
            Album_Dequeue_Test(testCollection);
            Album_Contains_Test(testCollection);
            Album_ContainsKey_Test(testCollection);
            Album_Put_Test(testCollection);
            Album_Count_Test(250000);
            Album_Clear_Test();
            Album_Add_V_Test(testCollection);
            Album_Count_Test(250000);
            Album_Remove_V_Test(testCollection);
            Album_Count_Test(220000);
            Album_Put_V_Test(testCollection);
            Album_IndexOf_Test(testCollection);
            Album_GetByIndexer_Test(testCollection);
            Album_Count_Test(250000);
        }

        /// <summary>
        /// The BaseCatalog_Integrated_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        public void BaseCatalog_Integrated_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            Album_Add_Test(testCollection);
            Album_Get_Test(testCollection);
            Album_GetCard_Test(testCollection);
            Album_Remove_Test(testCollection);
            Album_Enqueue_Test(testCollection);
            Album_Dequeue_Test(testCollection);
            Album_Contains_Test(testCollection);
            Album_ContainsKey_Test(testCollection);
            Album_Put_Test(testCollection);
            Album_Add_V_Test(testCollection);
            Album_Remove_V_Test(testCollection);
            Album_Put_V_Test(testCollection);
            Album_GetByIndexer_Test(testCollection);
        }

        /// <summary>
        /// The Album_Add_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Add_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The Album_Add_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Add_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Add(item.Value);
            }
        }

        /// <summary>
        /// The Album_Clear_Test.
        /// </summary>
        private void Album_Clear_Test()
        {
            registry.Clear();
            Assert.Empty(registry);
        }

        /// <summary>
        /// The Album_Contains_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Contains_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.Contains(registry.NewCard(item.Key, item.Value)))
                    items.Add(true);
            }
            Assert.Equal(220000, items.Count);
        }

        /// <summary>
        /// The Album_ContainsKey_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_ContainsKey_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection)
            {
                if (registry.ContainsKey(item.Key))
                    items.Add(true);
            }
            Assert.Equal(220000, items.Count);
        }

        /// <summary>
        /// The Album_CopyTo_Test.
        /// </summary>
        private void Album_CopyTo_Test()
        {
        }

        /// <summary>
        /// The Album_Count_Test.
        /// </summary>
        /// <param name="count">The count<see cref="int"/>.</param>
        private void Album_Count_Test(int count)
        {
            Assert.Equal(count, registry.Count);
        }

        /// <summary>
        /// The Album_Dequeue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Dequeue_Test(IList<KeyValuePair<object, string>> testCollection)
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
        /// The Album_Enqueue_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Enqueue_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<bool> items = new List<bool>();
            foreach (var item in testCollection.Skip(220000).Take(5))
            {
                if (registry.Enqueue(item.Key, item.Value))
                    items.Add(true);
            }
            Assert.Equal(5, items.Count);
        }

        /// <summary>
        /// The Album_First_Test.
        /// </summary>
        /// <param name="firstValue">The firstValue<see cref="string"/>.</param>
        private void Album_First_Test(string firstValue)
        {
            Assert.Equal(registry.Next(registry.First).Value, firstValue);
        }

        /// <summary>
        /// The Album_Get_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Get_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection)
            {
                string r = registry.Get(item.Key);
                if (r != null)
                    items.Add(r);
                else
                    r = "wrong";
            }
            Assert.Equal(250000, items.Count);
        }

        /// <summary>
        /// The Album_GetByIndexer_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_GetByIndexer_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            int i = 0;
            foreach (var item in testCollection)
            {
                string a = registry[i++];
                string b = item.Value;
            }
        }

        /// <summary>
        /// The Album_GetCard_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_GetCard_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<ICard<string>> items = new List<ICard<string>>();
            foreach (var item in testCollection)
            {
                var r = registry.GetCard(item.Key);
                if (r != null)
                    items.Add(r);
            }
            Assert.Equal(250000, items.Count);
        }

        /// <summary>
        /// The Album_IndexOf_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_IndexOf_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<int> items = new List<int>();
            foreach (var item in testCollection.Take(5000))
            {
                int r = registry.IndexOf(item.Value);
                items.Add(r);
            }
        }

        /// <summary>
        /// The Album_Last_Test.
        /// </summary>
        /// <param name="lastValue">The lastValue<see cref="string"/>.</param>
        private void Album_Last_Test(string lastValue)
        {
            Assert.Equal(registry.Last.Value, lastValue);
        }

        /// <summary>
        /// The Album_Put_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Put_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Key, item.Value);
            }
        }

        /// <summary>
        /// The Album_Put_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Put_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            foreach (var item in testCollection)
            {
                registry.Put(item.Value);
            }
        }

        /// <summary>
        /// The Album_Remove_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Remove_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(220000))
            {

                var r = registry.Remove(item.Key);
                if (r != null)
                    items.Add(r);
            }
            Assert.Equal(30000, items.Count);
        }

        /// <summary>
        /// The Album_Remove_V_Test.
        /// </summary>
        /// <param name="testCollection">The testCollection<see cref="IList{KeyValuePair{object, string}}"/>.</param>
        private void Album_Remove_V_Test(IList<KeyValuePair<object, string>> testCollection)
        {
            List<string> items = new List<string>();
            foreach (var item in testCollection.Skip(220000))
            {

                string r = registry.Remove(item.Value);
                items.Add(r);
            }
            Assert.Equal(30000, items.Count);
        }

        #endregion
    }
}
