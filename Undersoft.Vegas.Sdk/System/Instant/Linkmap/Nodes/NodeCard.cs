/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.BranchCard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Extract;
    using System.Instant.Linking;
    using System.Runtime.InteropServices;
    using System.Sets;
    using System.Uniques;


    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class NodeCard : BaseCard<BranchDeck>, IEquatable<BranchDeck>, IComparable<BranchDeck>
    {
        public NodeCard()
        {
        }
        public NodeCard(object key, BranchDeck value) : base(key, value)
        {
        }
        public NodeCard(ulong key, BranchDeck value) : base(key, value)
        {
        }
        public NodeCard(BranchDeck value) : base(value)
        {

        }
        public NodeCard(ICard<BranchDeck> value) : base(value)
        {
        }

        public object this[int fieldId]
        {
            get => this.value[fieldId];
            set => this.value[fieldId] = (ICard<IFigure>)value;
        }
        public object this[string propertyName]
        {
            get => this.value[propertyName];
            set => this.value[propertyName] = (ICard<IFigure>)value;
        }

        public override void Set(ulong key, BranchDeck value)
        {
            this.value = value;
            Member = value.Member;
        }
        public override void Set(object key, BranchDeck value)
        {
            this.value = value;
            Member = value.Member;
        }
        public override void Set(BranchDeck value)
        {
            this.value = value;
            Member = value.Member;
        }
        public override void Set(ICard<BranchDeck> card)
        {
            this.value = card.Value;
        }

        public override bool Equals(ulong key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.UniqueKey());
        }
        public bool Equals(BranchDeck other)
        {
            return Key == other.UniqueKey;
        }

        public override int GetHashCode()
        {
            return Value.GetUniqueBytes().BitAggregate64to32().ToInt32();
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.UniqueKey64());
        }
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<BranchDeck> other)
        {
            return (int)(Key - other.Key);
        }
        public int CompareTo(BranchDeck other)
        {
            return (int)(Key - other.UniqueKey);
        }

        public override byte[] GetBytes()
        {
            return value.GetBytes();
        }

        public unsafe override byte[] GetUniqueBytes()
        {
            return value.GetUniqueBytes();
        }

        public override int[] UniqueOrdinals()
        {
            return Member.KeyRubrics.Ordinals;
        }
        public override object[] UniqueValues()
        {
            if (this.value.Count > 0)
                return this.value[0].UniqueValues();
            return null;
        }
        public override ulong UniquesAsKey()
        {
            if (this.value.Count > 0)
                return this.value[0].UniquesAsKey();
            return 0;
        }

        public override ulong Key
        {
            get => value.UniqueKey;
            set => this.value.UniqueKey = value;
        }

        public override ulong UniqueKey
        {
            get => value.UniqueKey;
            set => this.value.UniqueKey = value;
        }

        public LinkMember Member { get; set; }

        public NodeCatalog Branches { get; set; }

        public override IUnique Empty => this.value.Empty;

        public override ulong UniqueSeed
        { get => Member.UniqueSeed; set => Member.UniqueSeed = value; }

        public override int CompareTo(IUnique other)
        {
            return this.value.CompareTo(other);
        }

        public override bool Equals(IUnique other)
        {
            return this.value.Equals(other);
        }
    }
}
