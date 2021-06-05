/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealEvent.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Instant;

    /// <summary>
    /// Defines the <see cref="DealEvent" />.
    /// </summary>
    [Serializable]
    public class DealEvent : Deputy
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealEvent"/> class.
        /// </summary>
        /// <param name="MethodName">The MethodName<see cref="string"/>.</param>
        /// <param name="TargetClassObject">The TargetClassObject<see cref="object"/>.</param>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        public DealEvent(string MethodName, object TargetClassObject, params object[] parameters) : base(TargetClassObject, MethodName)
        {
            base.ParameterValues = parameters;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealEvent"/> class.
        /// </summary>
        /// <param name="MethodName">The MethodName<see cref="string"/>.</param>
        /// <param name="TargetClassName">The TargetClassName<see cref="string"/>.</param>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        public DealEvent(string MethodName, string TargetClassName, params object[] parameters) : base(TargetClassName, MethodName)
        {
            base.ParameterValues = parameters;
        }

        #endregion
    }
}
