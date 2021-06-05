/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sleeves.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (02.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Uniques;

    public class Sleeves : IInstant
    {
        #region Fields

        private Type compiledType;
        private ulong key;

        #endregion

        #region Constructors

        public Sleeves(IFigures figuresObject) : this(figuresObject, null)
        {
        }
        public Sleeves(IFigures figuresObject, string sleeveTypeName)
        {
            Name = (sleeveTypeName != null && sleeveTypeName != "") ? sleeveTypeName : figuresObject.Type.Name + "_S";
            figures = figuresObject;
        }

        #endregion

        #region Properties

        public Type BaseType { get; set; }

        public string Name { get; set; }

        public IRubrics Rubrics { get => figures.Rubrics; }

        public int Size { get => figures.FigureSize; }

        public Type Type { get; set; }

        private IFigures figures { get; set; }

        #endregion

        #region Methods

        public ISleeves Combine()
        {
            if (figures != null)
            {
                if (this.Type == null)
                {
                    var rsb = new SleevesCompiler(this);
                    compiledType = rsb.CompileFigureType(Name);
                    this.Type = compiledType.New().GetType();
                    key = Name.UniqueKey64();
                }
                return newSleeves();
            }
            return null;
        }

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
            o.UniqueKey = key;
            o.UniqueSeed = Unique.NewKey;
            return o;
        }

        #endregion
    }
}
