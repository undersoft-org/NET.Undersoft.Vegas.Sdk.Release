using System.Instant.Linkmap;
using System.Instant.Treatment;

namespace System.Instant
{      
    public interface IMemberRubric
    {     
        string RubricName { get; set; }
        Type RubricType { get; set; }
        int RubricId { get; set; }
        int RubricSize { get; set; }
        int RubricOffset { get; set; }
     
        bool Visible { get; set; }
        bool Editable { get; set; }

        object[] RubricAttributes { get; set; }
    }
}
