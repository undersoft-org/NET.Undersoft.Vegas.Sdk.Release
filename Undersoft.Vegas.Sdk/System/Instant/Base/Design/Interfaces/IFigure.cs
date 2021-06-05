using System.Uniques;

namespace System.Instant
{
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