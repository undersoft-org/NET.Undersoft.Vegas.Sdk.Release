
namespace System.Instant
{
    public class Sleeves : IInstant
    {
        private Type compiledType;
        private IFigures figures { get; set; }

        public Sleeves(IFigures figuresObject) : this(figuresObject, null) { }
        public Sleeves(IFigures figuresObject, string sleeveTypeName)
        {
            Name = (sleeveTypeName != null && sleeveTypeName != "") ? sleeveTypeName : figuresObject.Type.Name + "_Sleeve";
            figures = figuresObject;        
        }

        public ISleeves Generate()
        {
            if (figures != null)
            {
                if (this.Type == null)
                {
                    var rsb = new SleevesCompiler(this);
                    compiledType = rsb.CompileFigureType(Name);

                    this.Type = compiledType.New().GetType();                   
                }
                return newSleeves();
            }
            return null;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public int Size { get => figures.FigureSize; }
        public IRubrics Rubrics { get => figures.Rubrics; }

        public object New()
        {
            return newSleeves();
        }

        private ISleeves newSleeves()
        {
            ISleeves o = (ISleeves)(Type.New());
            o.Figures = figures;
            o.Sleeves = (IFigures)(figures.Instant.New());
            o.Sleeves.Prime = false;
            o.Instant = figures.Instant;
            return o;
        }


    }
}