/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.LeftFormula.cs
   
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
    /// Defines the <see cref="LeftFormula" />.
    /// </summary>
    [Serializable]
    public abstract class LeftFormula : Formula
    {
        #region Methods

        //public abstract void Assign(int i, double v);
        //public abstract void Assign(int i, bool v);
        //public abstract void Assign(int i, int j, double v);
        //public abstract void Assign(int i, int j, bool v);
        /// <summary>
        /// The CompileAssign.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="cc">The cc<see cref="CompilerContext"/>.</param>
        /// <param name="post">The post<see cref="bool"/>.</param>
        /// <param name="partial">The partial<see cref="bool"/>.</param>
        public abstract void CompileAssign(ILGenerator g, CompilerContext cc, bool post, bool partial);

        #endregion
    }
}
