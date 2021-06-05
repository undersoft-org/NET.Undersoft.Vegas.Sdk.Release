/*************************************************
   Copyright (c) 2021 Undersoft

   LaborMethod.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Extract;
    using System.Instant;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="LaborMethod" />.
    /// </summary>
    public class LaborMethod : Card<IDeputy>
    {
        #region Fields

        /// <summary>
        /// Defines the key.
        /// </summary>
        private ulong key;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod"/> class.
        /// </summary>
        public LaborMethod()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        public LaborMethod(IDeputy value)
        {
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="long"/>.</param>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        public LaborMethod(long key, IDeputy value) : base(key, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LaborMethod"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        public LaborMethod(object key, IDeputy value) : base(key.UniqueKey64(), value)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public override ulong Key { get => key; set => key = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int CompareTo(object other)
        {
            return (int)(UniqueKey - other.UniqueKey());
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="y">The y<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object y)
        {
            return UniqueKey == y.UniqueKey64();
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public override byte[] GetBytes()
        {
            return Key.GetBytes();
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return Key.GetBytes().BitAggregate64to32().ToInt32();
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public override byte[] GetUniqueBytes()
        {
            return Key.GetBytes();
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="card">The card<see cref="ICard{IDeputy}"/>.</param>
        public override void Set(ICard<IDeputy> card)
        {
            Key = card.Key;
            Value = card.Value;
            Removed = false;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        public override void Set(IDeputy value)
        {
            Value = value;
            Removed = false;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        public override void Set(object key, IDeputy value)
        {
            Key = key.UniqueKey64();
            Value = value;
            Removed = false;
        }

        #endregion
    }
}
