/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.SpecrtumSeries.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/*********************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.SpectrumSeries
    
    @authors Darius Hanc & PhD Radek Rudek 
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT
 *********************************************************************************/
namespace System.Sets
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SpectrumSeries{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class SpectrumSeries<V> : IEnumerator<BaseCard<V>>, IEnumerator
    {
        #region Fields

        public BaseCard<V> Entry;
        private int iterated = 0;
        private int lastReturned;
        private Spectrum<V> map;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumSeries{V}"/> class.
        /// </summary>
        /// <param name="Map">The Map<see cref="Spectrum{V}"/>.</param>
        public SpectrumSeries(Spectrum<V> Map)
        {
            map = Map;
            Entry = new Card64<V>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Current.
        /// </summary>
        public object Current => Entry;

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public int Key
        {
            get { return (int)Entry.Key; }
        }

        /// <summary>
        /// Gets the Value.
        /// </summary>
        public V Value
        {
            get { return Entry.Value; }
        }

        /// <summary>
        /// Gets the Current.
        /// </summary>
        BaseCard<V> IEnumerator<BaseCard<V>>.Current => Entry;

        #endregion

        #region Methods

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            iterated = 0;
            Entry = null;
        }

        /// <summary>
        /// The HasNext.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool HasNext()
        {
            return iterated < map.Count;
        }

        /// <summary>
        /// The MoveNext.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool MoveNext()
        {
            return Next();
        }

        /// <summary>
        /// The Next.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Next()
        {
            if (!HasNext())
            {
                return false;
            }

            if (iterated == 0)
            {
                lastReturned = map.IndexMin;
                iterated++;
                Entry.Key = (uint)lastReturned;
                Entry.Value = map.Get(lastReturned);
            }
            else
            {
                lastReturned = map.Next(lastReturned); ;
                iterated++;
                Entry.Key = (uint)lastReturned;
                Entry.Value = map.Get(lastReturned);
            }
            return true;
        }

        /// <summary>
        /// The Reset.
        /// </summary>
        public void Reset()
        {
            Entry = new Card64<V>();
            iterated = 0;
        }

        #endregion
    }
}
