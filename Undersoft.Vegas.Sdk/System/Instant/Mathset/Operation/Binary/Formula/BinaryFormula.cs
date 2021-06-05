/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.BinaryFormula.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;

    /// <summary>
    /// Defines the <see cref="BinaryFormula" />.
    /// </summary>
    [Serializable]
    public abstract class BinaryFormula : Formula
    {
        #region Fields

        protected Formula expr1, expr2;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFormula"/> class.
        /// </summary>
        /// <param name="e1">The e1<see cref="Formula"/>.</param>
        /// <param name="e2">The e2<see cref="Formula"/>.</param>
        public BinaryFormula(Formula e1, Formula e2)
        {
            expr1 = e1;
            expr2 = e2;
        }

        #endregion
    }
}
