/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealManager.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;

    /// <summary>
    /// Defines the <see cref="DealManager" />.
    /// </summary>
    public class DealManager : IDisposable
    {
        #region Fields

        public ITransferContext transferContext;
        private DealContext dealContext;
        private ServiceSite site;
        private DealTransfer transfer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealManager"/> class.
        /// </summary>
        /// <param name="dealTransfer">The dealTransfer<see cref="DealTransfer"/>.</param>
        public DealManager(DealTransfer dealTransfer)
        {
            transfer = dealTransfer;
            transferContext = dealTransfer.Context;
            dealContext = dealTransfer.MyHeader.Context;
            site = dealContext.IdentitySite;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Assign.
        /// </summary>
        /// <param name="content">The content<see cref="object"/>.</param>
        /// <param name="direction">The direction<see cref="DirectionType"/>.</param>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Assign(object content, DirectionType direction, out object[] messages)
        {
            messages = null;

            DealOperation operation = new DealOperation(content, direction, transfer);
            operation.Resolve(out messages);

            if (messages != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (transferContext != null)
                transferContext.Dispose();
        }

        #endregion
    }
}
