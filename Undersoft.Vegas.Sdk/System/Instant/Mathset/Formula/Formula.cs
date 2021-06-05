/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.Formula.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System.Reflection.Emit;

    /// <summary>
    /// Defines the <see cref="Formula" />.
    /// </summary>
    [Serializable]
    public abstract class Formula
    {
        #region Fields

        [NonSerialized] public CombinedFormula CombinedFormula;
        [NonSerialized] public Formula LeftFormula;
        [NonSerialized] public Formula RightFormula;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public virtual MathsetSize Size
        {
            get
            {
                return new MathsetSize(0, 0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Cos.
        /// </summary>
        /// <param name="e">The e<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public static Formula Cos(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Cos);
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="e">The e<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public static Formula Log(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Log);
        }

        /// <summary>
        /// The MemPow.
        /// </summary>
        /// <param name="e1">The e1<see cref="Formula"/>.</param>
        /// <param name="e2">The e2<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public static Formula MemPow(Formula e1, Formula e2)
        {
            return new PowerOperation(e1, e2);
        }

        /// <summary>
        /// The Sin.
        /// </summary>
        /// <param name="e">The e<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public static Formula Sin(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Sin);
        }

        /// <summary>
        /// The CombineMathset.
        /// </summary>
        /// <param name="m">The m<see cref="CombinedFormula"/>.</param>
        /// <returns>The <see cref="CombinedMathset"/>.</returns>
        public CombinedMathset CombineMathset(CombinedFormula m)
        {
            return Compiler.Compile(m);
        }

        /// <summary>
        /// The CombineMathset.
        /// </summary>
        /// <param name="f">The f<see cref="Formula"/>.</param>
        /// <param name="m">The m<see cref="LeftFormula"/>.</param>
        /// <returns>The <see cref="CombinedMathset"/>.</returns>
        public CombinedMathset CombineMathset(Formula f, LeftFormula m)
        {
            CombinedMathset mathline = Compiler.Compile(new CombinedFormula(m, f));
            return mathline;
        }

        //public abstract double Math(int i, int j);
        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="cc">The cc<see cref="CompilerContext"/>.</param>
        public abstract void Compile(ILGenerator g, CompilerContext cc);

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="cm">The cm<see cref="CombinedMathset"/>.</param>
        public void Compute(CombinedMathset cm)
        {
            Evaluator e = new Evaluator(cm.Compute);
            e();
        }

        /// <summary>
        /// The CreateEvaluator.
        /// </summary>
        /// <param name="m">The m<see cref="CombinedFormula"/>.</param>
        /// <returns>The <see cref="Evaluator"/>.</returns>
        public Evaluator CreateEvaluator(CombinedFormula m)
        {
            CombinedMathset mathline = CombineMathset(m);
            Evaluator ev = new Evaluator(mathline.Compute);
            return ev;
        }

        /// <summary>
        /// The CreateEvaluator.
        /// </summary>
        /// <param name="e">The e<see cref="CombinedMathset"/>.</param>
        /// <returns>The <see cref="Evaluator"/>.</returns>
        public Evaluator CreateEvaluator(CombinedMathset e)
        {
            Evaluator ev = new Evaluator(e.Compute);
            return ev;
        }

        /// <summary>
        /// The CreateEvaluator.
        /// </summary>
        /// <param name="f">The f<see cref="Formula"/>.</param>
        /// <param name="m">The m<see cref="LeftFormula"/>.</param>
        /// <returns>The <see cref="Evaluator"/>.</returns>
        public Evaluator CreateEvaluator(Formula f, LeftFormula m)
        {
            CombinedMathset mathline = CombineMathset(f, m);
            Evaluator ev = new Evaluator(mathline.Compute);
            return ev;
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="o">The o<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            return this.Equals(o);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        // power e2 is always a literal
        /// <summary>
        /// The Pow.
        /// </summary>
        /// <param name="e2">The e2<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public Formula Pow(Formula e2)
        {
            return new PowerOperation(this, e2);
        }

        /// <summary>
        /// The Prepare.
        /// </summary>
        /// <param name="f">The f<see cref="Formula"/>.</param>
        /// <param name="m">The m<see cref="LeftFormula"/>.</param>
        /// <param name="partial">The partial<see cref="bool"/>.</param>
        /// <returns>The <see cref="CombinedFormula"/>.</returns>
        public CombinedFormula Prepare(Formula f, LeftFormula m, bool partial = false)
        {
            CombinedFormula = new CombinedFormula(m, f, partial);
            CombinedFormula.LeftFormula = m;
            CombinedFormula.RightFormula = f;
            return CombinedFormula;
        }

        /// <summary>
        /// The Prepare.
        /// </summary>
        /// <param name="m">The m<see cref="LeftFormula"/>.</param>
        /// <param name="partial">The partial<see cref="bool"/>.</param>
        /// <returns>The <see cref="CombinedFormula"/>.</returns>
        public CombinedFormula Prepare(LeftFormula m, bool partial = false)
        {
            CombinedFormula = new CombinedFormula(m, this, partial);
            CombinedFormula.LeftFormula = m;
            CombinedFormula.RightFormula = this;
            return CombinedFormula;
        }

        /// <summary>
        /// The Transpose.
        /// </summary>
        /// <returns>The <see cref="Formula"/>.</returns>
        public Formula Transpose()
        {
            return new TransposeOperation(this);
        }

        #endregion


        // addition
        public static Formula operator +(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Plus());
        }

        // subtraction
        public static Formula operator -(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Minus());
        }

        // multiplication
        public static Formula operator *(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Multiply());
        }

        // division
        public static Formula operator /(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Divide());
        }

        // equal
        public static Formula operator ==(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Equal());
        }

        // not equal
        public static Formula operator !=(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new NotEqual());
        }

        // lesser
        public static Formula operator <(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Less());
        }

        // or
        public static Formula operator |(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new OrOperand());
        }

        // greater
        public static Formula operator >(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Greater());
        }

        // and
        public static Formula operator &(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new AndOperand());
        }

        // not equal literal
        public static bool operator !=(Formula e1, object o)
        {
            if (o == null)
                return NullCheck.IsNotNull(e1);
            else
                return !e1.Equals(o);
        }

        // equal literal
        public static bool operator ==(Formula e1, object o)
        {
            if (o == null)
                return NullCheck.IsNull(e1);
            else
                return e1.Equals(o);
        }

        public static implicit operator Formula(double f)
        {
            return new UnsignedFormula(f);
        }
    }

    /// <summary>
    /// Defines the <see cref="NullCheck" />.
    /// </summary>
    public static class NullCheck
    {
        #region Methods

        /// <summary>
        /// The IsNotNull.
        /// </summary>
        /// <param name="o">The o<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNotNull(object o)
        {
            if (o is ValueType)
                return false;
            else
                return !ReferenceEquals(o, null);
        }

        /// <summary>
        /// The IsNull.
        /// </summary>
        /// <param name="o">The o<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNull(object o)
        {
            if (o is ValueType)
                return false;
            else
                return ReferenceEquals(o, null);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SizeMismatchException" />.
    /// </summary>
    public class SizeMismatchException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeMismatchException"/> class.
        /// </summary>
        /// <param name="s">The s<see cref="string"/>.</param>
        public SizeMismatchException(string s) : base(s)
        {
        }

        #endregion
    }
}
