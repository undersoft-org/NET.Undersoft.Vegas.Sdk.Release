/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.CombinedFormula.cs
   
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

    // assign and generate reckond loops
    /// <summary>
    /// Defines the <see cref="CombinedFormula" />.
    /// </summary>
    [Serializable]
    public class CombinedFormula : Formula
    {
        #region Fields

        public Formula expr;
        public LeftFormula lexpr;
        public bool partial = false;
        internal int iI, lL;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedFormula"/> class.
        /// </summary>
        /// <param name="m">The m<see cref="LeftFormula"/>.</param>
        /// <param name="e">The e<see cref="Formula"/>.</param>
        /// <param name="partial">The partial<see cref="bool"/>.</param>
        public CombinedFormula(LeftFormula m, Formula e, bool partial = false)
        {
            lexpr = m;
            expr = e;
            this.partial = partial;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size.
        /// </summary>
        internal MathsetSize size
        {
            get { return expr.Size; }
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
            bool biloop = size.rows > 1 && size.cols > 1;

            if (cc.IsFirstPass())
            {
                if (!partial)
                {
                    iI = cc.AllocIndexVariable();   // i
                    lL = cc.AllocIndexVariable();   // l
                }
                expr.Compile(g, cc);
                lexpr.CompileAssign(g, cc, true, partial);
            }
            else
            {
                if (!partial)
                {
                    int i, l, svi;
                    Label topLabel;
                    Label topLabelE;

                    topLabel = g.DefineLabel();
                    topLabelE = g.DefineLabel();

                    i = cc.GetIndexVariable(iI);
                    l = cc.GetIndexVariable(lL);
                    // then
                    svi = cc.GetIndexVariable(0);

                    cc.SetIndexVariable(0, i);
                    cc.SetIndexVariable(1, size.rows);

                    g.Emit(OpCodes.Ldc_I4_0);   // i = 0
                    g.Emit(OpCodes.Stloc, i);
                    g.Emit(OpCodes.Ldarg_0);
                    g.Emit(OpCodes.Ldc_I4_0);
                    g.EmitCall(OpCodes.Callvirt, typeof(CombinedMathset).GetMethod("GetRowCount", new Type[] { typeof(int) }), null);
                    g.Emit(OpCodes.Stloc, l);

                    if (size.rows > 1 || size.cols > 1)
                    {
                        // iterate rows, so move
                        int index;
                        int count;

                        index = i;
                        count = size.rows;

                        // just one loop. Set wich 
                        g.MarkLabel(topLabel);          // TP:

                        lexpr.CompileAssign(g, cc, false, false);
                        expr.Compile(g, cc);                        // value
                        lexpr.CompileAssign(g, cc, true, false);

                        // increment j
                        g.Emit(OpCodes.Ldloc, index);           // 1
                        g.Emit(OpCodes.Ldc_I4_1);               // j =>	
                        g.Emit(OpCodes.Add);                    // +
                        g.Emit(OpCodes.Dup);
                        g.Emit(OpCodes.Stloc, index);           // j <=									

                        // here from first jump
                        //g.Emit(OpCodes.Ldc_I4, count);  // j < cols

                        g.Emit(OpCodes.Ldloc, l);
                        g.Emit(OpCodes.Blt, topLabel);
                    }
                    else
                    {
                        lexpr.CompileAssign(g, cc, false, false);
                        expr.Compile(g, cc);                        // value
                        lexpr.CompileAssign(g, cc, true, false);
                    }
                    cc.SetIndexVariable(0, svi);
                }
                else
                {
                    lexpr.CompileAssign(g, cc, false, true);
                    expr.Compile(g, cc);                        // value
                    lexpr.CompileAssign(g, cc, true, true);
                }
            }
        }

        #endregion
    }
}
