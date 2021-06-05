/*************************************************
   Copyright (c) 2021 Undersoft

   Laborator.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Sets;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="Laborator" />.
    /// </summary>
    public class Laborator
    {
        #region Fields

        /// <summary>
        /// Defines the holder.
        /// </summary>
        internal readonly object holder = new object();
        /// <summary>
        /// Defines the holderIO.
        /// </summary>
        internal readonly object holderIO = new object();
        /// <summary>
        /// Defines the Notes.
        /// </summary>
        public LaborNotes Notes;
        /// <summary>
        /// Defines the Ready.
        /// </summary>
        public bool Ready;
        /// <summary>
        /// Defines the Scope.
        /// </summary>
        public Scope Scope;
        /// <summary>
        /// Defines the Subject.
        /// </summary>
        public Subject Subject;
        /// <summary>
        /// Defines the laborers.
        /// </summary>
        private Thread[] laborers;
        /// <summary>
        /// Defines the LaborersCount.
        /// </summary>
        private int LaborersCount;
        /// <summary>
        /// Defines the LaborersQueue.
        /// </summary>
        private Board<Laborer> LaborersQueue =
            new Board<Laborer>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Laborator"/> class.
        /// </summary>
        /// <param name="mission">The mission<see cref="Subject"/>.</param>
        public Laborator(Subject mission)
        {
            Subject = mission;
            Scope = Subject.Scope;
            Notes = Scope.Notes;
            LaborersCount = Subject.LaborersCount;
            Ready = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Close.
        /// </summary>
        /// <param name="SafeClose">The SafeClose<see cref="bool"/>.</param>
        public void Close(bool SafeClose)
        {
            // Enqueue one null item per worker to make each exit.
            foreach (Thread laborer in laborers)
            {
                Elaborate(null);

                if (SafeClose && laborer.ThreadState == ThreadState.Running)
                    laborer.Join();
            }
        }

        /// <summary>
        /// The CreateLaborers.
        /// </summary>
        /// <param name="antcount">The antcount<see cref="int"/>.</param>
        public void CreateLaborers(int antcount = 1)
        {
            if (antcount > 1)
            {
                LaborersCount = antcount;
                Subject.LaborersCount = antcount;
            }
            else
                LaborersCount = Subject.LaborersCount;

            laborers = new Thread[LaborersCount];
            // Create and start a separate thread for each Ant
            for (int i = 0; i < LaborersCount; i++)
            {
                laborers[i] = new Thread(ActivateLaborer);
                laborers[i].Start();
            }
        }

        /// <summary>
        /// The Elaborate.
        /// </summary>
        /// <param name="worker">The worker<see cref="Laborer"/>.</param>
        public void Elaborate(Laborer worker)
        {
            lock (holder)
            {
                if (worker != null)
                {
                    Laborer Worker = CloneLaborer(worker);
                    LaborersQueue.Enqueue(Worker);
                    Monitor.Pulse(holder);

                }
                else
                {
                    LaborersQueue.Enqueue(DateTime.Now.ToBinary(), worker);
                    Monitor.Pulse(holder);
                }
            }
        }

        /// <summary>
        /// The Reset.
        /// </summary>
        /// <param name="antcount">The antcount<see cref="int"/>.</param>
        public void Reset(int antcount = 1)
        {
            Close(true);
            CreateLaborers(antcount);
        }

        /// <summary>
        /// The ActivateLaborer.
        /// </summary>
        private void ActivateLaborer()
        {
            while (true)
            {
                Laborer worker = null;
                object input = null;
                lock (holder)
                {
                    do
                    {
                        while (LaborersQueue.Count == 0)
                        {
                            Monitor.Wait(holder);
                        }
                    } while (!LaborersQueue.TryDequeue(out worker));


                    if (worker != null)
                        input = worker.Input;
                }

                if (worker == null)
                    return;

                object output = null;

                if (input != null)
                {
                    if (input is IList)
                        output = worker.Work.Execute((object[])input);
                    else
                        output = worker.Work.Execute(input);
                }
                else
                    output = worker.Work.Execute();

                lock (holderIO)
                    Outpost(worker, output);

            }
        }

        /// <summary>
        /// The CloneLaborer.
        /// </summary>
        /// <param name="laborer">The laborer<see cref="Laborer"/>.</param>
        /// <returns>The <see cref="Laborer"/>.</returns>
        private Laborer CloneLaborer(Laborer laborer)
        {
            Laborer _laborer = new Laborer(laborer.LaborerName, laborer.Work);
            _laborer.Input = laborer.Input;
            _laborer.EvokersIn = laborer.EvokersIn;
            _laborer.Labor = laborer.Labor;
            return _laborer;
        }

        /// <summary>
        /// The Outpost.
        /// </summary>
        /// <param name="worker">The worker<see cref="Laborer"/>.</param>
        /// <param name="output">The output<see cref="object"/>.</param>
        private void Outpost(Laborer worker, object output)
        {
            if (output != null)
            {
                worker.Output = output;

                if (worker.EvokersIn != null && worker.EvokersIn.AsValues().Any())
                {
                    List<Note> intios = new List<Note>();
                    foreach (NoteEvoker evoker in worker.EvokersIn.AsValues())
                    {
                        Note intio = new Note(worker.Labor, evoker.Recipient, evoker, null, output);
                        intio.SenderBox = worker.Labor.Box;
                        intios.Add(intio);
                    }

                    if (intios.Any())
                        Notes.Send(intios);
                }

            }
        }

        #endregion
    }
}
