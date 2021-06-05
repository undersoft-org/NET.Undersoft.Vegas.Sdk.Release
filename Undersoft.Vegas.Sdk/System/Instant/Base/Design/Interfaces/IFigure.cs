/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IFigure.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Uniques;

    public interface IFigure : IUnique
    {
        object this[string propertyName] { get; set; }

        object this[int fieldId] { get; set; }

        object[] ValueArray { get; set; }

        Ussn SerialCode { get; set; }

        //V Get<V>(string propertyName);

        //void Set<V>(string propertyName, V value);
    }
}
