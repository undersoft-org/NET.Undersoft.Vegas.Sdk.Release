using System.Collections;
using System.Extract;
using System.Runtime.CompilerServices;
using System.Uniques;

namespace System.Uniques
{
    public class Unique32 : Uniqueness
    {
        public Unique32() : base(HashBits.bit32)
        {            
        }

        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, length, seed);
        }
        public override unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, seed);
        }
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, length, seed);
        }
        public override unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, seed);
        }

        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }
        public unsafe override 
                        Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return Key(obj.GetBytes(), seed);
        }
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey();
        }
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }
    }

    public class Unique64 : Uniqueness
    {
        public Unique64() : base(HashBits.bit64)
        {
        }

        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, length, seed);
        }
        public override unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, seed);
        }
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, length, seed);
        }
        public override unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, seed);
        }

        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }
        public override Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return ComputeKey(obj.GetUniqueBytes(), seed);
        }
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }
    }

    public abstract class Uniqueness : IUniqueness
    {
        protected Uniqueness unique;

        public Uniqueness()
        {
            unique = Unique.Bit64;
        }
        public Uniqueness(HashBits hashBits)
        {
            if (hashBits == HashBits.bit32)
                unique = Unique.Bit32;
            else
                unique = Unique.Bit64;
        }

        public virtual unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeBytes(bytes, length, seed);
        }
        public virtual unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
         {
            return unique.ComputeBytes(bytes, seed);
        }
        public virtual unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, length, seed);
        }
        public virtual unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, seed);
        }

        protected virtual unsafe 
                       Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }
        public virtual Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }

        public virtual Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }
        public virtual Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }
        public virtual Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }
        public virtual Byte[] Bytes(string obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }
        public virtual Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        protected virtual unsafe
                       UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }

        public virtual UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }
        public virtual UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }
        public virtual UInt64 Key(Object obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }
        public virtual UInt64 Key(IList obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }
        public virtual UInt64 Key(string obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }
        public virtual UInt64 Key(IUnique obj, ulong seed)
        {
            return unique.ComputeKey(obj.GetUniqueBytes(), seed);
        }
        public virtual UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }
        public virtual UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return unique.Key(obj.UniqueValues(), seed);
        }
        public virtual UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }
    }

    public interface IUniqueness
    {
        Byte[] Bytes(Byte[] obj, ulong seed = 0);
        Byte[] Bytes(Object obj, ulong seed = 0);
        Byte[] Bytes(IList obj, ulong seed = 0);
        Byte[] Bytes(string obj, ulong seed = 0);
        Byte[] Bytes(IUnique obj);

        UInt64 Key(Byte[] obj, ulong seed = 0);
        UInt64 Key(Object obj, ulong seed = 0);
        UInt64 Key(IList obj, ulong seed = 0);
        UInt64 Key(string obj, ulong seed = 0);
        UInt64 Key(IUnique obj, ulong seed);
        UInt64 Key(IUnique obj);
        UInt64 Key<V>(IUnique<V> obj);
    }

    public enum HashBits
    {
        bit64,
        bit32
    }
}

