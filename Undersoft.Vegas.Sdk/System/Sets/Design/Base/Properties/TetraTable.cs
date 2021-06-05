/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Basedeck.TetraTable
              
    @author Dariusz Hanc                                                  
    @project Undersoft.Vegas.Sdk                      
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/

namespace System.Sets.Basedeck
{
    public struct TetraTable<V> : IDisposable
    {
        public TetraTable(TetraSet<V> hashdeck, int size = 8)
        {
            EvenPositiveSize = hashdeck.EmptyCardTable(size);
            OddPositiveSize = hashdeck.EmptyCardTable(size);
            EvenNegativeSize = hashdeck.EmptyCardTable(size);
            OddNegativeSize = hashdeck.EmptyCardTable(size);
            tetraTable = new ICard<V>[4][] { EvenPositiveSize, OddPositiveSize, EvenNegativeSize, OddNegativeSize };

        }

        public unsafe ICard<V>[] this[uint id]
        {
            get
            {
                return tetraTable[id];
            }
            set
            {
                tetraTable[id] = value;
            }
        }

        public unsafe ICard<V> this[uint id, uint pos]
        {
            get
            {
                return this[id][pos];
            }
            set
            {
                this[id][pos] = value;
            }
        }

        public unsafe ICard<V>[] this[ulong key]
        {
            get
            {
                return this[(uint)((key & 1) | ((key >> 62) & 2))];
            }
            set
            {
                this[(uint)((key & 1) | ((key >> 62) & 2))] = value;
            }
        }

        public unsafe ICard<V> this[ulong key, long size]
        {
            get
            {
                return this[(uint)((key & 1) | ((key >> 62) & 2))]
                                 [(int)(key % (uint)size)];
            }
            set
            {
                this[(uint)((key & 1) | ((key >> 62) & 2))]
                                 [(int)(key % (uint)size)] = value;
            }
        }

        public static int GetId(ulong key)
        {
            ulong ukey = (ulong)key;
            return (int)((ukey & 1) | ((ukey >> 62) & 2));
        }

        public static int GetPosition(ulong key, long size)
        {
            return (int)(key % (ulong)size);
        }

        private ICard<V>[] EvenPositiveSize;
        private ICard<V>[] OddPositiveSize;
        private ICard<V>[] EvenNegativeSize;
        private ICard<V>[] OddNegativeSize;

        private ICard<V>[][] tetraTable;

        public void Dispose()
        {
            EvenPositiveSize = null;
            OddPositiveSize = null;
            EvenNegativeSize = null;
            OddNegativeSize = null;
            tetraTable = null;
        }
    }
}
