using System.Runtime.InteropServices;
using System.Uniques;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Card32
    
    Implementation of Card abstract class
    using 32 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                           
 
 ******************************************************************/
namespace System.Multemic
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Card32<V> : Card<V>
    {
        private int _key;

        public Card32()
        { }
        public Card32(object key, V value) : base(key, value)
        {
        }
        public Card32(long key, V value) : base(key, value)
        {
        }
        public Card32(V value) : base(value)
        {
        }
        public Card32(ICard<V> value) : base(value)
        {
        }

        public override void Set(object key, V value)
        {
            this.value = value;
            _key = key.GetHashKey32();
        }
        public override void Set(V value)
        {            
            this.value = value;
            _key = value.GetHashKey32();
        }
        public override void Set(ICard<V> card)
        {
            this.value = card.Value;
            _key = (int)card.Key;
        }

        public override bool Equals(long key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return _key.Equals(y.GetHashKey32());
        }

        public override int GetHashCode()
        {
            return _key;
        }

        public override int CompareTo(object other)
        {
            return (_key - other.GetHashKey32());
        }
        public override int CompareTo(long key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
        }

        public override byte[] GetBytes()
        {
            return GetKeyBytes();
        }

        public unsafe override byte[] GetKeyBytes()
        {
            byte[] b = new byte[4];
            fixed (byte* s = b)
                *(int*)s = _key;
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
                _key = (int)value;
            }
        }
    }
}
