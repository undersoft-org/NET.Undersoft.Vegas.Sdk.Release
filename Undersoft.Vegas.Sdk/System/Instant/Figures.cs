/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Figures.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Figures" />.
    /// </summary>
    public class Figures : IInstant
    {
        #region Fields

        private Type compiledType;
        private Figure figure;
        private ulong key;
        private bool safeThread;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureGenerator">The figureGenerator<see cref="Figure"/>.</param>
        /// <param name="figuresTypeName">The figuresTypeName<see cref="string"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(Figure figureGenerator, string figuresTypeName = null, bool safeThread = true)
        {
            if (figureGenerator.Type == null)
                figureGenerator.Combine();
            this.safeThread = safeThread;
            this.figure = figureGenerator;
            Name = (figuresTypeName != null && figuresTypeName != "") ? figuresTypeName : figure.Name + "_F";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureObject">The figureObject<see cref="IFigure"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(IFigure figureObject, bool safeThread = true)
        : this(new Figure(figureObject.GetType(), figureObject.GetType().Name, FigureMode.Reference), null, safeThread)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureObject">The figureObject<see cref="IFigure"/>.</param>
        /// <param name="figuresTypeName">The figuresTypeName<see cref="string"/>.</param>
        /// <param name="modeType">The modeType<see cref="FigureMode"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(IFigure figureObject, string figuresTypeName, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
           : this(new Figure(figureObject.GetType(), figureObject.GetType().Name, modeType), figuresTypeName, safeThread)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureRubrics">The figureRubrics<see cref="MemberRubrics"/>.</param>
        /// <param name="figuresTypeName">The figuresTypeName<see cref="string"/>.</param>
        /// <param name="figureTypeName">The figureTypeName<see cref="string"/>.</param>
        /// <param name="modeType">The modeType<see cref="FigureMode"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(MemberRubrics figureRubrics, string figuresTypeName = null, string figureTypeName = null, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
         : this(new Figure(figureRubrics, figureTypeName, modeType), figuresTypeName, safeThread)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureModelType">The figureModelType<see cref="Type"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(Type figureModelType, bool safeThread = true)
           : this(figureModelType, null, null, FigureMode.Reference, safeThread)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureModelType">The figureModelType<see cref="Type"/>.</param>
        /// <param name="figuresTypeName">The figuresTypeName<see cref="string"/>.</param>
        /// <param name="IsVirtual">The IsVirtual<see cref="bool"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(Type figureModelType, string figuresTypeName, bool IsVirtual = false, bool safeThread = true)
           : this(figureModelType, figuresTypeName, null, FigureMode.Reference, safeThread)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Figures"/> class.
        /// </summary>
        /// <param name="figureModelType">The figureModelType<see cref="Type"/>.</param>
        /// <param name="figuresTypeName">The figuresTypeName<see cref="string"/>.</param>
        /// <param name="figureTypeName">The figureTypeName<see cref="string"/>.</param>
        /// <param name="modeType">The modeType<see cref="FigureMode"/>.</param>
        /// <param name="safeThread">The safeThread<see cref="bool"/>.</param>
        public Figures(Type figureModelType, string figuresTypeName, string figureTypeName, FigureMode modeType = FigureMode.Reference, bool safeThread = true)
           : this(new Figure(figureModelType, figureTypeName, modeType), figuresTypeName, safeThread)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BaseType.
        /// </summary>
        public Type BaseType { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the Rubrics.
        /// </summary>
        public IRubrics Rubrics { get => figure.Rubrics; }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public int Size { get => figure.Size; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public Type Type { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Combine.
        /// </summary>
        /// <returns>The <see cref="IFigures"/>.</returns>
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

        /// <summary>
        /// The New.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object New()
        {
            return newFigures();
        }

        /// <summary>
        /// The CloneRubrics.
        /// </summary>
        /// <returns>The <see cref="MemberRubrics"/>.</returns>
        private MemberRubrics CloneRubrics()
        {
            var rubrics = new MemberRubrics();
            rubrics.KeyRubrics = new MemberRubrics();
            foreach (var rubric in figure.Rubrics.AsValues())
            {
                var clonedRubric = new MemberRubric(rubric);
                rubrics.Add(clonedRubric);
                if (clonedRubric.IsKey)
                {
                    rubrics.KeyRubrics.Add(clonedRubric);
                }
            }
            rubrics.Update();
            return rubrics;
        }

        /// <summary>
        /// The newFigures.
        /// </summary>
        /// <returns>The <see cref="IFigures"/>.</returns>
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

        /// <summary>
        /// The newFigures.
        /// </summary>
        /// <param name="newfigures">The newfigures<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
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

        #endregion
    }
}
