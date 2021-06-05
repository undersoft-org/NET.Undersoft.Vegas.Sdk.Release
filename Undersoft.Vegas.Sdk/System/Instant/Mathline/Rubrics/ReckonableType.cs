using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace System.Instant.Mathline
{   
    [Serializable]
    public enum ComputeableType
    {
        None,
        Value,
        Percent,
        Margin
    }
}
