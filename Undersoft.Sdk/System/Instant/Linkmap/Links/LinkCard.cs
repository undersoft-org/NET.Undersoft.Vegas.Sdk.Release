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

    @name System.Instant.FigureCard                
    
    @project NET.Undersoft.Sdk
    @author Darius Hanc                                                                               
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ******************************************************************/
namespace System.Instant
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class LinkCard : Card<LinkBranch>, IEquatable<LinkBranch>, IComparable<LinkBranch>
    {
        private long _key;

        public LinkCard()
        {
        }
        public LinkCard(object key, LinkBranch value) : base(key, value)
        {
        }
        public LinkCard(long key, LinkBranch value) : base(key, value)
        {
        }
        public LinkCard(LinkBranch value) : base(value)
        {
            
        }
        public LinkCard(ICard<LinkBranch> value) : base(value)
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

        public override void Set(object key, LinkBranch value)
        {
            this.value = value;
            Member = value.Member;
            Key = key.GetHashKey64(value.GetHashSeed());
        }
        public override void Set(LinkBranch value)
        {
            this.value = value;
            Member = value.Member;
            Key = value.GetHashKey64(value.GetHashSeed());
        }
        public override void Set(ICard<LinkBranch> card)
        {
            this.value = card.Value;
            this.Key = card.Key;
        }

        public override bool Equals(long key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.GetHashKey());
        }
        public          bool Equals(LinkBranch other)
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
        public override int CompareTo(ICard<LinkBranch> other)
        {
            return (int)(Key - other.Key);
        }
        public          int CompareTo(LinkBranch other)
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
            return Member.KeyRubrics.Ordinals;
        }
        public override object[] IdentityValues()
        {
            if (this.value.Count > 0)
                return this.value[0].IdentityValues();
            return null;      
        }
        public override     long IdentitiesToKey()
        {
            if (this.value.Count > 0)
                return this.value[0].IdentitiesToKey();
            return -1;
        }  

        public override long Key
        {
            get => _key;                                                   
            set => _key = value;            
        }

        public override long KeyBlock
        {
            get => value.KeyBlock;
            set => this.value.KeyBlock = value;
        }      
       
        public LinkMember Member { get; set; } 

        public LinkBranches Branches { get; set; }

        public override IUnique Empty => this.value.Empty;

        public override uint SeedBlock
        { get => Member.SeedBlock; set => Member.SetHashSeed(value); }

        public override int CompareTo(IUnique other)
        {
            return this.value.CompareTo(other);
        }

        public override bool Equals(IUnique other)
        {
            return this.value.Equals(other);
        }

        public override long GetHashKey()
        {
            return this.value.GetHashKey();
        }

        public override void SetHashKey(long value)
        {
            this.value.SetHashKey(value);
        }

        public override void SetHashSeed(uint seed)
        {
            Member.SetHashSeed(seed);
        }

        public override uint GetHashSeed()
        {
            return Member.GetHashSeed();
        }

    }
}
