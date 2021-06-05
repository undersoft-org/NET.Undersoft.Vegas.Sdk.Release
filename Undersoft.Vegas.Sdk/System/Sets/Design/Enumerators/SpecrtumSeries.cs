using System.Collections;
using System.Collections.Generic;

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
    public class SpectrumSeries<V> : IEnumerator<BaseCard<V>>, IEnumerator
    {
        private Spectrum<V> map;
        private int iterated = 0;
        private int lastReturned;

        public SpectrumSeries(Spectrum<V> Map)
        {
            map = Map;
            Entry = new Card64<V>();
        }

        public BaseCard<V> Entry;

        public int Key { get { return (int)Entry.Key; } }
        public V Value { get { return Entry.Value; } }

        public object Current => Entry;

        BaseCard<V> IEnumerator<BaseCard<V>>.Current => Entry;

        public bool MoveNext()
        {
            return Next();
        }

        public void Reset()
        {
            Entry = new Card64<V>();
            iterated = 0;
        }

        public bool HasNext()
        {
            return iterated < map.Count;
        }

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

        public void Dispose()
        {
            iterated = 0;
            Entry = null;
        }
    }

}
