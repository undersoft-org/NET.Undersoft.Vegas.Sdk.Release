/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.Uniqueness.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Collections;

    #region Enums

    public enum HashBits
    {
        bit64,
        bit32
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="IUniqueness" />.
    /// </summary>
    public interface IUniqueness
    {
        #region Methods

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        Byte[] Bytes(Byte[] obj, ulong seed = 0);

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        Byte[] Bytes(IList obj, ulong seed = 0);

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        Byte[] Bytes(IUnique obj);

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        Byte[] Bytes(Object obj, ulong seed = 0);

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        Byte[] Bytes(string obj, ulong seed = 0);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(Byte[] obj, ulong seed = 0);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(IList obj, ulong seed = 0);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(IUnique obj);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(IUnique obj, ulong seed);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key<V>(IUnique<V> obj);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(Object obj, ulong seed = 0);

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        UInt64 Key(string obj, ulong seed = 0);

        #endregion
    }
    /// <summary>
    /// Defines the <see cref="Unique32" />.
    /// </summary>
    public class Unique32 : Uniqueness
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Unique32"/> class.
        /// </summary>
        public Unique32() : base(HashBits.bit32)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public unsafe override
                        Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey();
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return Key(obj.GetBytes(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="Unique64" />.
    /// </summary>
    public class Unique64 : Uniqueness
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Unique64"/> class.
        /// </summary>
        public Unique64() : base(HashBits.bit64)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public override unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="Uniqueness" />.
    /// </summary>
    public abstract class Uniqueness : IUniqueness
    {
        #region Fields

        protected Uniqueness unique;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Uniqueness"/> class.
        /// </summary>
        public Uniqueness()
        {
            unique = Unique.Bit64;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Uniqueness"/> class.
        /// </summary>
        /// <param name="hashBits">The hashBits<see cref="HashBits"/>.</param>
        public Uniqueness(HashBits hashBits)
        {
            if (hashBits == HashBits.bit32)
                unique = Unique.Bit32;
            else
                unique = Unique.Bit64;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual Byte[] Bytes(string obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeBytes.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        public virtual unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return unique.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// The ComputeKey.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Byte[]"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IList"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(IList obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IntPtr"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="IUnique"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(IUnique obj, ulong seed)
        {
            return unique.ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.UniquesAsKey();
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <typeparam name="V">.</typeparam>
        /// <param name="obj">The obj<see cref="IUnique{V}"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return unique.Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="Object"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(Object obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="string"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        public virtual UInt64 Key(string obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// The Bytes.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="Byte[]"/>.</returns>
        protected virtual unsafe
                       Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }

        /// <summary>
        /// The Key.
        /// </summary>
        /// <param name="obj">The obj<see cref="byte*"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="seed">The seed<see cref="ulong"/>.</param>
        /// <returns>The <see cref="UInt64"/>.</returns>
        protected virtual unsafe
                       UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }

        #endregion
    }
}
