using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace System.Instant.Mathset
{
    // Binary Member By Member Oprator Formula
    [Serializable]
    public class CompareOperation : BinaryFormula
    {
        BinaryOperator oper;
        //MathsetSize size;

        public CompareOperation(Formula e1, Formula e2, BinaryOperator op) : base(e1, e2)
        {
            oper = op;
            //MathsetSize o1 = expr1.Size;
            //MathsetSize o2 = expr2.Size;

            //if (o1 != o2 && (o1 != MathsetSize.Scalar && o2 != MathsetSize.Scalar))
            //    throw new SizeMismatchException("Binary Memberwise Mismatch");

            //size = o1 == MathsetSize.Scalar ? o2 : o1;
        }

        // Compilation First Pass: check sizes
        // Code Generation: the operator code
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            expr1.Compile(g, cc);
            expr2.Compile(g, cc);
            if (cc.IsFirstPass())
                return;
            oper.Compile(g);
        }

        //public override double Math(int i, int j)
        //{
        //    return oper.Apply(expr1.Math(i, j), expr2.Math(i, j));
        //}

        public override MathsetSize Size
        {
            get { return expr1.Size == MathsetSize.Scalar ? expr2.Size : expr1.Size; }
        }
    }
}
