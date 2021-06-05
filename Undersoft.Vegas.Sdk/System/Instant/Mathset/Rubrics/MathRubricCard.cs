using System.Runtime.InteropServices;
using System.Sets;
using System.Uniques;

/**
    Copyright (c) 2019 Undersoft

    System.Instant.Mathset.MathsetCard
        
    @author Darius Hanc                                                  
    @version 0.8.D (Dec 29, 2019)                                            
 
 **/
namespace System.Instant.Mathset
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MathRubricCard : Card<MathRubric>
    {
        private ulong _key;

        public MathRubricCard()
        { }
        public MathRubricCard(object key, MathRubric value) : base(key.UniqueKey64(), value)
        {
        }
        public MathRubricCard(long key, MathRubric value) : base(key, value)
        {
        }
        public MathRubricCard(MathRubric value) : base(value)
        {
        }
        public MathRubricCard(ICard<MathRubric> value) : base(value)
        {
        }

        public override void Set(object key, MathRubric value)
        {
            this.value = value;
            _key = key.UniqueKey64();
        }
        public override void Set(MathRubric value)
        {
            this.value = value;
            _key = value.UniqueKey;
        }
        public override void Set(ICard<MathRubric> card)
        {
            this.value = card.Value;
            _key = card.Key;     
        }

        public override bool Equals(ulong key)
        {
            return _key == key;
        }
        public override bool Equals(object y)
        {
            return _key.Equals(y.UniqueKey64());
        }

        public override int GetHashCode()
        {
            return (int)_key;
        }

        public override int CompareTo(object other)
        {
            return (int)(_key - other.UniqueKey64());
        }
        public override int CompareTo(ulong key)
        {
            return (int)(_key - key);
        }
        public override int CompareTo(ICard<MathRubric> other)
        {
            return (int)(_key - other.Key);
        }

        public override byte[] GetBytes()
        {
            return GetUniqueBytes();
        }

        public unsafe override byte[] GetUniqueBytes()
        {
            byte[] b = new byte[8];
            fixed (byte* s = b)
                *(ulong*)s = _key;
            return b;
        }

        public override ulong Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
    }
}
