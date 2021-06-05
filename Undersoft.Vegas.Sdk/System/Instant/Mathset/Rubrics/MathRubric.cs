using System;
using System.Extract;
using System.Linq;
using System.Uniques;
using System.Collections;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection.Emit;

namespace System.Instant.Mathset
{
    public class MathRubric : IUnique
    {
        #region Private NonSerialized
        [NonSerialized] private MemberRubric memberRubric;
        [NonSerialized] private CombinedMathset reckoner;
        [NonSerialized] private CombinedFormula formula;
        [NonSerialized] private Mathset formuler;
        [NonSerialized] private MathRubrics formulaRubrics;
        [NonSerialized] private MathRubrics mathlineRubrics;
        [NonSerialized] private SubMathset subFormuler;

        #endregion

        public MathRubric(MathRubrics rubrics, MemberRubric rubric)
        {
            memberRubric = rubric;
            mathlineRubrics = rubrics;
            SerialCode = rubric.SerialCode;
        }

        #region Mathset Formula

        public int FigureFieldId { get => memberRubric.FieldId; }
        public int ComputeOrdinal { get; set; }
        public Type RubricType { get => memberRubric.RubricType; }
        public string RubricName { get => memberRubric.RubricName; }

        public Mathset CloneMathset()
        {
            return formuler.Clone();
        }
        public Mathset NewMathset()
        {
            return Formuler = new Mathset(this);
        }
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

        public Mathset Formuler
        { get { return formuler; } set { formuler = value; } }

        public MathRubrics MathsetRubrics
        { get { return mathlineRubrics; } set { mathlineRubrics = value; } }
        public MathRubrics FormulaRubrics
        { get { return formulaRubrics; } set { formulaRubrics = value; } }
        public MathRubric  FormulaRubric
        { get { return this; } }

        public SubMathset  SubFormuler
        { get; set; }
        public Formula      RightFormula
        { get; set; }

        public bool PartialMathset { get; set; }

        public CombinedMathset Evaluator
        { get { return reckoner; } set { reckoner = value; } }

        public LeftFormula Compute()
        {
            if (reckoner != null)
            {
                Evaluator reckon = new Evaluator(reckoner.Compute);
                reckon();
            }
            return formula.lexpr;
        }

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

        public CombinedMathset CombineEvaluator()
        {
            if (reckoner == null)
                reckoner = formula.CombineMathset(formula);

            return reckoner;
        }

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
       
        #endregion

        public IUnique Empty => Ussn.Empty;

        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }
        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        public byte[] GetBytes()
        {
            return SerialCode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        //public void SetUniqueKey(long value)
        //{
        //    systemSerialCode.UniqueKey = value;
        //}

        //public long GetUniqueKey()
        //{
        //    return systemSerialCode.UniqueKey;
        //}

        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }

        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }

        //public void SetUniqueSeed(uint seed)
        //{
        //    systemSerialCode.SetUniqueSeed(seed);
        //}

        //public uint GetUniqueSeed()
        //{
        //    return systemSerialCode.GetUniqueSeed();
        //}

        public Ussn SerialCode { get; set; }

    }

}
