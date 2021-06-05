using System.Instant.Treatments;
using System.Sets;

namespace System.Instant
{
    public interface ISleeves : IFigures
    {
        new IInstant Instant { get; set; }

        IFigures Sleeves { get; set; }

        IFigures Figures { get; set; }      
    }
}