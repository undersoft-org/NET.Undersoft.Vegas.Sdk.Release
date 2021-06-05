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

    /// <summary>
    /// Defines the <see cref="MemberIdentity" />.
    /// </summary>
    [Serializable]
    public class MemberIdentity
    {
        #region Fields

        public ServiceSite Site;
        public IdentityType Type;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the AuthId.
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// Gets or sets the DataPlace.
        /// </summary>
        public string DataPlace { get; set; }

        /// <summary>
        /// Gets or sets the DeptId.
        /// </summary>
        public string DeptId { get; set; }

        /// <summary>
        /// Gets or sets the Host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Ip.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the LastAction.
        /// </summary>
        public DateTime LastAction { get; set; }

        /// <summary>
        /// Gets or sets the LifeTime.
        /// </summary>
        public DateTime LifeTime { get; set; }

        /// <summary>
        /// Gets or sets the Limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the RegisterTime.
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// Gets or sets the Salt.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the Scale.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public string UserId { get; set; }

        #endregion
    }
}
