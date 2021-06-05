/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathRubricCard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

/**
    Copyright (c) 2019 Undersoft

    System.Instant.Mathset.MathsetCard
        
    @author Darius Hanc                                                  
    @version 0.8.D (Dec 29, 2019)                                            
 
 **/
namespace System.Instant.Mathset
{
    using System.Runtime.InteropServices;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="MathRubricCard" />.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MathRubricCard : Card<MathRubric>
    {
        #region Fields

        private ulong _key;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard"/> class.
        /// </summary>
        public MathRubricCard()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{MathRubric}"/>.</param>
        public MathRubricCard(ICard<MathRubric> value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="long"/>.</param>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        public MathRubricCard(long key, MathRubric value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        public MathRubricCard(MathRubric value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubricCard"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        public MathRubricCard(object key, MathRubric value) : base(key.UniqueKey64(), value)
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
                _key = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="ICard{MathRubric}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(ICard<MathRubric> other)
        {
            return (int)(_key - other.Key);
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(object other)
        {
            return (int)(_key - other.UniqueKey64());
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(ulong key)
        {
            return (int)(_key - key);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object y)
        {
            return _key.Equals(y.UniqueKey64());
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(ulong key)
        {
            return _key == key;
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
            byte[] b = new byte[8];
            fixed (byte* s = b)
                *(ulong*)s = _key;
            return b;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{MathRubric}"/>.</param>
        public override void Set(ICard<MathRubric> card)
        {
            this.value = card.Value;
            _key = card.Key;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        public override void Set(MathRubric value)
        {
            this.value = value;
            _key = value.UniqueKey;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        public override void Set(object key, MathRubric value)
        {
            this.value = value;
            _key = key.UniqueKey64();
        }

        #endregion
    }
}
