/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.CardKeySeries.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/**********************************************
    Copyright (c) 2020 Undersoft

    System.Sets.DeckSeries
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                     
 
*********************************************/
namespace System.Sets
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="CardKeySeries{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public class CardKeySeries<V> : IEnumerator<IUnique<V>>, IEnumerator
    {
        #region Fields

        public ICard<V> Entry;
        private IDeck<V> map;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CardKeySeries{V}"/> class.
        /// </summary>
        /// <param name="Map">The Map<see cref="IDeck{V}"/>.</param>
        public CardKeySeries(IDeck<V> Map)
        {
            map = Map;
            Entry = map.First;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Current.
        /// </summary>
        public object Current => Entry.Key;

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public ulong Key
        {
            get { return Entry.Key; }
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
        IUnique<V> IEnumerator<IUnique<V>>.Current => Entry;

        #endregion

        #region Methods

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            Entry = map.First;
        }

        /// <summary>
        /// The MoveNext.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool MoveNext()
        {
            Entry = Entry.Next;
            if (Entry != null)
            {
                if (Entry.Removed)
                    return MoveNext();
                return true;
            }
            return false;
        }

        /// <summary>
        /// The Reset.
        /// </summary>
        public void Reset()
        {
            Entry = map.First;
        }

        #endregion
    }
}
