/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Basedeck.TetraTable
              
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                      
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Multemic.Basedeck
{
    public struct TetraTable<V>: IDisposable
    {
        public TetraTable(Tetradeck<V> hashdeck, int size = 8)
        {
            EvenPositiveSize = hashdeck.EmptyCardTable(size);
            OddPositiveSize = hashdeck.EmptyCardTable(size);
            EvenNegativeSize = hashdeck.EmptyCardTable(size);
            OddNegativeSize = hashdeck.EmptyCardTable(size);
            tetraTable = new ICard<V>[4][] { EvenPositiveSize, OddPositiveSize, EvenNegativeSize, OddNegativeSize};

        }

        public unsafe ICard<V>[] this[int id]
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

        public unsafe ICard<V> this[int id, int pos]
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

        public unsafe ICard<V>[] this[long key]
        {
            get
            {
                ulong ukey = (ulong)key;
                return this[(int)((ukey & 1) | ((ukey >> 62) & 2))];
            }
            set
            {
                ulong ukey = (ulong)key;
                this[(int)((ukey & 1) | ((ukey >> 62) & 2))] = value;
            }
        }

        public unsafe ICard<V> this[long key, long size]
        {
            get
            {
                ulong ukey = (ulong)key;
                return this[(int)((ukey & 1) | ((ukey >> 62) & 2))]
                                 [(int)(ukey % (uint)size)];
            }
            set
            {
                ulong ukey = (ulong)key;
                this[(int)((ukey & 1) | ((ukey >> 62) & 2))]
                                 [(int)(ukey % (uint)size)] = value;
            }
        }

        public static int GetId(long key)
        {
            ulong ukey = (ulong)key;
            return (int)((ukey & 1) | ((ukey >> 62) & 2));
        }

        public static int GetPosition(long key, long size)
        {
            return (int)((ulong)key % (uint)size);
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
