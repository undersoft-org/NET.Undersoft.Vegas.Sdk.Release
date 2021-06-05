/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.FunctionOperation.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Defines the <see cref="FunctionOperation" />.
    /// </summary>
    [Serializable]
    public class FunctionOperation : UnsignedOperator
    {
        #region Fields

        internal FunctionType effx;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOperation"/> class.
        /// </summary>
        /// <param name="ee">The ee<see cref="Formula"/>.</param>
        /// <param name="fx">The fx<see cref="FunctionType"/>.</param>
        public FunctionOperation(Formula ee, FunctionType fx) : base(ee)
        {
            effx = fx;
        }

        #endregion

        #region Enums

        public enum FunctionType { Cos, Sin, Ln, Log };

        #endregion

        #region Properties

        //public override double Math(int i, int j)
        //{
        //    double f = e.Math(i, j);
        //    switch (effx)
        //    {
        //        case FunctionType.Cos: return Math.Cos(f);
        //        case FunctionType.Sin: return Math.Sin(f);
        //        case FunctionType.Ln: return Math.Log(f);
        //        case FunctionType.Log: return Math.Log10(f);
        //        default:
        //            return f;
        //    }
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
            if (cc.IsFirstPass())
            {
                e.Compile(g, cc);
                return;
            }
            MethodInfo mi = null;

            switch (effx)
            {
                case FunctionType.Cos: mi = typeof(Math).GetMethod("Cos"); break;
                case FunctionType.Sin: mi = typeof(Math).GetMethod("Sin"); break;
                case FunctionType.Ln: mi = typeof(Math).GetMethod("Log"); break;
                case FunctionType.Log: mi = typeof(Math).GetMethod("Log10"); break;
                default:
                    break;
            }
            if (mi == null) return;

            e.Compile(g, cc);

            g.EmitCall(OpCodes.Call, mi, null);
        }

        #endregion
    }
}
