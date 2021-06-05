using System.Uniques;

namespace System.Instant
{
    public class Figures : IInstant
    {
        private Type compiledType;
        private Figure figure;
        private bool safeThread;
        private ulong key;

        public Figures(Figure figureGenerator, string figuresTypeName = null, bool safeThread = true)
        {
            if(figureGenerator.Type == null)
                figureGenerator.Combine();
            this.safeThread = safeThread;
            this.figure = figureGenerator;
            Name = (figuresTypeName != null && figuresTypeName != "") ? figuresTypeName : figure.Name + "_F";
        }

        public Figures(Type figureModelType, bool safeThread = true)
           : this(figureModelType, null, null, FigureMode.Reference, safeThread)
        {
        }
        public Figures(Type figureModelType, string figuresTypeName, bool IsVirtual = false, bool safeThread = true)
           : this(figureModelType, figuresTypeName, null, FigureMode.Reference, safeThread)
        {
        }
        public Figures(Type figureModelType, string figuresTypeName, string figureTypeName, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
           : this(new Figure(figureModelType, figureTypeName, modeType), figuresTypeName, safeThread)
        {
        }

        public Figures(IFigure figureObject, bool safeThread = true)
        : this(new Figure(figureObject.GetType(), figureObject.GetType().Name, FigureMode.Reference), null, safeThread)
        {
        }
        public Figures(IFigure figureObject, string figuresTypeName, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
           : this(new Figure(figureObject.GetType(), figureObject.GetType().Name, modeType), figuresTypeName, safeThread)
        {
        }

        public Figures(MemberRubrics figureRubrics, string figuresTypeName = null, string figureTypeName = null, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
         : this(new Figure(figureRubrics, figureTypeName, modeType), figuresTypeName, safeThread)
        {
        }

        public Type   BaseType { get; set; }
        public Type   Type { get; set; }
        public string Name { get; set; }
        public int Size { get => figure.Size; }
        public IRubrics Rubrics { get => figure.Rubrics; }

        public object New()
        {
            return newFigures();
        }

        public IFigures Combine()
        {
            if (this.Type == null)
            {
                var ifc = new FiguresCompiler(this, safeThread);
                compiledType = ifc.CompileFigureType(Name);
                this.Type = compiledType.New().GetType();
                key = Name.UniqueKey64();
            }
            return newFigures();
        }

        private IFigures newFigures(IFigures newfigures)
        {
            newfigures.FigureType = figure.Type;
            newfigures.FigureSize = figure.Size;
            newfigures.Type = this.Type;                 
            newfigures.Instant = this;
            newfigures.Prime = true;
            newfigures.UniqueKey = key;
            newfigures.UniqueSeed = Unique.NewKey;
            newfigures.Linker.Figures = newfigures;
            return newfigures;
        }
        private IFigures newFigures()
        {
            IFigures newfigures = newFigures((IFigures)(this.Type.New()));
            newfigures.Rubrics = CloneRubrics();
            newfigures.KeyRubrics = newfigures.Rubrics.KeyRubrics;
            newfigures.View = newFigures((IFigures)this.Type.New());
            newfigures.View.Rubrics = newfigures.Rubrics;
            newfigures.View.KeyRubrics = newfigures.KeyRubrics;
            return newfigures;
        }
      
        private MemberRubrics CloneRubrics()
        {
            var rubrics = new MemberRubrics();
            rubrics.KeyRubrics = new MemberRubrics();
            foreach (var rubric in figure.Rubrics.AsValues())
            {
                var clonedRubric = new MemberRubric(rubric);
                rubrics.Add(clonedRubric);
                if(clonedRubric.IsKey)
                {
                    rubrics.KeyRubrics.Add(clonedRubric);
                }
            }
            rubrics.Update();
            return rubrics;
        }
    }
}