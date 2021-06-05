/*************************************************
   Copyright (c) 2021 Undersoft

   Note.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Uniques;

    public class Note : IUnique
    {
        #region Fields

        public object[] Parameters;
        public NoteBox SenderBox;

        #endregion

        #region Constructors

        public Note(Labor sender, Labor recipient, NoteEvoker Out, NoteEvokers In, params object[] Params)
        {
            Parameters = Params;

            if (recipient != null)
            {
                Recipient = recipient;
                RecipientName = Recipient.Laborer.LaborerName;
            }

            Sender = sender;
            SenderName = Sender.Laborer.LaborerName;

            if (Out != null)
                EvokerOut = Out;

            if (In != null)
                EvokersIn = In;
        }
        public Note(string sender, params object[] Params) : this(sender, null, null, null, Params)
        {
        }
        public Note(string sender, string recipient, NoteEvoker Out, NoteEvokers In, params object[] Params)
        {
            SenderName = sender;
            Parameters = Params;

            if (recipient != null)
                RecipientName = recipient;

            if (Out != null)
                EvokerOut = Out;

            if (In != null)
                EvokersIn = In;
        }
        public Note(string sender, string recipient, NoteEvoker Out, params object[] Params) : this(sender, recipient, Out, null, Params)
        {
        }
        public Note(string sender, string recipient, params object[] Params) : this(sender, recipient, null, null, Params)
        {
        }

        #endregion

        #region Properties

        public IUnique Empty => new Ussc();

        public NoteEvoker EvokerOut { get; set; }

        public NoteEvokers EvokersIn { get; set; }

        public Labor Recipient { get; set; }

        public string RecipientName { get; set; }

        public Labor Sender { get; set; }

        public string SenderName { get; set; }

        public ulong UniqueKey { get => Sender.UniqueKey; set => Sender.UniqueKey = value; }

        public ulong UniqueSeed { get => ((IUnique)Sender).UniqueSeed; set => ((IUnique)Sender).UniqueSeed = value; }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return Sender.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return Sender.Equals(other);
        }

        public byte[] GetBytes()
        {
            return Sender.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return Sender.GetUniqueBytes();
        }

        //public uint GetUniqueSeed()
        //{
        //    return ((IUnique)Sender).UniqueSeed;
        //}

        //public void SetUniqueKey(long value)
        //{
        //    Sender.UniqueKey = value;
        //}

        //public void SetUniqueSeed(uint seed)
        //{
        //    ((IUnique)Sender).SetUniqueSeed(seed);
        //}

        //public ulong UniqueKey()
        //{
        //    return Sender.UniqueKey();
        //}

        #endregion
    }
}
