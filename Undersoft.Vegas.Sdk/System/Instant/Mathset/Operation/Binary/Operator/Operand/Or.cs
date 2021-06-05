/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.Or.cs
   
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
    /// Defines the <see cref="OrOperand" />.
    /// </summary>
    [Serializable]
    public class OrOperand : BinaryOperator
    {
        #region Methods

        /// <summary>
        /// The Apply.
        /// </summary>
        /// <param name="a">The a<see cref="double"/>.</param>
        /// <param name="b">The b<see cref="double"/>.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public override double Apply(double a, double b)
        {
            return Convert.ToDouble(Convert.ToBoolean(a) || Convert.ToBoolean(b));
        }

        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        public override void Compile(ILGenerator g)
        {
            g.Emit(OpCodes.Or);
            g.Emit(OpCodes.Conv_R8);
        }

        #endregion
    }
}
