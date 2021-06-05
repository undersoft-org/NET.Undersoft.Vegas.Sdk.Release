/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.ReckonableOperand.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;

    #region Enums

    [Serializable]
    public enum ComputeableOperand
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide
    }

    #endregion
}
