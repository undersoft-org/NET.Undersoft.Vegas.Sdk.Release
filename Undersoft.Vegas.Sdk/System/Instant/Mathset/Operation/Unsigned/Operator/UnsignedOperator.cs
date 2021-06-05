/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.UnsignedOperator.cs
   
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
    /// Defines the <see cref="UnsignedOperator" />.
    /// </summary>
    [Serializable]
    public class UnsignedOperator : Formula
    {
        #region Fields

        protected Formula e;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedOperator"/> class.
        /// </summary>
        /// <param name="ee">The ee<see cref="Formula"/>.</param>
        public UnsignedOperator(Formula ee)
        {
            e = ee;
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
            get { return e.Size; }
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
            e.Compile(g, cc);
        }

        #endregion
    }
}
