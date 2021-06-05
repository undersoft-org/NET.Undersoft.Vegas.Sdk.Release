/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.RubricCard.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Runtime.InteropServices;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="RubricCard" />.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class RubricCard : BaseCard<MemberRubric>
    {
        #region Fields

        private ulong _key;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RubricCard"/> class.
        /// </summary>
        public RubricCard()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RubricCard"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{MemberRubric}"/>.</param>
        public RubricCard(ICard<MemberRubric> value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RubricCard"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        public RubricCard(MemberRubric value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RubricCard"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        public RubricCard(object key, MemberRubric value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RubricCard"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        public RubricCard(ulong key, MemberRubric value) : base(key, value)
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
        /// <param name="other">The other<see cref="ICard{MemberRubric}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(ICard<MemberRubric> other)
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
            return (int)(Key - other.UniqueKey64());
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
            return Key.Equals(y.UniqueKey64());
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
            return (int)Key;
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
        /// <param name="card">The card<see cref="ICard{MemberRubric}"/>.</param>
        public override void Set(ICard<MemberRubric> card)
        {
            this.value = card.Value;
            _key = card.Key;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        public override void Set(MemberRubric value)
        {
            this.value = value;
            _key = value.UniqueKey;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        public override void Set(object key, MemberRubric value)
        {
            this.value = value;
            _key = key.UniqueKey64();
        }

        #endregion
    }
}
