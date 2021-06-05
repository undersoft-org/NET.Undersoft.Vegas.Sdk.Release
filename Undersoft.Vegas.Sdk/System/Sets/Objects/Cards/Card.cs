using System.Runtime.InteropServices;
using System.Uniques;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Card64
    
    Implementation of Card abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ******************************************************************/
namespace System.Sets
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Card<V> : BaseCard<V>
    {
        private ulong _key;

        public Card()
        { }
        public Card(object key, V value) : base(key, value)
        {
        }
        public Card(ulong key, V value) : base(key, value)
        {
        }
        public Card(V value) : base(value)
        {
        }
        public Card(ICard<V> value) : base(value)
        {
        }

        public override void Set(object key, V value)
        {
            this.value = value;
            _key = key.UniqueKey64();
        }
        public override void Set(V value)
        {
            this.value = value;
            _key = value.UniqueKey64();
        }
        public override void Set(ICard<V> card)
        {
            this.value = card.Value;
            _key = card.Key;
        }

        public override bool Equals(ulong key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.UniqueKey64());
        }

        public override int GetHashCode()
        {
            return (int)Key;
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.UniqueKey64());
        }
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
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
