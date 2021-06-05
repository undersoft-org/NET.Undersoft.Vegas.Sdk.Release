/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.PowerOperation.cs
   
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

    /// <summary>
    /// Defines the <see cref="PowerOperation" />.
    /// </summary>
    [Serializable]
    public class PowerOperation : BinaryFormula
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerOperation"/> class.
        /// </summary>
        /// <param name="e1">The e1<see cref="Formula"/>.</param>
        /// <param name="e2">The e2<see cref="Formula"/>.</param>
        public PowerOperation(Formula e1, Formula e2) : base(e1, e2)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public override MathsetSize Size
        {
            get { return expr1.Size; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="cc">The cc<see cref="CompilerContext"/>.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass())
            {
                expr1.Compile(g, cc);
                expr2.Compile(g, cc);
                if (!(expr2.Size == MathsetSize.Scalar))
                    throw new SizeMismatchException("Pow Operator requires a scalar second operand");
                return;
            }
            expr1.Compile(g, cc);
            expr2.Compile(g, cc);
            g.EmitCall(OpCodes.Call, typeof(Math).GetMethod("Pow"), null);
        }

        #endregion
    }
}
