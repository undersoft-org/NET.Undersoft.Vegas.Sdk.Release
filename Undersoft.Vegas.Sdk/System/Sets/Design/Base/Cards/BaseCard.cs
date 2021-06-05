using System;
using System.Uniques;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

/******************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Card
    
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
namespace System.Sets
{
    [StructLayout(LayoutKind.Sequential)]
    public abstract class BaseCard<V> : IEquatable<ICard<V>>, IEquatable<object>, IEquatable<ulong>, IComparable<object>, 
                                    IComparable<ulong>, IComparable<ICard<V>>, ICard<V>
    {       
        protected       V  value;
        private   ICard<V> extent;
        private   ICard<V> next;

        public BaseCard()
        { }
        public BaseCard(ulong key, V value)
        {
            Set(key, value);
        }
        public BaseCard(object key, V value)
        {
            Set(key, value);
        }
        public BaseCard(ICard<V> value)
        {
            Set(value);
        }
        public BaseCard(V value)
        {
            Set(value);
        }

        public virtual int Index { get; set; } = -1;
        public virtual bool Removed { get; set; }
        
        public abstract ulong Key { get; set; }
        
        public             V Value { get => value; set => this.value = value; }

        public abstract void Set(V value);     
        public virtual  void Set(ulong key, V value)
        {
            this.value = value;
            Key = key;
        }
        public abstract void Set(object key, V value);
        public abstract void Set(ICard<V> card);

        public virtual bool Equals(IUnique other)
        {
            return Key == other.UniqueKey;
        }
        public virtual bool Equals(ICard<V> y)
        {
            return this.Equals(y.Key);
        }
        public virtual bool Equals(ulong key)
        {
            return Key == key;
        }

        public override abstract bool Equals(object y);
 
        public override abstract int GetHashCode();

        public virtual  int CompareTo(IUnique other)
        {
            return (int)(Key - other.UniqueKey);
        }
        public abstract int CompareTo(object other);
        public virtual  int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }
        public virtual  int CompareTo(ICard<V> other)
        {
            return (int) (Key - other.Key);         
        }

        public abstract byte[] GetBytes();
        public abstract byte[] GetUniqueBytes();

        public virtual ICard<V> Extent { get => extent; set => extent = value; }
        public virtual ICard<V> Next { get => next; set => next = value; }

        public virtual IUnique Empty => throw new NotImplementedException();

        public virtual ulong UniqueKey { get => Key; set => Key = value; }

        public virtual ulong UniqueSeed { get => 0; set => throw new NotImplementedException(); }

        public virtual Type GetUniqueType() { return this.GetType(); }

        public virtual int[] UniqueOrdinals()
        {
            return null;
        }

        public virtual object[] UniqueValues()
        {
            return new object[] { Key };
        }

        public virtual ulong UniquesAsKey()
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
