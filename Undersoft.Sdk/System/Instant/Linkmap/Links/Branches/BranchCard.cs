using System.Runtime.InteropServices;
using System.Extract;
using System.Uniques;
using System.Multemic;
using System.Instant.Linkmap;
using System.Linq;
using System.Reflection;
using System.IO;

/******************************************************************
    Copyright (c) 2020 Undersoft

    @name System.Instant.BranchCard                
    
    @project NET.Undersoft.Sdk
    @author Darius Hanc                                                                               
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ******************************************************************/
namespace System.Instant
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class BranchCard : Card<ICard<IFigure>>, IFigure, IEquatable<ICard<IFigure>>, IComparable<ICard<IFigure>>
    {
        public BranchCard(LinkMember member)
        {
            Member = member;
        }
        public BranchCard(object key, ICard<IFigure> value, LinkMember member) : base(key, value)
        {
            Member = member;
        }
        public BranchCard(long key, ICard<IFigure> value, LinkMember member) : base(key, value)
        {
            Member = member;
        }
        public BranchCard(ICard<IFigure> value, LinkMember member) : base(value)
        {
            Member = member;

        }
        public BranchCard(ICard<ICard<IFigure>> value, LinkMember member) : base(value)
        {
            Member = member;
        }

        public object this[int fieldId]
        {
            get => ((IFigure)this.value)[fieldId];
            set => ((IFigure)this.value)[fieldId] = value;
        }
        public object this[string propertyName]
        {
            get => ((IFigure)this.value)[propertyName];
            set => ((IFigure)this.value)[propertyName] = value;
        }

        public override void Set(object key, ICard<IFigure> value)
        {
            this.value = value;
            this.value.KeyBlock = key.GetHashKey();
        }
        public override void Set(ICard<IFigure> value)
        {
            this.value = value;
        }
        public override void Set(ICard<ICard<IFigure>> card)
        {
            this.value = card.Value;        
        }

        public override bool Equals(long key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.GetHashKey());
        }
        public          bool Equals(ICard<IFigure> other)
        {
            return Key == other.KeyBlock;
        }

        public override int GetHashCode()
        {
            return Value.GetKeyBytes().BitAggregate64to32().ToInt32();
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.GetHashKey64());
        }
        public override int CompareTo(long key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<ICard<IFigure>> other)
        {
            return (int)(Key - other.Key);
        }
        public          int CompareTo(ICard<IFigure> other)
        {
            return (int)(Key - other.KeyBlock);
        }

        public override byte[] GetBytes()
        {
            return value.GetBytes();
        }

        public unsafe override byte[] GetKeyBytes()
        {
            return value.GetKeyBytes();
        }

        public override    int[] IdentityIndexes()
        {           
            return Member.GetLinkOrdinals();
        }
        public override object[] IdentityValues()
        {
            return Member.GetLinkValues(value.Value);      
        }
        public override     long IdentitiesToKey()
        {
            return Member.GetLinkKey(value.Value);
        }  

        public override long Key
        {
            get => value.KeyBlock;                                                   
            set => this.value.KeyBlock = value;            
        }

        public override long KeyBlock
        {
            get => value.KeyBlock;
            set => this.value.KeyBlock = value;
        }

        public object[] ValueArray
        {
            get
            {
                return ((IFigure)this.value).ValueArray;
            }
            set
            {
                ((IFigure)this.value).ValueArray = value;
            }
        }

        public     Ussn SystemSerialCode
        {
            get => value.Value.SystemSerialCode;
            set => this.value.Value.SystemSerialCode = value;
        }
       
        public LinkMember Member { get; set; }

    }
}
