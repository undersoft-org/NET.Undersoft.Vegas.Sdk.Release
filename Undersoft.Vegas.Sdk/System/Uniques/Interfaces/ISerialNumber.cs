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

    public interface ISerialNumber<V> : ISerialNumber
    {
        #region Properties

        Type IdentifierType { get; }

        FieldInfo[] KeyFields { get; }

        V Value { get; }

        #endregion
    }
    public interface ISerialNumber : IUnique, IEquatable<BitVector32>, IEquatable<DateTime>, IEquatable<ISerialNumber>
    {
        #region Properties

        ushort BlockX { get; set; }

        ushort BlockY { get; set; }

        ushort BlockZ { get; set; }

        ushort FlagsBlock { get; set; }

        long TimeBlock { get; set; }

        #endregion

        #region Methods

        ulong ValueFromXYZ(uint vectorZ, uint vectorY);

        ushort[] ValueToXYZ(ulong vectorZ, ulong vectorY, ulong value);

        #endregion
    }
}
