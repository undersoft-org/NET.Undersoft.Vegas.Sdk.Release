/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealTransfer.cs
   
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
    /// Defines the <see cref="DealTransfer" />.
    /// </summary>
    public class DealTransfer : IDisposable
    {
        #region Fields

        public ITransferContext Context;
        public DealHeader HeaderReceived;
        public MemberIdentity Identity;
        public TransferManager Manager;
        public DealMessage MessageReceived;
        public DealHeader MyHeader;
        private DealMessage mymessage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealTransfer"/> class.
        /// </summary>
        public DealTransfer()
        {
            MyHeader = new DealHeader(this);
            Manager = new TransferManager(this);
            MyMessage = new DealMessage(this, DirectionType.Send, null);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealTransfer"/> class.
        /// </summary>
        /// <param name="identity">The identity<see cref="MemberIdentity"/>.</param>
        /// <param name="message">The message<see cref="object"/>.</param>
        /// <param name="context">The context<see cref="ITransferContext"/>.</param>
        public DealTransfer(MemberIdentity identity, object message = null, ITransferContext context = null)
        {
            Context = context;
            if (Context != null)
                MyHeader = new DealHeader(this, Context, identity);
            else
                MyHeader = new DealHeader(this, identity);

            Identity = identity;
            Manager = new TransferManager(this);
            MyMessage = new DealMessage(this, DirectionType.Send, message);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the MyMessage.
        /// </summary>
        public DealMessage MyMessage
        {
            get
            {
                return mymessage;
            }
            set
            {
                mymessage = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (MyHeader != null)
                MyHeader.Dispose();
            if (mymessage != null)
                mymessage.Dispose();
            if (HeaderReceived != null)
                HeaderReceived.Dispose();
            if (MessageReceived != null)
                MessageReceived.Dispose();
            if (Context != null)
                Context.Dispose();
        }

        #endregion
    }
}
