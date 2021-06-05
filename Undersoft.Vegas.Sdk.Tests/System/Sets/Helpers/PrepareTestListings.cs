/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.PrepareTestListings.cs.Tests
   
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
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="PrepareTestListings" />.
    /// </summary>
    public static class PrepareTestListings
    {
        #region Methods

        /// <summary>
        /// The prepareIdentifierKeyTestCollection.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair{object, string}}"/>.</returns>
        public static IList<KeyValuePair<object, string>> prepareIdentifierKeyTestCollection()
        {
            List<KeyValuePair<object, string>> list = new List<KeyValuePair<object, string>>();
            string now = DateTime.Now.ToString() + "_prepareStringKeyTestCollection";
            ulong max = uint.MaxValue + 250000L;
            for (ulong i = uint.MaxValue; i < max; i++)
            {
                string str = i.ToString() + "_" + now;
                list.Add(new KeyValuePair<object, string>(new Usid(i), str));
            }
            return list;
        }

        /// <summary>
        /// The prepareIntKeyTestCollection.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair{object, string}}"/>.</returns>
        public static IList<KeyValuePair<object, string>> prepareIntKeyTestCollection()
        {
            List<KeyValuePair<object, string>> list = new List<KeyValuePair<object, string>>();
            string now = DateTime.Now.ToString() + "_prepareStringKeyTestCollection";
            for (int i = 0; i < 250000; i++)
            {
                string str = i.ToString() + "_" + now;
                list.Add(new KeyValuePair<object, string>(i, str));
            }
            return list;
        }

        /// <summary>
        /// The prepareLongKeyTestCollection.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair{object, string}}"/>.</returns>
        public static IList<KeyValuePair<object, string>> prepareLongKeyTestCollection()
        {
            List<KeyValuePair<object, string>> list = new List<KeyValuePair<object, string>>();
            string now = DateTime.Now.ToString() + "_prepareStringKeyTestCollection";
            ulong max = uint.MaxValue + 250000L;
            for (ulong i = uint.MaxValue; i < max; i++)
            {
                string str = i.ToString() + "_" + now;
                list.Add(new KeyValuePair<object, string>(i, str));
            }
            return list;
        }

        /// <summary>
        /// The prepareStringKeyTestCollection.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair{object, string}}"/>.</returns>
        public static IList<KeyValuePair<object, string>> prepareStringKeyTestCollection()
        {
            List<KeyValuePair<object, string>> list = new List<KeyValuePair<object, string>>();
            string now = DateTime.Now.ToString() + "_prepareStringKeyTestCollection";
            for (int i = 0; i < 250000; i++)
            {
                string str = i.ToString() + "_" + now;
                list.Add(new KeyValuePair<object, string>(new object[] { (i + 1000).ToString() + now, new Usid(DateTime.Now.ToBinary()), DateTime.Now }, str));
            }
            List<object[]> keys = new List<object[]>();
            now = "_prepareObjectKeyTestCollection";
            for (int i = 0; i < 250000; i++)
            {
                keys.Add(new object[] { (i + 1000).ToString() + now, new Usid(DateTime.Now.ToBinary()), DateTime.Now });
            }
            List<ulong> hashes = new List<ulong>();
            foreach (var s in keys)
            {
                hashes.Add(s.UniqueKey64());
            }
            return list;
        }

        #endregion
    }
}
