using System.Instant.Treatment;
using System.Multemic;

namespace System.Instant
{
    public interface ISleeves : IFigures
    {
        new IInstant Instant { get; set; }

        IFigures Sleeves { get; set; }

        IFigures Figures { get; set; }      
    }
}