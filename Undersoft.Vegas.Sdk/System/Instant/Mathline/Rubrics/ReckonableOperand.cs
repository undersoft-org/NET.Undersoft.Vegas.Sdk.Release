using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace System.Instant.Mathline
{   
    [Serializable]
    public enum ComputeableOperand
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
