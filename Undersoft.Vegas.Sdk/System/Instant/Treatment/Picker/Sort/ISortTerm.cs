using System.Linq;

namespace System.Instant.Treatment
{
    public interface ISortTerm
    {
        int RubricId { get; set; }

        string RubricName { get; set; }

        SortDirection Direction { get; set; }     
    }
}