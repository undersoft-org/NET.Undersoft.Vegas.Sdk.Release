/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.UnsignedFormula.cs
   
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
    /// Defines the <see cref="UnsignedFormula" />.
    /// </summary>
    [Serializable]
    public class UnsignedFormula : Formula
    {
        #region Fields

        internal double thevalue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedFormula"/> class.
        /// </summary>
        /// <param name="vv">The vv<see cref="double"/>.</param>
        public UnsignedFormula(double vv)
        {
            thevalue = vv;
        }

        #endregion

        #region Properties

        //public override double Math(int i, int j)
        //{
        //    return thevalue;
        //}
        /// <summary>
        /// Gets the Size.
        /// </summary>
        public override MathsetSize Size
        {
            get { return MathsetSize.Scalar; }
        }

        #endregion

        #region Methods

        // First Pass: none
        // Push a float literal (partial evaluation)
        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="cc">The cc<see cref="CompilerContext"/>.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass()) return;
            g.Emit(OpCodes.Ldc_R8, thevalue);
        }

        #endregion
    }
}
