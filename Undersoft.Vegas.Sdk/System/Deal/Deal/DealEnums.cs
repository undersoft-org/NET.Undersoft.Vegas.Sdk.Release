/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealEnums.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;

    #region Enums

    [Serializable]
    public enum DirectionType
    {
        Send,
        Receive,
        None
    }
    [Serializable]
    public enum DealProtocol
    {
        NONE,
        DOTP,
        HTTP
    }
    [Serializable]
    public enum ProtocolMethod
    {
        NONE,
        DEAL,
        SYNC,
        GET,
        POST,
        OPTIONS,
        PUT,
        DELETE,
        PATCH
    }
    [Serializable]
    public enum DealComplexity
    {
        Guide,
        Basic,
        Standard,
        Advanced
    }
    [Serializable]
    public enum MessagePart
    {
        Header,
        Message,
    }

    #endregion
}
