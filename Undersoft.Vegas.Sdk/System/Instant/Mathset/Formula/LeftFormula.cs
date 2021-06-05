using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace System.Instant.Mathset
{
    /// <summary>
    /// Incapsulates the LValues, an assignable matrix like Mathset or SubMathset
    /// <summary>   
    [Serializable]
    public abstract class LeftFormula : Formula
    {
        //public abstract void Assign(int i, double v);
        //public abstract void Assign(int i, bool v);
        //public abstract void Assign(int i, int j, double v);
        //public abstract void Assign(int i, int j, bool v);
        public abstract void CompileAssign(ILGenerator g, CompilerContext cc, bool post, bool partial);
    }
}
