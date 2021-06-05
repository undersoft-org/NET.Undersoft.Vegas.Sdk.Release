/*************************************************
   Copyright (c) 2021 Undersoft

   Laborer.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Instant;
    using System.Sets;
    using System.Uniques;

    public class Laborer : IUnique
    {
        #region Fields

        private Board<object> input;
        private Board<object> output;
        private Ussc SerialCode;

        #endregion

        #region Constructors

        public Laborer()
        {
            input = new Board<object>();
            output = new Board<object>();
            EvokersIn = new NoteEvokers();
        }
        public Laborer(string Name, IDeputy Method) : this()
        {
            Work = Method;
            LaborerName = Name;

            SerialCode = new Ussc(($"{Work.UniqueKey}.{DateTime.Now.ToBinary()}").UniqueKey());
        }

        #endregion

        #region Properties

        public IUnique Empty => new Ussc();

        public NoteEvokers EvokersIn { get; set; }

        public object Input
        {

            get
            {
                object _entry = null;
                input.TryDequeue(out _entry);
                return _entry;

            }
            set
            {
                input.Enqueue(value);
            }
        }

        public Labor Labor { get; set; }

        public string LaborerName { get; set; }

        public object Output
        {
            get
            {
                object _result = null;
                if (output.TryPick(0, out _result))
                    return _result;
                return null;
            }
            set
            {
                output.Enqueue(value);
            }
        }

        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.UniqueKey = value; }

        public ulong UniqueSeed { get => ((IUnique)SerialCode).UniqueSeed; set => ((IUnique)SerialCode).UniqueSeed = value; }

        public IDeputy Work { get; set; }

        #endregion

        #region Methods

        public void AddEvoker(Labor Recipient)
        {
            EvokersIn.Add(new NoteEvoker(Labor, Recipient, new List<Labor>() { Labor }));
        }

        public void AddEvoker(Labor Recipient, List<Labor> RelationLabors)
        {
            EvokersIn.Add(new NoteEvoker(Labor, Recipient, RelationLabors));
        }

        public void AddEvoker(string RecipientName)
        {
            EvokersIn.Add(new NoteEvoker(Labor, RecipientName, new List<string>() { LaborerName }));
        }

        public void AddEvoker(string RecipientName, List<string> RelationNames)
        {
            EvokersIn.Add(new NoteEvoker(Labor, RecipientName, RelationNames));
        }

        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return SerialCode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        #endregion
    }
}
