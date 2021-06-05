/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.TransferManager.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="TransferManager" />.
    /// </summary>
    public class TransferManager
    {
        #region Fields

        private DealContext context;
        private ServiceSite site;
        private DealTransfer transaction;
        private ITransferContext transferContext;
        private DealManager treatment;// Important Field !!! - Dealer Treatment initiatie, filtering, sorting, saving, editing all treatment here.

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferManager"/> class.
        /// </summary>
        /// <param name="_transaction">The _transaction<see cref="DealTransfer"/>.</param>
        public TransferManager(DealTransfer _transaction)
        {
            transaction = _transaction;
            transferContext = transaction.Context;
            context = transaction.MyHeader.Context;
            site = context.IdentitySite;
            treatment = new DealManager(_transaction);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The HeaderContent.
        /// </summary>
        /// <param name="content">The content<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="_direction">The _direction<see cref="DirectionType"/>.</param>
        public void HeaderContent(object content, object value, DirectionType _direction)
        {
            DirectionType direction = _direction;
            object _content = value;
            if (_content != null)
            {
                Type[] ifaces = _content.GetType().GetInterfaces();
                if (ifaces.Contains(typeof(ISerialFormatter)))
                {
                    transaction.MyHeader.Context.ContentType = _content.GetType();

                    if (direction == DirectionType.Send)
                        _content = ((ISerialFormatter)value).GetHeader();

                    object[] messages_ = null;
                    if (treatment.Assign(_content, direction, out messages_)                               // Dealer Treatment assign with handle its only place where its called and mutate data. 
                    )
                    {
                        if (messages_.Length > 0)
                        {
                            context.ObjectsCount = messages_.Length;
                            for (int i = 0; i < context.ObjectsCount; i++)
                            {
                                ISerialFormatter message = ((ISerialFormatter[])messages_)[i];
                                ISerialFormatter head = (ISerialFormatter)((ISerialFormatter[])messages_)[i].GetHeader();
                                message.SerialCount = message.ItemsCount;
                                head.SerialCount = message.ItemsCount;
                            }

                            if (direction == DirectionType.Send)
                                transaction.MyMessage.Content = messages_;
                            else
                                transaction.MyMessage.Content = ((ISerialFormatter)_content).GetHeader();
                        }
                    }
                }
            }
            content = _content;
        }

        /// <summary>
        /// The MessageContent.
        /// </summary>
        /// <param name="content">The content<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="_direction">The _direction<see cref="DirectionType"/>.</param>
        public void MessageContent(ref object content, object value, DirectionType _direction)
        {
            DirectionType direction = _direction;
            object _content = value;
            if (_content != null)
            {
                if (direction == DirectionType.Receive)
                {
                    Type[] ifaces = _content.GetType().GetInterfaces();
                    if (ifaces.Contains(typeof(ISerialFormatter)))
                    {
                        object[] messages_ = ((ISerialFormatter)value).GetMessage();
                        if (messages_ != null)
                        {
                            int length = messages_.Length;
                            for (int i = 0; i < length; i++)
                            {
                                ISerialFormatter message = ((ISerialFormatter[])messages_)[i];
                                ISerialFormatter head = (ISerialFormatter)((ISerialFormatter[])messages_)[i].GetHeader();
                                message.SerialCount = head.SerialCount;
                                message.DeserialCount = head.DeserialCount;
                            }

                            _content = messages_;
                        }
                    }
                }
            }
            content = _content;
        }

        #endregion
    }
}
