/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.TransposeOperation.cs
   
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
    /// Defines the <see cref="TransposeOperation" />.
    /// </summary>
    [Serializable]
    public class TransposeOperation : UnsignedOperator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransposeOperation"/> class.
        /// </summary>
        /// <param name="e">The e<see cref="Formula"/>.</param>
        public TransposeOperation(Formula e) : base(e)
        {
        }

        #endregion

        #region Properties

        //public override double Math(int i, int j)
        //{
        //    return e.Math(j, i);
        //}
        /// <summary>
        /// Gets the Size.
        /// </summary>
        public override MathsetSize Size
        {
            get
            {
                MathsetSize o = e.Size;
                return new MathsetSize(o.cols, o.rows);
            }
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
                e.Compile(g, cc);
                return;
            }

            // swap the indexes at the compiler level
            int i1 = cc.GetIndexVariable(0);
            int i2 = cc.GetIndexVariable(1);
            cc.SetIndexVariable(1, i1);
            cc.SetIndexVariable(0, i2);
            e.Compile(g, cc);
            cc.SetIndexVariable(0, i1);
            cc.SetIndexVariable(1, i2);
        }

        #endregion
    }
}
