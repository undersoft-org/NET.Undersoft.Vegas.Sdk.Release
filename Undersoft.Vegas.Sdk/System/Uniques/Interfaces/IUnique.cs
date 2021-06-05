/*************************************************
   Copyright (c) 2021 Undersoft

   System.IUnique.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{  
    public interface IUnique : IEquatable<IUnique>, IComparable<IUnique>
    {
        #region Properties

        IUnique Empty { get; }

        ulong UniqueKey { get; set; }

        ulong UniqueSeed { get; set; }

        #endregion

        #region Methods

        byte[] GetBytes();

        byte[] GetUniqueBytes();

        #endregion
    }
}
