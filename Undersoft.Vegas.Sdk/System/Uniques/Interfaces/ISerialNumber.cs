/*************************************************
   Copyright (c) 2021 Undersoft

   System.ISerialNumber.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Collections.Specialized;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="ISerialNumber{V}" />.
    /// </summary>
    /// <typeparam name="V">.</typeparam>
    public interface ISerialNumber<V> : ISerialNumber
    {
        #region Properties

        /// <summary>
        /// Gets the IdentifierType.
        /// </summary>
        Type IdentifierType { get; }

        /// <summary>
        /// Gets the KeyFields.
        /// </summary>
        FieldInfo[] KeyFields { get; }

        /// <summary>
        /// Gets the Value.
        /// </summary>
        V Value { get; }

        #endregion
    }
    /// <summary>
    /// Defines the <see cref="ISerialNumber" />.
    /// </summary>
    public interface ISerialNumber : IUnique, IEquatable<BitVector32>, IEquatable<DateTime>, IEquatable<ISerialNumber>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the BlockX.
        /// </summary>
        ushort BlockX { get; set; }

        /// <summary>
        /// Gets or sets the BlockY.
        /// </summary>
        ushort BlockY { get; set; }

        /// <summary>
        /// Gets or sets the BlockZ.
        /// </summary>
        ushort BlockZ { get; set; }

        /// <summary>
        /// Gets or sets the FlagsBlock.
        /// </summary>
        ushort FlagsBlock { get; set; }

        /// <summary>
        /// Gets or sets the TimeBlock.
        /// </summary>
        long TimeBlock { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The ValueFromXYZ.
        /// </summary>
        /// <param name="vectorZ">The vectorZ<see cref="uint"/>.</param>
        /// <param name="vectorY">The vectorY<see cref="uint"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        ulong ValueFromXYZ(uint vectorZ, uint vectorY);

        /// <summary>
        /// The ValueToXYZ.
        /// </summary>
        /// <param name="vectorZ">The vectorZ<see cref="ulong"/>.</param>
        /// <param name="vectorY">The vectorY<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ushort[]"/>.</returns>
        ushort[] ValueToXYZ(ulong vectorZ, ulong vectorY, ulong value);

        #endregion
    }
}
