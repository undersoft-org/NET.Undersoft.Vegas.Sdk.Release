/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.DealOperation.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    #region Enums

    public enum StateFlag : ushort
    {
        Synced = 1,
        Edited = 2,
        Added = 4,
        Quered = 8,
        Saved = 16,
        Canceled = 32
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="DealOperation" />.
    /// </summary>
    public class DealOperation : IDisposable
    {
        #region Fields

        [NonSerialized]
        public ITransferContext transferContext;
        private ISerialFormatter content;
        private DealContext dealContext;
        private DirectionType direction;
        private ServiceSite site;
        private ushort state;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealOperation"/> class.
        /// </summary>
        /// <param name="dealContent">The dealContent<see cref="object"/>.</param>
        public DealOperation(object dealContent)
        {
            site = ServiceSite.Server;
            direction = DirectionType.None;
            state = ((ISerialNumber)dealContent).FlagsBlock;
            content = (ISerialFormatter)dealContent;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DealOperation"/> class.
        /// </summary>
        /// <param name="dealContent">The dealContent<see cref="object"/>.</param>
        /// <param name="directionType">The directionType<see cref="DirectionType"/>.</param>
        /// <param name="transfer">The transfer<see cref="DealTransfer"/>.</param>
        public DealOperation(object dealContent, DirectionType directionType, DealTransfer transfer) : this(dealContent)
        {
            direction = directionType;
            transferContext = transfer.Context;
            dealContext = transfer.MyHeader.Context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        public void Resolve(out object[] messages)
        {
            messages = null;
            switch (site)
            {
                case ServiceSite.Server:
                    switch (direction)
                    {
                        case DirectionType.Receive:
                            // messages = content.GetMessage();
                            break;
                        case DirectionType.Send:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    SrvSendSync(out messages);
                                    break;
                            }
                            break;
                        case DirectionType.None:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    SrvSendSync(out messages);
                                    break;
                            }
                            break;
                    }
                    break;
                case ServiceSite.Client:
                    switch (direction)
                    {
                        case DirectionType.Receive:
                            //messages = content.GetMessage();
                            break;
                        case DirectionType.Send:
                            switch (state & (int)StateFlag.Synced)
                            {
                                case 0:
                                    CltSendSync(out messages);
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// The CltSendSync.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void CltSendSync(out object[] messages)
        {
            if (direction != DirectionType.None)
                if (((state & (int)StateFlag.Edited) > 0) ||
                    ((state & (int)StateFlag.Saved) > 0) ||
                    ((state & (int)StateFlag.Quered) > 0) ||
                    ((state & (int)StateFlag.Canceled) > 0))
                {
                    transferContext.Synchronic = true;
                    dealContext.Synchronic = true;
                }

            messages = content.GetMessage();
        }

        /// <summary>
        /// The SrvSendCancel.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void SrvSendCancel(out object[] messages)
        {
            messages = content.GetMessage();
        }

        /// <summary>
        /// The SrvSendEdit.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void SrvSendEdit(out object[] messages)
        {
            messages = content.GetMessage();
        }

        /// <summary>
        /// The SrvSendQuery.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void SrvSendQuery(out object[] messages)
        {
            messages = content.GetMessage();
        }

        /// <summary>
        /// The SrvSendSave.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void SrvSendSave(out object[] messages)
        {
            messages = content.GetMessage();
        }

        /// <summary>
        /// The SrvSendSync.
        /// </summary>
        /// <param name="messages">The messages<see cref="object[]"/>.</param>
        private void SrvSendSync(out object[] messages)
        {
            if (direction != DirectionType.None)
                if (((state & (int)StateFlag.Edited) > 0) ||
                    ((state & (int)StateFlag.Saved) > 0) ||
                    ((state & (int)StateFlag.Quered) > 0) ||
                    ((state & (int)StateFlag.Canceled) > 0))
                {
                    transferContext.Synchronic = true;
                    dealContext.Synchronic = true;
                }

            messages = null;
            switch (state & (int)StateFlag.Edited)
            {
                case 2:
                    SrvSendEdit(out messages);
                    break;
            }
            switch (state & (int)StateFlag.Canceled)
            {
                case 32:
                    SrvSendCancel(out messages);
                    break;
            }
            switch ((int)state & (int)StateFlag.Saved)
            {
                case 16:
                    SrvSendSave(out messages);
                    break;
            }
            switch (state & (int)StateFlag.Quered)
            {
                case 8:
                    SrvSendQuery(out messages);
                    break;
            }
            if (messages == null)
            {
                messages = content.GetMessage();
            }
        }

        #endregion
    }
}
