/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Net;

    /// <summary>
    /// Defines the <see cref="DealContext" />.
    /// </summary>
    [Serializable]
    public class DealContext
    {
        #region Fields

        public string EventClass;
        public string EventMethod;
        [NonSerialized] public IPEndPoint LocalEndPoint;
        [NonSerialized] public IPEndPoint RemoteEndPoint;
        [NonSerialized] private Type contentType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Complexity.
        /// </summary>
        public DealComplexity Complexity { get; set; } = DealComplexity.Standard;

        /// <summary>
        /// Gets or sets the ContentType.
        /// </summary>
        public Type ContentType
        {
            get
            {
                if (contentType == null && ContentTypeName != null)
                    ContentType = Assemblies.FindType(ContentTypeName);
                return contentType;
            }
            set
            {
                if (value != null)
                {
                    ContentTypeName = value.FullName;
                    contentType = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ContentTypeName.
        /// </summary>
        public string ContentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Echo.
        /// </summary>
        public string Echo { get; set; }

        /// <summary>
        /// Gets or sets the Errors.
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// Gets or sets the Identity.
        /// </summary>
        public MemberIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the IdentitySite.
        /// </summary>
        public ServiceSite IdentitySite { get; set; } = ServiceSite.Client;

        /// <summary>
        /// Gets or sets the ObjectsCount.
        /// </summary>
        public int ObjectsCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether ReceiveMessage.
        /// </summary>
        public bool ReceiveMessage { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether SendMessage.
        /// </summary>
        public bool SendMessage { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether Synchronic.
        /// </summary>
        public bool Synchronic { get; set; } = false;

        #endregion
    }
}
