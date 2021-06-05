/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathRubric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="MathRubric" />.
    /// </summary>
    public class MathRubric : IUnique
    {
        #region Fields

        [NonSerialized] private CombinedFormula formula;
        [NonSerialized] private MathRubrics formulaRubrics;
        [NonSerialized] private Mathset formuler;
        [NonSerialized] private MathRubrics mathlineRubrics;
        [NonSerialized] private MemberRubric memberRubric;
        [NonSerialized] private CombinedMathset reckoner;
        [NonSerialized] private SubMathset subFormuler;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MathRubric"/> class.
        /// </summary>
        /// <param name="rubrics">The rubrics<see cref="MathRubrics"/>.</param>
        /// <param name="rubric">The rubric<see cref="MemberRubric"/>.</param>
        public MathRubric(MathRubrics rubrics, MemberRubric rubric)
        {
            memberRubric = rubric;
            mathlineRubrics = rubrics;
            SerialCode = rubric.SerialCode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ComputeOrdinal.
        /// </summary>
        public int ComputeOrdinal { get; set; }

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the Evaluator.
        /// </summary>
        public CombinedMathset Evaluator
        {
            get { return reckoner; }
            set { reckoner = value; }
        }

        /// <summary>
        /// Gets the FigureFieldId.
        /// </summary>
        public int FigureFieldId { get => memberRubric.FieldId; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public Formula Formula
        {
            get
            {
                return (!ReferenceEquals(formula, null)) ? formula : null;
            }
            set
            {
                if (!ReferenceEquals(value, null))
                {
                    formula = value.Prepare(Formuler[this.memberRubric.RubricName]);
                }
            }
        }

        /// <summary>
        /// Gets the FormulaRubric.
        /// </summary>
        public MathRubric FormulaRubric
        {
            get { return this; }
        }

        /// <summary>
        /// Gets or sets the FormulaRubrics.
        /// </summary>
        public MathRubrics FormulaRubrics
        {
            get { return formulaRubrics; }
            set { formulaRubrics = value; }
        }

        /// <summary>
        /// Gets or sets the Formuler.
        /// </summary>
        public Mathset Formuler
        {
            get { return formuler; }
            set { formuler = value; }
        }

        /// <summary>
        /// Gets or sets the MathsetRubrics.
        /// </summary>
        public MathRubrics MathsetRubrics
        {
            get { return mathlineRubrics; }
            set { mathlineRubrics = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PartialMathset.
        /// </summary>
        public bool PartialMathset { get; set; }

        /// <summary>
        /// Gets or sets the RightFormula.
        /// </summary>
        public Formula RightFormula { get; set; }

        /// <summary>
        /// Gets the RubricName.
        /// </summary>
        public string RubricName { get => memberRubric.RubricName; }

        /// <summary>
        /// Gets the RubricType.
        /// </summary>
        public Type RubricType { get => memberRubric.RubricType; }

        //public void SetUniqueSeed(uint seed)
        //{
        //    systemSerialCode.SetUniqueSeed(seed);
        //}

        //public uint GetUniqueSeed()
        //{
        //    return systemSerialCode.GetUniqueSeed();
        //}
        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussn SerialCode { get; set; }

        /// <summary>
        /// Gets or sets the SubFormuler.
        /// </summary>
        public SubMathset SubFormuler { get; set; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        #endregion

        #region Methods

        /// <summary>
        /// The AssignRubric.
        /// </summary>
        /// <param name="ordinal">The ordinal<see cref="int"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        public MathRubric AssignRubric(int ordinal)
        {
            if (FormulaRubrics == null)
                FormulaRubrics = new MathRubrics(mathlineRubrics);

            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[ordinal];
            if (rubric != null)
            {
                erubric = new MathRubric(MathsetRubrics, rubric);
                assignRubric(erubric);
            }
            return erubric;
        }

        /// <summary>
        /// The AssignRubric.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        public MathRubric AssignRubric(string name)
        {
            if (FormulaRubrics == null)
                FormulaRubrics = new MathRubrics(mathlineRubrics);

            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[name];
            if (rubric != null)
            {
                erubric = new MathRubric(MathsetRubrics, rubric);
                assignRubric(erubric);
            }
            return erubric;
        }

        /// <summary>
        /// The CloneMathset.
        /// </summary>
        /// <returns>The <see cref="Mathset"/>.</returns>
        public Mathset CloneMathset()
        {
            return formuler.Clone();
        }

        /// <summary>
        /// The CombineEvaluator.
        /// </summary>
        /// <returns>The <see cref="CombinedMathset"/>.</returns>
        public CombinedMathset CombineEvaluator()
        {
            if (reckoner == null)
                reckoner = formula.CombineMathset(formula);

            return reckoner;
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <returns>The <see cref="LeftFormula"/>.</returns>
        public LeftFormula Compute()
        {
            if (reckoner != null)
            {
                Evaluator reckon = new Evaluator(reckoner.Compute);
                reckon();
            }
            return formula.lexpr;
        }

        //public void SetUniqueKey(long value)
        //{
        //    systemSerialCode.UniqueKey = value;
        //}

        //public long GetUniqueKey()
        //{
        //    return systemSerialCode.UniqueKey;
        //}
        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return SerialCode.GetBytes();
        }

        /// <summary>
        /// The GetMathset.
        /// </summary>
        /// <returns>The <see cref="Mathset"/>.</returns>
        public Mathset GetMathset()
        {
            if (!ReferenceEquals(Formuler, null))
                return Formuler;
            else
            {
                formulaRubrics = new MathRubrics(mathlineRubrics);
                return Formuler = new Mathset(this);
            }
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        /// <summary>
        /// The NewMathset.
        /// </summary>
        /// <returns>The <see cref="Mathset"/>.</returns>
        public Mathset NewMathset()
        {
            return Formuler = new Mathset(this);
        }

        /// <summary>
        /// The RemoveRubric.
        /// </summary>
        /// <param name="ordinal">The ordinal<see cref="int"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        public MathRubric RemoveRubric(int ordinal)
        {
            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[ordinal];
            if (rubric != null)
            {
                erubric = MathsetRubrics[rubric];
                removeRubric(erubric);
            }
            return erubric;
        }

        /// <summary>
        /// The RemoveRubric.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        public MathRubric RemoveRubric(string name)
        {
            MathRubric erubric = null;
            MemberRubric rubric = MathsetRubrics.Rubrics[name];
            if (rubric != null)
            {
                erubric = MathsetRubrics[rubric];
                removeRubric(erubric);
            }
            return erubric;
        }

        /// <summary>
        /// The assignRubric.
        /// </summary>
        /// <param name="erubric">The erubric<see cref="MathRubric"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        private MathRubric assignRubric(MathRubric erubric)
        {
            if (!FormulaRubrics.Contains(erubric))
            {
                if (!MathsetRubrics.MathsetRubrics.Contains(erubric))
                {
                    MathsetRubrics.MathsetRubrics.Add(erubric);
                }

                if (erubric.FigureFieldId == FormulaRubric.FigureFieldId &&
                    !MathsetRubrics.FormulaRubrics.Contains(erubric))
                    MathsetRubrics.FormulaRubrics.Add(erubric);

                FormulaRubrics.Add(erubric);
            }
            return erubric;
        }

        /// <summary>
        /// The removeRubric.
        /// </summary>
        /// <param name="erubric">The erubric<see cref="MathRubric"/>.</param>
        /// <returns>The <see cref="MathRubric"/>.</returns>
        private MathRubric removeRubric(MathRubric erubric)
        {
            if (!FormulaRubrics.Contains(erubric))
            {
                FormulaRubrics.Remove(erubric);

                if (!MathsetRubrics.MathsetRubrics.Contains(erubric))
                    MathsetRubrics.MathsetRubrics.Remove(erubric);

                if (!ReferenceEquals(Formuler, null) &&
                    !MathsetRubrics.FormulaRubrics.Contains(erubric))
                {
                    MathsetRubrics.Rubrics.Remove(erubric);
                    Formuler = null;
                    Formula = null;
                }
            }
            return erubric;
        }

        #endregion
    }
}
