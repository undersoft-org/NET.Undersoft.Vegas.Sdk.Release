/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathRubrics.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System.Linq;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="MathRubrics" />.
    /// </summary>
    public class MathRubrics : BaseAlbum<MathRubric>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="IFigures"/>.</param>
        public MathRubrics(IFigures data)
        {
            Rubrics = data.Rubrics;
            FormulaRubrics = new MathRubrics(Rubrics);
            MathsetRubrics = new MathRubrics(Rubrics);
            Data = data;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics"/> class.
        /// </summary>
        /// <param name="rubrics">The rubrics<see cref="IRubrics"/>.</param>
        public MathRubrics(IRubrics rubrics)
        {
            Rubrics = rubrics;
            Data = rubrics.Figures;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubrics"/> class.
        /// </summary>
        /// <param name="rubrics">The rubrics<see cref="MathRubrics"/>.</param>
        public MathRubrics(MathRubrics rubrics)
        {
            Rubrics = rubrics.Rubrics;
            Data = rubrics.Data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        public IFigures Data { get; set; }

        /// <summary>
        /// Gets or sets the FormulaRubrics.
        /// </summary>
        public MathRubrics FormulaRubrics { get; set; }

        /// <summary>
        /// Gets or sets the MathsetRubrics.
        /// </summary>
        public MathRubrics MathsetRubrics { get; set; }

        /// <summary>
        /// Gets the RowsCount.
        /// </summary>
        public int RowsCount
        {
            get
            {
                return Data.Count;
            }
        }

        /// <summary>
        /// Gets or sets the Rubrics.
        /// </summary>
        public IRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets the RubricsCount.
        /// </summary>
        public int RubricsCount
        {
            get
            {
                return Rubrics.Count;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Combine.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Combine()
        {
            if (!ReferenceEquals(Data, null))
            {
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;
        }

        /// <summary>
        /// The Combine.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Combine(IFigures table)
        {
            if (!ReferenceEquals(Data, table))
            {
                Data = table;
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;
        }

        /// <summary>
        /// The CombineEvaluators.
        /// </summary>
        /// <returns>The <see cref="CombinedMathset[]"/>.</returns>
        public CombinedMathset[] CombineEvaluators()
        {
            return this.AsValues().Select(m => m.CombineEvaluator()).ToArray();
        }

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}[]"/>.</returns>
        public override ICard<MathRubric>[] EmptyBaseDeck(int size)
        {
            return new MathRubricCard[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{MathRubric}"/>.</returns>
        public override ICard<MathRubric> EmptyCard()
        {
            return new MathRubricCard();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}[]"/>.</returns>
        public override ICard<MathRubric>[] EmptyCardTable(int size)
        {
            return new MathRubricCard[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{MathRubric}"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}"/>.</returns>
        public override ICard<MathRubric> NewCard(ICard<MathRubric> value)
        {
            return new MathRubricCard(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}"/>.</returns>
        public override ICard<MathRubric> NewCard(MathRubric value)
        {
            return new MathRubricCard(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}"/>.</returns>
        public override ICard<MathRubric> NewCard(object key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="MathRubric"/>.</param>
        /// <returns>The <see cref="ICard{MathRubric}"/>.</returns>
        public override ICard<MathRubric> NewCard(ulong key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }

        #endregion
    }
}
