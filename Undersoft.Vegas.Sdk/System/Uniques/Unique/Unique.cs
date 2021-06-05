/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.Unique.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Collections.Concurrent;
    using System.Threading;

    public static class Unique
    {
        #region Fields

        private static readonly int CAPACITY = 100 * 1000;
        private static readonly int LOW_LIMIT = 50 * 1000;
        private static readonly uint NEXT_KEY_VECTOR = (uint)PRIMES_ARRAY.Get(4);
        private static readonly int WAIT_LOOPS = 500;
        private static bool generating;
        private static Thread generator;
        private static object holder = new object();
        private static Unique32 bit32;
        private static Unique64 bit64;
        private static ulong keyNumber = (ulong)DateTime.Now.Ticks;
        private static ConcurrentQueue<ulong> keys = new ConcurrentQueue<ulong>();
        private static Random randomSeed = new Random((int)DateTime.Now.Ticks.UniqueKey32());

        #endregion

        #region Constructors

        static Unique()
        {
            bit32 = new Unique32();
            bit64 = new Unique64();
            generator = startup();
        }

        #endregion

        #region Properties

        public static Unique32 Bit32 { get => bit32; }

        public static Unique64 Bit64 { get => bit64; }

        public static ulong NewKey
        {
            get
            {
                ulong key = 0;
                int counter = 0;
                bool loop = false;
                while (counter < WAIT_LOOPS)
                {
                    if (!(loop = keys.TryDequeue(out key)))
                    {
                        if (!generating)
                            Start();

                        counter++;
                        Thread.Sleep(20);
                    }
                    else
                    {
                        int count = keys.Count;
                        if (count < LOW_LIMIT)
                            Start();
                        break;
                    }
                }
                return key;
            }
        }

        #endregion

        #region Methods

        public static void Start()
        {
            lock (holder)
            {
                if (!generating)
                {
                    generating = true;
                    generator.Start();
                }
            }
        }

        public static void Stop()
        {
            if (generating)
            {
                generator.Join();
                generating = false;
            }
        }

        private unsafe static void keyGeneration()
        {
            ulong seed = nextSeed();
            int count = CAPACITY - keys.Count;
            for (int i = 0; i < count; i++)
            {
                ulong keyNo = nextKeyNumber();
                keys.Enqueue(Hasher64.ComputeKey(((byte*)&keyNo), 8, seed));
            }
            Stop();
        }

        private static unsafe ulong nextKeyNumber()
        {
            return keyNumber += NEXT_KEY_VECTOR;
        }

        private static ulong nextSeed()
        {
            return (ulong)randomSeed.Next();
        }

        private static Thread startup()
        {
            generating = true;
            Thread _reffiler = new Thread(new ThreadStart(keyGeneration));
            _reffiler.Start();
            return _reffiler;
        }

        #endregion
    }
}
