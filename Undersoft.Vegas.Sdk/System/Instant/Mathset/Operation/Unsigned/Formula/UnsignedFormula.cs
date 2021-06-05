using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace System.Instant.Mathset
{
    [Serializable]
    public class UnsignedFormula : Formula
    {
        double thevalue;

        public UnsignedFormula(double vv)
        {
            thevalue = vv;
        }

        // First Pass: none
        // Push a float literal (partial evaluation)
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass()) return;
            g.Emit(OpCodes.Ldc_R8, thevalue);
        }

        //public override double Math(int i, int j)
        //{
        //    return thevalue;
        //}

        public override MathsetSize Size
        {
            get { return MathsetSize.Scalar; }
        }
    }
}
