/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.Minus.cs
   
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
    /// Defines the <see cref="Minus" />.
    /// </summary>
    [Serializable]
    public class Minus : BinaryOperator
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
            return a - b;
        }

        /// <summary>
        /// The Compile.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        public override void Compile(ILGenerator g)
        {
            g.Emit(OpCodes.Sub);
        }

        #endregion
    }
}
