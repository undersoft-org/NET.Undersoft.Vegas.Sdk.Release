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

    /// <summary>
    /// Defines the <see cref="Sleeves" />.
    /// </summary>
    public class Sleeves : IInstant
    {
        #region Fields

        private Type compiledType;
        private ulong key;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sleeves"/> class.
        /// </summary>
        /// <param name="figuresObject">The figuresObject<see cref="IFigures"/>.</param>
        public Sleeves(IFigures figuresObject) : this(figuresObject, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Sleeves"/> class.
        /// </summary>
        /// <param name="figuresObject">The figuresObject<see cref="IFigures"/>.</param>
        /// <param name="sleeveTypeName">The sleeveTypeName<see cref="string"/>.</param>
        public Sleeves(IFigures figuresObject, string sleeveTypeName)
        {
            Name = (sleeveTypeName != null && sleeveTypeName != "") ? sleeveTypeName : figuresObject.Type.Name + "_S";
            figures = figuresObject;
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
        public IRubrics Rubrics { get => figures.Rubrics; }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public int Size { get => figures.FigureSize; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        private IFigures figures { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Combine.
        /// </summary>
        /// <returns>The <see cref="ISleeves"/>.</returns>
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

        /// <summary>
        /// The New.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object New()
        {
            return newSleeves();
        }

        /// <summary>
        /// The newSleeves.
        /// </summary>
        /// <returns>The <see cref="ISleeves"/>.</returns>
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
