/*************************************************
   Copyright (c) 2021 Undersoft

   NoteEvoker.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Extract;
    using System.Linq;
    using System.Sets;
    using System.Uniques;

    public class NoteEvoker : Board<Labor>, IUnique
    {
        #region Fields

        public List<Labor> RelationLabors = new List<Labor>();
        public List<string> RelationNames = new List<string>();
        private Usid SerialCode;

        #endregion

        #region Constructors

        public NoteEvoker(Labor sender, Labor recipient, List<Labor> relayLabors)
        {
            Sender = sender;
            SenderName = sender.Laborer.LaborerName;
            Recipient = recipient;
            RecipientName = recipient.Laborer.LaborerName;
            SerialCode = new Usid(($"{SenderName}.{RecipientName}").UniqueKey());
            RelationLabors = relayLabors;
            RelationNames.AddRange(RelationLabors.Select(rn => rn.Laborer.LaborerName));
        }
        public NoteEvoker(Labor sender, Labor recipient, List<string> relayNames)
        {
            Sender = sender;
            SenderName = sender.Laborer.LaborerName;
            Recipient = recipient;
            RecipientName = recipient.Laborer.LaborerName;
            SerialCode = new Usid(($"{SenderName}.{RecipientName}").UniqueKey());
            RelationNames = relayNames;
            RelationLabors = Sender.Scope.Subjects.AsCards()
                                    .Where(m => m.Value.Labors.AsIdentifiers()
                                        .Where(k => relayNames
                                            .Select(rn => rn.UniqueKey()).Contains(k.UniqueKey)).Any())
                                                .SelectMany(os => os.Value.Labors.AsCards()
                                                    .Select(o => o.Value)).ToList();
        }
        public NoteEvoker(Labor sender, string recipientName, IList<Labor> relayLabors)
        {
            Sender = sender;
            SenderName = sender.Laborer.LaborerName;
            RecipientName = recipientName;
            SerialCode = new Usid(($"{SenderName}.{RecipientName}").UniqueKey());
            List<Labor> objvl = Sender.Scope.Subjects.AsCards()
                                        .Where(m => m.Value.Labors.ContainsKey(recipientName))
                                            .SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
            if (objvl.Any())
                Recipient = objvl.First();
            RelationLabors = new List<Labor>(relayLabors);
            RelationNames.AddRange(RelationLabors.Select(rn => rn.Laborer.LaborerName));
        }
        public NoteEvoker(Labor sender, string recipientName, IList<string> relayNames)
        {
            Sender = sender;
            SenderName = sender.Laborer.LaborerName;
            List<Labor> objvl = Sender.Scope.Subjects.AsCards().Where(m => m.Value.Labors.ContainsKey(recipientName)).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
            if (objvl.Any())
                Recipient = objvl.First();
            RecipientName = recipientName;
            SerialCode = new Usid(($"{SenderName}.{RecipientName}").UniqueKey());
            RelationNames = new List<string>(relayNames);
            RelationLabors = Sender.Scope.Subjects.AsCards().Where(m => m.Value.Labors.AsIdentifiers().Where(k => relayNames.Select(rn => rn.UniqueKey()).Contains(k.UniqueKey)).Any()).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
        }

        #endregion

        #region Properties

        public IUnique Empty => new Usid();

        public string EvokerName { get; set; }

        public EvokerType EvokerType { get; set; }

        public Labor Recipient { get; set; }

        public string RecipientName { get; set; }

        public Labor Sender { get; set; }

        public string SenderName { get; set; }

        public new ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }

        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        #endregion

        #region Methods

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
            return ($"{SenderName}.{RecipientName}").GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        //public uint GetUniqueSeed()
        //{
        //    return SerialCode.UniqueSeed;
        //}

        //public void SetUniqueKey(long value)
        //{
        //    SerialCode.UniqueKey = value;
        //}

        //public void SetUniqueSeed(uint seed)
        //{
        //    SerialCode.SetUniqueSeed(seed);
        //}

        //public ulong UniqueKey()
        //{
        //    return SerialCode.UniqueKey();
        //}

        #endregion
    }
}
