using System.Multemic;

namespace System.Instant
{
    public class Figures : IInstant
    {
        private Type compiledType;
        private Figure figure;
        private bool safeThread;

        public Figures(Figure figureGenerator, string figuresTypeName = null, bool safeThread = true)
        {
            if(figureGenerator.Type == null)
                figureGenerator.Generate();
            this.safeThread = safeThread;
            this.figure = figureGenerator;
            Name = (figuresTypeName != null && figuresTypeName != "") ? figuresTypeName : figure.Name + "_Figures";
        }

        public Figures(Type figureModelType, bool safeThread = true)
           : this(figureModelType, null, null, FigureMode.Reference, safeThread)
        {
        }
        public Figures(Type figureModelType, string figuresTypeName, bool safeThread = true)
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
       
        public Type   Type { get; set; }
        public string Name { get; set; }
        public int Size { get => figure.Size; }
        public IRubrics Rubrics { get => figure.Rubrics; }

        public object New()
        {
            return newFigures();
        }

        public IFigures Generate()
        {
            if (this.Type == null)
            {
                var ifc = new FiguresCompiler(this, safeThread);
                compiledType = ifc.CompileFigureType(Name);
                this.Type = compiledType.New().GetType();
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
            return newfigures;
        }
        private IFigures newFigures()
        {
            IFigures newfigures = newFigures((IFigures)(this.Type.New()));
            newfigures.Rubrics = CloneRubrics();
            newfigures.KeyRubrics = newfigures.Rubrics.KeyRubrics;
            newfigures.Picked = newFigures((IFigures)this.Type.New());
            newfigures.Picked.Rubrics = newfigures.Rubrics;
            newfigures.Picked.KeyRubrics = newfigures.KeyRubrics;
            return newfigures;
        }
      
        private MemberRubrics CloneRubrics()
        {
            var rbrcs = new MemberRubrics();
            rbrcs.KeyRubrics = new MemberRubrics();
            foreach (var rbrc in figure.Rubrics.AsValues())
                rbrcs.Add(new MemberRubric(rbrc));
            foreach (var rbrk in rbrcs.AsValues())
                if(rbrk.IsKey)
                    rbrcs.KeyRubrics.Add(new MemberRubric(rbrk));
            rbrcs.Update();
            return rbrcs;
        }
    }
}