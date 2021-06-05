using System.Collections;
using System.Collections.Generic;
using System.Sets.Basedeck;

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
    public class CardSeries<V> : IEnumerator<ICard<V>>, IEnumerator<V>, IEnumerator
    {
        private IDeck<V> map;

        public CardSeries(IDeck<V> Map)
        {
            map = Map;        
            Entry = map.First;
        }

        public ICard<V> Entry;

        public ulong Key { get { return Entry.Key; } }
        public V Value { get { return Entry.Value; } }

        public object Current => Entry.Value;

        ICard<V> IEnumerator<ICard<V>>.Current => Entry;

        V IEnumerator<V>.Current => Entry.Value;

        public bool MoveNext()
        {
            Entry = map.Next(Entry);
            if (Entry != null)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            Entry = map.First;
        }

        public void Dispose()
        {
            Entry = map.First;
        }
    }    
}
