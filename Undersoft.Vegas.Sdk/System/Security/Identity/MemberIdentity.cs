/*************************************************
   Copyright (c) 2021 Undersoft

   System.MemberIdentity.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    #region Enums

    [Serializable]
    public enum ServiceSite
    {
        Client,
        Server
    }
    public enum IdentityType
    {
        User,
        Server,
        Service
    }

    #endregion

    [Serializable]
    public class MemberIdentity
    {
        #region Fields

        public ServiceSite Site;
        public IdentityType Type;

        #endregion

        #region Properties

        public bool Active { get; set; }

        public string AuthId { get; set; }

        public string DataPlace { get; set; }

        public string DeptId { get; set; }

        public string Host { get; set; }

        public int Id { get; set; }

        public string Ip { get; set; }

        public string Key { get; set; }

        public DateTime LastAction { get; set; }

        public DateTime LifeTime { get; set; }

        public int Limit { get; set; }

        public string Name { get; set; }

        public int Port { get; set; }

        public DateTime RegisterTime { get; set; }

        public string Salt { get; set; }

        public int Scale { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }

        #endregion
    }
}
