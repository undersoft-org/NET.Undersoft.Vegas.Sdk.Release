using System.Collections;
using System.Extract;
using System.Runtime.InteropServices;
using System.Uniques;

namespace System.Uniques
{
    public class HashKey32 : HashKey
    {
        public HashKey32() : base(HashBits.bit32)
        {            
        }

        public override unsafe Byte[] ComputeHashBytes(byte* bytes, int length, uint seed = 0)
        {
            return HashHandle32.ComputeHashBytes(bytes, length, seed);
        }
        public override unsafe Byte[] ComputeHashBytes(byte[] bytes, uint seed = 0)
        {
            return HashHandle32.ComputeHashBytes(bytes, seed);
        }
        public override unsafe UInt64 ComputeHashKey(byte* bytes, int length, uint seed = 0)
        {
            return HashHandle32.ComputeHashKey(bytes, length, seed);
        }
        public override unsafe UInt64 ComputeHashKey(byte[] bytes, uint seed = 0)
        {
            return HashHandle32.ComputeHashKey(bytes, seed);
        }

        public override unsafe Byte[] GetHashBytes(byte* obj, int length, uint seed = 0)
        {
            return ComputeHashBytes(obj, length, seed);
        }
        public override unsafe Byte[] GetHashBytes(IntPtr obj, int length, uint seed = 0)
        {
            return ComputeHashBytes((byte*)obj.ToPointer(), length, seed);
        }

        public override Byte[] GetHashBytes(Byte[] obj, uint seed = 0)
        {
            return obj.GetHashBytes32(seed);
        }
        public override Byte[] GetHashBytes(Object obj, uint seed = 0)
        {
            return obj.GetHashBytes32(seed);
        }
        public override Byte[] GetHashBytes(IList obj, uint seed = 0)
        {
            return obj.GetHashBytes32(seed);
        }
        public unsafe override 
                        Byte[] GetHashBytes(string obj, uint seed = 0)
        {
            return obj.GetHashBytes32(seed);
        }
        public override Byte[] GetHashBytes(IUnique obj)
        {
            return obj.GetKeyBytes();
        }

        public override unsafe Int64 GetHashKey(byte* obj, int length, uint seed = 0)
        {
            return (long)ComputeHashKey(obj, length, seed);
        }
        public override unsafe Int64 GetHashKey(IntPtr obj, int length, uint seed = 0)
        {
            return (long)ComputeHashKey((byte*)obj.ToPointer(), length, seed);
        }

        public override Int64 GetHashKey(Byte[] obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public override Int64 GetHashKey(Object obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public override Int64 GetHashKey(IList obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public override Int64 GetHashKey(string obj, uint seed = 0)
        {
            return obj.GetHashKey32(seed);
        }
        public override Int64 GetHashKey(IUnique obj, uint seed)
        {
            return GetHashKey(obj.GetKeyBytes(), seed);
        }
        public override Int64 GetHashKey(IUnique obj)
        {
            return obj.GetHashKey();
        }
        public override Int64 GetHashKey<V>(IUnique<V> obj, uint seed)
        {
            return GetHashKey(obj.IdentityValues(), seed);
        }
        public override Int64 GetHashKey<V>(IUnique<V> obj)
        {
            return obj.IdentitiesToKey();
        }
    }

    public class HashKey64 : HashKey
    {
        public HashKey64() : base(HashBits.bit64)
        {
        }

        public override unsafe Byte[] ComputeHashBytes(byte* bytes, int length, uint seed = 0)
        {
            return HashHandle64.ComputeHashBytes(bytes, length, seed);
        }
        public override unsafe Byte[] ComputeHashBytes(byte[] bytes, uint seed = 0)
        {
            return HashHandle64.ComputeHashBytes(bytes, seed);
        }
        public override unsafe UInt64 ComputeHashKey(byte* bytes, int length, uint seed = 0)
        {
            return HashHandle32.ComputeHashKey(bytes, length, seed);
        }
        public override unsafe UInt64 ComputeHashKey(byte[] bytes, uint seed = 0)
        {
            return HashHandle32.ComputeHashKey(bytes, seed);
        }

        public override unsafe Byte[] GetHashBytes(byte* obj, int length, uint seed = 0)
        {
            return ComputeHashBytes(obj, length, seed);
        }
        public override unsafe Byte[] GetHashBytes(IntPtr obj, int length, uint seed = 0)
        {
            return ComputeHashBytes((byte*)obj.ToPointer(), length, seed);
        }

        public override Byte[] GetHashBytes(Byte[] obj, uint seed = 0)
        {
            return obj.GetHashBytes64(seed);
        }
        public override Byte[] GetHashBytes(Object obj, uint seed = 0)
        {
            return obj.GetHashBytes64(seed);
        }
        public override Byte[] GetHashBytes(IList obj, uint seed = 0)
        {
            return obj.GetHashBytes64(seed);
        }
        public override Byte[] GetHashBytes(string obj, uint seed = 0)
        {
            return obj.GetHashBytes64(seed);
        }
        public override Byte[] GetHashBytes(IUnique obj)
        {
            return obj.GetKeyBytes();
        }

        public override unsafe Int64 GetHashKey(byte* obj, int length, uint seed = 0)
        {
            return (long)ComputeHashKey(obj, length, seed);
        }
        public override unsafe Int64 GetHashKey(IntPtr obj, int length, uint seed = 0)
        {
            return (long)ComputeHashKey((byte*)obj.ToPointer(), length, seed);
        }

        public override Int64 GetHashKey(Byte[] obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public override Int64 GetHashKey(Object obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public override Int64 GetHashKey(IList obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public override Int64 GetHashKey(string obj, uint seed = 0)
        {
            return obj.GetHashKey64(seed);
        }
        public override Int64 GetHashKey(IUnique obj, uint seed)
        {
            return GetHashKey(obj.GetKeyBytes(), seed);
        }
        public override Int64 GetHashKey(IUnique obj)
        {
            return obj.GetHashKey();
        }
        public override Int64 GetHashKey<V>(IUnique<V> obj, uint seed)
        {
            return GetHashKey(obj.IdentityValues(), seed);
        }
        public override Int64 GetHashKey<V>(IUnique<V> obj)
        {
            return obj.IdentitiesToKey();
        }
    }

    public abstract class HashKey : IHashKey
    {
        protected HashKey _base;

        public HashKey()
        {
            _base = Hash.Key64;
        }
        public HashKey(HashBits hashBits)
        {
            if (hashBits == HashBits.bit32)
                _base = Hash.Key32;
            else
                _base = Hash.Key64;
        }

        public virtual unsafe Byte[] ComputeHashBytes(byte* bytes, int length, uint seed = 0)
        {
            return _base.ComputeHashBytes(bytes, length, seed);
        }
        public virtual unsafe Byte[] ComputeHashBytes(byte[] bytes, uint seed = 0)
         {
            return _base.ComputeHashBytes(bytes, seed);
        }
        public virtual unsafe UInt64 ComputeHashKey(byte* bytes, int length, uint seed = 0)
        {
            return _base.ComputeHashKey(bytes, length, seed);
        }
        public virtual unsafe UInt64 ComputeHashKey(byte[] bytes, uint seed = 0)
        {
            return _base.ComputeHashKey(bytes, seed);
        }

        public virtual unsafe 
                       Byte[] GetHashBytes(byte* obj, int length, uint seed = 0)
        {
            return _base.GetHashBytes(obj, length, seed);
        }
        public virtual Byte[] GetHashBytes(IntPtr obj, int length, uint seed = 0)
        {
            return _base.GetHashBytes(obj, length, seed);
        }

        public virtual Byte[] GetHashBytes(Byte[] obj, uint seed = 0)
        {
            return _base.GetHashBytes(obj, seed);
        }
        public virtual Byte[] GetHashBytes(Object obj, uint seed = 0)
        {
            return _base.GetHashBytes(obj, seed);
        }
        public virtual Byte[] GetHashBytes(IList obj, uint seed = 0)
        {
            return _base.GetHashBytes(obj, seed);
        }
        public virtual Byte[] GetHashBytes(string obj, uint seed = 0)
        {
            return _base.GetHashBytes(obj, seed);
        }
        public virtual Byte[] GetHashBytes(IUnique obj)
        {
            return obj.GetKeyBytes();
        }

        public virtual unsafe 
                       Int64 GetHashKey(byte* obj, int length, uint seed = 0)
        {
            return _base.GetHashKey(obj, length, seed);
        }
        public virtual Int64 GetHashKey(IntPtr obj, int length, uint seed = 0)
        {
            return _base.GetHashKey(obj, length, seed);
        }
        public virtual Int64 GetHashKey(Byte[] obj, uint seed = 0)
        {
            return _base.GetHashKey(obj, seed);
        }
        public virtual Int64 GetHashKey(Object obj, uint seed = 0)
        {
            return _base.GetHashKey(obj, seed);
        }
        public virtual Int64 GetHashKey(IList obj, uint seed = 0)
        {
            return _base.GetHashKey(obj, seed);
        }
        public virtual Int64 GetHashKey(string obj, uint seed = 0)
        {
            return _base.GetHashKey(obj, seed);
        }
        public virtual Int64 GetHashKey(IUnique obj, uint seed)
        {
            return _base.GetHashKey(obj.GetHashKey(), seed);
        }
        public virtual Int64 GetHashKey(IUnique obj)
        {
            return obj.GetHashKey();
        }
        public virtual Int64 GetHashKey<V>(IUnique<V> obj, uint seed)
        {
            return _base.GetHashKey(obj.IdentityValues(), seed);
        }
        public virtual Int64 GetHashKey<V>(IUnique<V> obj)
        {
            return obj.IdentitiesToKey();
        }
    }

    public interface IHashKey
    {
        Byte[] GetHashBytes(Byte[] obj, uint seed = 0);
        Byte[] GetHashBytes(Object obj, uint seed = 0);
        Byte[] GetHashBytes(IList obj, uint seed = 0);
        Byte[] GetHashBytes(string obj, uint seed = 0);
        Byte[] GetHashBytes(IUnique obj);

        Int64 GetHashKey(Byte[] obj, uint seed = 0);
        Int64 GetHashKey(Object obj, uint seed = 0);
        Int64 GetHashKey(IList obj, uint seed = 0);
        Int64 GetHashKey(string obj, uint seed = 0);
        Int64 GetHashKey(IUnique obj, uint seed);
        Int64 GetHashKey(IUnique obj);
        Int64 GetHashKey<V>(IUnique<V> obj);
    }

    public enum HashBits
    {
        bit64,
        bit32
    }
}

