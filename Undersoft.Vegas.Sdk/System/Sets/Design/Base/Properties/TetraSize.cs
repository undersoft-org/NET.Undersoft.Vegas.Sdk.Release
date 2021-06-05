using System.Uniques;
/*************************************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Basedeck.TetraSize
              
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                      
    @version 0.7.1.r.d (Feb 7, 2020)                                            
    @licence MIT                                       
 *********************************************************************************/
namespace System.Sets.Basedeck
{  
    public struct TetraSize
    {
        public TetraSize(int size = 8)
        {
            StartSize = size;
            EvenPositiveSize = size;
            OddPositiveSize = size;
            EvenNegativeSize = size;
            OddNegativeSize = size;

            EvenPositivePrimesId=0;
            OddPositivePrimesId = 0;
            EvenNegativePrimesId = 0;
            OddNegativePrimesId = 0;
        }

        public unsafe int this[uint id]
        {
            get
            {
                fixed (TetraSize* a = &this)
                    return *&((int*)a)[id];
            }
            set
            {
                fixed (TetraSize* a = &this)
                    *&((int*)a)[id] = value;
            }
        }

        public unsafe int NextSize(uint id)
        {
            fixed (TetraSize* a = &this)
                return (*&((int*)a)[id]) = PRIMES_ARRAY.Get((*&((int*)a)[id+4])++);
        }
        public unsafe int PreviousSize(uint id)
        {
            fixed (TetraSize* a = &this)
                return (*&((int*)a)[id]) = PRIMES_ARRAY.Get(--(*&((int*)a)[id + 4]));
        }

        public unsafe int GetPrimesId(uint id)
        {
            return this[id + 4];
        }

        public unsafe void SetPrimesId(uint id, int value)
        {
            this[id + 4] = value;
        }

        public unsafe void Reset(uint id)
        {
            fixed (TetraSize* a = &this)
            {
                (*&((int*)a)[id]) = StartSize;
            }
        }

        public unsafe void ResetAll()
        {
            fixed (TetraSize* a = &this)
            {
                (*&((int*)a)[0]) = StartSize;
                (*&((int*)a)[1]) = StartSize;
                (*&((int*)a)[2]) = StartSize;
                (*&((int*)a)[3]) = StartSize;
            }
        }

        public int EvenPositiveSize;
        public int OddPositiveSize;
        public int EvenNegativeSize;
        public int OddNegativeSize;

        public int EvenPositivePrimesId;
        public int OddPositivePrimesId;
        public int EvenNegativePrimesId;
        public int OddNegativePrimesId;

        public int StartSize;
    }

}
