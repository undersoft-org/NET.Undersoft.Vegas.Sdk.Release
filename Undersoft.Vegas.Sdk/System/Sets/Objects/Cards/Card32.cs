/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.Card32.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.Card32
    
    Implementation of Card abstract class
    using 32 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                           
 
 ******************************************************************/
namespace System.Sets
{
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Card32{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Card32<V> : BaseCard<V>
    {
        #region Fields

        private uint _key;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}"/> class.
        /// </summary>
        public Card32()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{V}"/>.</param>
        public Card32(ICard<V> value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public Card32(object key, V value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public Card32(ulong key, V value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Card32{V}"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        public Card32(V value) : base(value)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public override ulong Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = (uint)value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="ICard{V}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(ICard<V> other)
        {
            return (int)(Key - other.Key);
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(object other)
        {
            return (int)(_key - other.UniqueKey32());
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object y)
        {
            return _key.Equals(y.UniqueKey32());
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(ulong key)
        {
            return Key == key;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public override byte[] GetBytes()
        {
            return GetUniqueBytes();
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return (int)_key;
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public unsafe override byte[] GetUniqueBytes()
        {
            byte[] b = new byte[4];
            fixed (byte* s = b)
                *(uint*)s = _key;
            return b;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{V}"/>.</param>
        public override void Set(ICard<V> card)
        {
            this.value = card.Value;
            _key = (uint)card.Key;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="V"/>.</param>
        public override void Set(object key, V value)
        {
            this.value = value;
            _key = key.UniqueKey32();
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="V"/>.</param>
        public override void Set(V value)
        {
            this.value = value;
            _key = value.UniqueKey32();
        }

        #endregion
    }
}
