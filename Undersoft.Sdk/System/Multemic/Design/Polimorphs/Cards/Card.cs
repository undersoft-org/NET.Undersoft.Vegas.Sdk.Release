using System;
using System.Uniques;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

/******************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.Card
    
    Card abstract class. 
    Reference type of common used 
    value type Bucket in Hashtables.
    Include properties: 
    Key - long abstract property to implement different
          type fields with hashkey like long, int etc.
    Value - Generic type property to store collection item.
    Next - for one site list implementation. 
    Extent - for one site list hash conflict items
    Removed - flag for removed items to skip before
              removed items limit exceed and rehash
              process executed
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                          
 
 ****************************************/
namespace System.Multemic
{
    [StructLayout(LayoutKind.Sequential)] [Serializable]
    public abstract class Card<V> : IEquatable<ICard<V>>, IEquatable<object>, IEquatable<long>, IComparable<object>, IComparable<long>, IComparable<ICard<V>>, ICard<V>
    {
        [NonSerialized] private ICard<V> extent;
        [NonSerialized] private ICard<V> next;
                      protected       V  value;
                      protected      int index = -1;
                      protected     bool removed;

        public Card()
        { }
        public Card(long key, V value)
        {
            Set(key, value);
        }
        public Card(object key, V value)
        {
            Set(key, value);
        }
        public Card(ICard<V> value)
        {
            Set(value);
        }
        public Card(V value)
        {
            Set(value);
        }

        public int    Index { get => index; set => index = value; }
        public bool Removed { get => removed; set => removed = value; }
        
        public abstract long Key { get; set; }
        
        public             V Value { get => value; set => this.value = value; }

        public abstract void Set(V value);     
        public virtual  void Set(long key, V value)
        {
            this.value = value;
            Key = key;
        }
        public abstract void Set(object key, V value);
        public abstract void Set(ICard<V> card);

        public virtual bool Equals(IUnique other)
        {
            return Key == other.KeyBlock;
        }
        public virtual bool Equals(ICard<V> y)
        {
            return this.Equals(y.Key);
        }
        public virtual bool Equals(long key)
        {
            return Key == key;
        }

        public virtual void SetHashKey(long hashcode)
        {
            KeyBlock = hashcode;
        }
        public virtual long GetHashKey()
        {
            return Key;
        }

        public virtual void SetHashSeed(uint hashseed)
        {
            SeedBlock = hashseed;
        }
        public virtual uint GetHashSeed()
        {
            return SeedBlock;
        }

        public override abstract bool Equals(object y);
 
        public override abstract int GetHashCode();

        public virtual  int CompareTo(IUnique other)
        {
            return (int)(Key - other.GetHashKey());
        }
        public abstract int CompareTo(object other);
        public virtual  int CompareTo(long key)
        {
            return (int)(Key - key);
        }
        public virtual  int CompareTo(ICard<V> other)
        {
            return (int) (Key - other.Key);         
        }

        public abstract byte[] GetBytes();
        public abstract byte[] GetKeyBytes();

        public virtual ICard<V> Extent { get => extent; set => extent = value; }
        public virtual ICard<V> Next { get => next; set => next = value; }

        public virtual IUnique Empty => throw new NotImplementedException();

        public virtual long KeyBlock { get => Key; set => Key = value; }

        public virtual uint SeedBlock { get => 0; set => throw new NotImplementedException(); }

        public virtual Type GetUniqueType() { return this.GetType(); }

        public virtual int[] IdentityIndexes()
        {
            return null;
        }

        public virtual object[] IdentityValues()
        {
            return  new object[] { Key };
        }

        public virtual long IdentitiesToKey()
        {
            return Key;
        }

        #region IDisposable
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Value = default(V);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }    
}
