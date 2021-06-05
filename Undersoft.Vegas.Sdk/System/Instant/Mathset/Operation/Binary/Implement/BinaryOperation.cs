/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.BinaryOperation.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;
    using System.Reflection.Emit;

    // Binary Member By Member Oprator Formula
    /// <summary>
    /// Defines the <see cref="BinaryOperation" />.
    /// </summary>
    [Serializable]
    public class BinaryOperation : BinaryFormula
    {
        #region Fields

        internal BinaryOperator oper;

        #endregion

        #region Constructors

        // MathsetSize size;
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperation"/> class.
        /// </summary>
        /// <param name="e1">The e1<see cref="Formula"/>.</param>
        /// <param name="e2">The e2<see cref="Formula"/>.</param>
        /// <param name="op">The op<see cref="BinaryOperator"/>.</param>
        public BinaryOperation(Formula e1, Formula e2, BinaryOperator op) : base(e1, e2)
        {
            oper = op;
        }

        #endregion

        #region Properties

        //public override double Math(int i, int j)
        //{
        //    return oper.Apply(expr1.Math(i, j), expr2.Math(i, j));
        //}
        /// <summary>
        /// Gets the Size.
        /// </summary>
        public override MathsetSize Size
        {
            get { return expr1.Size == MathsetSize.Scalar ? expr2.Size : expr1.Size; }
        }

        #endregion

        #region Methods

        // Compilation First Pass: check sizes
        // Code Generation: the operator code
        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="cc">The cc<see cref="CompilerContext"/>.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            expr1.Compile(g, cc);
            expr2.Compile(g, cc);
            if (cc.IsFirstPass())
                return;
            oper.Compile(g);
        }

        #endregion
    }
}
