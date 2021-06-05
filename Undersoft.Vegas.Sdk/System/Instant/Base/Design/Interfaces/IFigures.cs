using System.Sets;
using System.Instant.Treatments;
using System.Instant.Linking;

namespace System.Instant
{
    public interface IFigures : IDeck<IFigure>, IFigure, ISerialFormatter
    {
        IInstant Instant { get; set; }

        bool Prime { get; set; } 

        new IFigure this[int index] { get; set; }

        object this[int index, string propertyName] { get; set; }

        object this[int index, int fieldId] { get; set; }        

        IRubrics Rubrics { get; set; }

        IRubrics KeyRubrics { get; set; }

        IFigure NewFigure();

        Type FigureType { get; set; }

        int FigureSize { get; set; }

        Type Type { get; set; }

        IFigures View { get; set; }

        IFigure Summary { get; set; }

        FigureFilter Filter { get; set; }

        FigureSort Sort { get; set; }

        Func<IFigure, bool> QueryFormula { get; set; }

        Treatment Treatment { get; set; }

        Linker Linker { get; set; }

        IDeck<IComputation> Computations { get; set; }
    }
}