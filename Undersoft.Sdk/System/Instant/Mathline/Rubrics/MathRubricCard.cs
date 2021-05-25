using System.Runtime.InteropServices;
using System.Multemic;
using System.Uniques;

/**
    Copyright (c) 2019 Undersoft

    System.Instant.Mathline.MathlineCard
        
    @author Darius Hanc                                                  
    @version 0.8.D (Dec 29, 2019)                                            
 
 **/
namespace System.Instant.Mathline
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MathRubricCard : Card<MathRubric>
    {
        private long _key;

        public MathRubricCard()
        { }
        public MathRubricCard(object key, MathRubric value) : base(key.GetHashKey64(), value)
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
            _key = key.GetHashKey64();
        }
        public override void Set(MathRubric value)
        {
            this.value = value;
            _key = value.GetHashKey();
        }
        public override void Set(ICard<MathRubric> card)
        {
            this.value = card.Value;
            _key = card.Key;     
        }

        public override bool Equals(long key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.GetHashKey64());
        }

        public override int GetHashCode()
        {
            return (int)Key;
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.GetHashKey64());
        }
        public override int CompareTo(long key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<MathRubric> other)
        {
            return (int)(Key - other.Key);
        }

        public override byte[] GetBytes()
        {
            return GetKeyBytes();
        }

        public unsafe override byte[] GetKeyBytes()
        {
            byte[] b = new byte[8];
            fixed (byte* s = b)
                *(long*)s = _key;
            return b;
        }

        public override long Key
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
