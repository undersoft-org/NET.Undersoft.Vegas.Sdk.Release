using System.Text;
using System.Linq;
using System.Collections;
using System.Sets;
using System.Instant;
using System;

namespace System.Deal
{   
    public class TransferOperation
    { 
        private DealTransfer transaction;
        private DealContext transportContext;

        private ITransferContext transferContext;
        private ServiceSite site;
        private DirectionType direction;
        private MessagePart part;
        private DealProtocol protocol;
        private ProtocolMethod method;

        public TransferOperation(DealTransfer _transaction, MessagePart _part, DirectionType _direction)
        {
            transaction = _transaction;
            transferContext = transaction.Context;
            transportContext = transaction.MyHeader.Context;
            site = transportContext.IdentitySite;
            direction = _direction;
            part = _part;
            protocol = transferContext.Protocol;
            method = transferContext.Method;
        }

        public void Resolve(ISerialBuffer buffer = null)
        {
            switch (protocol)
            {
                case DealProtocol.DOTP:
                    switch (site)
                    {
                        case ServiceSite.Server:
                            switch (direction)
                            {
                                case DirectionType.Receive:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            SrvRecHead(buffer);
                                            break;
                                        case MessagePart.Message:
                                            SrvRecMsg(buffer);
                                            break;
                                    }
                                    break;
                                case DirectionType.Send:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            SrvSendHead();
                                            break;
                                        case MessagePart.Message:
                                            SrvSendMsg();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case ServiceSite.Client:
                            switch (direction)
                            {
                                case DirectionType.Receive:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            CltRecHead(buffer);
                                            break;
                                        case MessagePart.Message:
                                            CltRecMsg(buffer);
                                            break;
                                    }
                                    break;
                                case DirectionType.Send:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            CltSendHead();
                                            break;
                                        case MessagePart.Message:
                                            CltSendMsg();
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            
                case DealProtocol.NONE:              
                    switch (site)
                    {
                        case ServiceSite.Server:
                            switch (direction)
                            {
                                case DirectionType.Receive:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            SrvRecHead(buffer);
                                            break;
                                        case MessagePart.Message:
                                            SrvRecMsg(buffer);
                                            break;
                                    }
                                    break;
                                case DirectionType.Send:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            SrvSendHead();
                                            break;
                                        case MessagePart.Message:
                                            SrvSendMsg();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case ServiceSite.Client:
                            switch (direction)
                            {
                                case DirectionType.Receive:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            CltRecHead(buffer);
                                            break;
                                        case MessagePart.Message:
                                            CltRecMsg(buffer);
                                            break;
                                    }
                                    break;
                                case DirectionType.Send:
                                    switch (part)
                                    {
                                        case MessagePart.Header:
                                            CltSendHead();
                                            break;
                                        case MessagePart.Message:
                                            CltSendMsg();
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DealProtocol.HTTP:
                    switch(method)
                    {
                        case ProtocolMethod.GET:
                            switch (site)
                            {
                                case ServiceSite.Server:
                                    switch (direction)
                                    {
                                        case DirectionType.Receive:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                    SrvRecGet(buffer);
                                                    break;                                              
                                            }
                                            break;
                                        case DirectionType.Send:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                    SrvSendGet();
                                                    break;                                               
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case ProtocolMethod.POST:
                            switch (site)
                            {
                                case ServiceSite.Server:
                                    switch (direction)
                                    {
                                        case DirectionType.Receive:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                     SrvRecPost(buffer);
                                                    break;
                                            }
                                            break;
                                        case DirectionType.Send:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                    SrvSendPost();
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case ProtocolMethod.OPTIONS:
                            switch (site)
                            {
                                case ServiceSite.Server:
                                    switch (direction)
                                    {
                                        case DirectionType.Receive:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                    SrvRecOptions();
                                                    break;
                                            }
                                            break;
                                        case DirectionType.Send:
                                            switch (part)
                                            {
                                                case MessagePart.Header:
                                                    SrvSendOptions();
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void SrvRecHead(ISerialBuffer buffer)
        {
            bool isError = false;
            string errorMessage = "";
            try
            {
                DealHeader headerObject = (DealHeader)transaction.MyHeader.Deserialize(buffer);
                if (headerObject != null)
                {
                    transaction.HeaderReceived =  headerObject;

                    if (DealServer.Security.Register(transaction.HeaderReceived.Context.Identity, true))
                    {
                        transaction.MyHeader.Context.Identity.UserId = transaction.HeaderReceived.Context.Identity.UserId;
                        transaction.MyHeader.Context.Identity.Token = transaction.HeaderReceived.Context.Identity.Token;
                        transaction.MyHeader.Context.Identity.DeptId = transaction.HeaderReceived.Context.Identity.DeptId;

                        if (transaction.HeaderReceived.Context.ContentType != null)
                        {

                            object _content = transaction.HeaderReceived.Content;
                            //((ISettings)_content).State.Synced = false;

                            Type[] ifaces = _content.GetType().GetInterfaces();
                            if (ifaces.Contains(typeof(ISerialFormatter)) && ifaces.Contains(typeof(ISerialObject)))
                            {
                                int objectCount = transaction.HeaderReceived.Context.ObjectsCount;
                                transferContext.Synchronic = transaction.HeaderReceived.Context.Synchronic;

                                object myheader = ((ISerialObject)_content).Locate();

                                if (myheader != null)
                                {
                                    if (objectCount == 0)
                                    {
                                        transferContext.ReceiveMessage = false;

                                        //if (((ISettings)_content).Config.DealerId.IsNotEmpty)
                                        //    transaction.MyHeader.Content = ((ISerialObject)myheader).Merge(_content);
                                        //else
                                        //{
                                        //    ((ISettings)myheader).State.Expeled = true;
                                            transaction.MyHeader.Content = myheader;
                                       // }
                                    }
                                    else
                                    {
                                        transaction.MyHeader.Content = ((ISerialObject)myheader).Merge(_content);
                                        transaction.MessageReceived = new DealMessage(transaction, DirectionType.Receive, transaction.MyHeader.Content);
                                    }
                                }
                                else
                                {
                                    isError = true;
                                    errorMessage += "Prime not exist - incorrect object target ";
                                }
                            }
                            else
                            {
                                isError = true;
                                errorMessage += "Incorrect DPOT object - deserialization error ";
                            }
                        }
                        else
                        {
                            transaction.MyHeader.Content = new Hashtable() { { "Register", true } };
                            transaction.MyHeader.Context.Echo += "Registration success - ContentType: null ";
                        }
                    }
                    else
                    {
                        isError = true;
                        transaction.MyHeader.Content = new Hashtable() { { "Register", false } };
                        transaction.MyHeader.Context.Echo += "Registration failed - access denied ";
                    }
                }
                else
                {
                    isError = true;
                    errorMessage += "Incorrect DPOT object - deserialization error ";
                }
            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage += ex.ToString();
            }

            if (isError)
            {
                transaction.Context.Close = true;
                transaction.Context.ReceiveMessage = false;
                transaction.Context.SendMessage = false;

                if (errorMessage != "")
                {
                    transaction.MyHeader.Content += errorMessage;
                    transaction.MyHeader.Context.Echo += errorMessage;
                }
                transaction.MyHeader.Context.Errors++;
            }
        }
        private void SrvRecMsg(ISerialBuffer buffer)
        {
            object serialCardsObj = ((object[])transaction.MessageReceived.Content)[buffer.DeserialBlockId];
            object deserialCardsObj = ((ISerialFormatter)serialCardsObj).Deserialize(buffer);
            ISerialFormatter deserialCards = (ISerialFormatter)deserialCardsObj;
            if (deserialCards.DeserialCount <= deserialCards.ProgressCount || deserialCards.ProgressCount == 0)
            {
                transaction.Context.ObjectsLeft--;
                deserialCards.ProgressCount = 0;               
            }
        }

        private void SrvSendHead()
        {
            transaction.Manager.HeaderContent(transferContext.Transfer.MyHeader.Content,
                                              transferContext.Transfer.MyHeader.Content, 
                                              DirectionType.Send);

            if (transaction.MyHeader.Context.ObjectsCount == 0)
                transferContext.SendMessage = false;

            transferContext.Transfer.MyHeader.Serialize(transferContext, 0, 0);
        }
        private void SrvSendMsg()
        {            
            int serialBlockId = ((ISerialFormatter[])transaction.MyMessage.Content)[transferContext.ObjectPosition].Serialize(transferContext, transferContext.SerialBlockId, 5000);

            if (serialBlockId < 0)
            {
                if (transferContext.ObjectPosition < (transaction.MyHeader.Context.ObjectsCount - 1))
                {
                    transferContext.ObjectPosition++;
                    transferContext.SerialBlockId = 0;
                    return;
                }           
            }
            transferContext.SerialBlockId = serialBlockId;
        }

        private void CltRecHead(ISerialBuffer buffer)
        {
            DealHeader headerObject = (DealHeader)transaction.MyHeader.Deserialize(buffer);

            if (headerObject != null)
            {
                transaction.HeaderReceived = headerObject;

                transaction.MyHeader.Context.Identity.Key = null;
                transaction.MyHeader.Context.Identity.Name = null;
                transaction.MyHeader.Context.Identity.UserId = transaction.HeaderReceived.Context.Identity.UserId;
                transaction.MyHeader.Context.Identity.Token = transaction.HeaderReceived.Context.Identity.Token;
                transaction.MyHeader.Context.Identity.DeptId = transaction.HeaderReceived.Context.Identity.DeptId;

                object reciveContent = transaction.HeaderReceived.Content;

                Type[] ifaces = reciveContent.GetType().GetInterfaces();
                if (ifaces.Contains(typeof(ISerialFormatter)) && ifaces.Contains(typeof(ISerialObject)))
                {
                    if (transaction.MyHeader.Content == null)
                        transaction.MyHeader.Content = ((ISerialObject)reciveContent).Locate();

                    object myContent = transaction.MyHeader.Content;

                    ((ISerialObject)myContent).Merge(reciveContent);

                    int objectCount = transaction.HeaderReceived.Context.ObjectsCount;
                    if (objectCount == 0)
                        transferContext.ReceiveMessage = false;
                    else
                        transaction.MessageReceived = new DealMessage(transaction, DirectionType.Receive, myContent);
                }
                else if(reciveContent is Hashtable)
                {
                    Hashtable hashTable = (Hashtable)reciveContent;
                    if (hashTable.Contains("Register"))
                    {
                        transferContext.Denied = !(bool)hashTable["Register"];
                        if (transferContext.Denied)
                        {
                            transferContext.Close = true;
                            transferContext.ReceiveMessage = false;
                            transferContext.SendMessage = false;
                        }
                    }
                }
                else
                    transferContext.SendMessage = false;
            }
        }
        private void CltRecMsg(ISerialBuffer buffer)
        {
            object serialCardsObj = ((object[])transaction.MessageReceived.Content)[buffer.DeserialBlockId];
            ISerialFormatter serialCards = (ISerialFormatter)serialCardsObj;

            object deserialCardsObj = serialCards.Deserialize(buffer);
            ISerialFormatter deserialCards = (ISerialFormatter)deserialCardsObj;
            if (deserialCards.DeserialCount <= deserialCards.ProgressCount || deserialCards.ProgressCount == 0)
            {
                transaction.Context.ObjectsLeft--;
                deserialCards.ProgressCount = 0;
            }
        }

        private void CltSendHead()
        {
            transaction.Manager.HeaderContent(transferContext.Transfer.MyHeader.Content, 
                                              transferContext.Transfer.MyHeader.Content, 
                                              DirectionType.Send);

            if (transaction.MyHeader.Context.ObjectsCount == 0)
                transferContext.SendMessage = false;

            transferContext.Transfer.MyHeader.Serialize(transferContext, 0, 0);
        }
        private void CltSendMsg()
        {
            object serialcards = ((object[])transaction.MyMessage.Content)[transferContext.ObjectPosition];

            int serialBlockId = ((ISerialFormatter)serialcards).Serialize(transferContext, transferContext.SerialBlockId, 5000);
            if (serialBlockId < 0)
            {
                if (transferContext.ObjectPosition < (transaction.MyHeader.Context.ObjectsCount - 1))
                {
                    transferContext.ObjectPosition++;
                    transferContext.SerialBlockId = 0;
                    return;
                }
            }
            transferContext.SerialBlockId = serialBlockId;
        }

        private void SrvRecGet(ISerialBuffer buffer)
        {
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
            transaction.HeaderReceived = transaction.MyHeader;        
            transferContext.HandleGetRequest();
        }
        private void SrvSendGet()
        {
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;           
        }

        private void SrvRecPost(ISerialBuffer buffer)
        {
            if (SrvRecPostDealer(buffer))
                transaction.HeaderReceived = transaction.MyHeader;
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
        }
        private bool SrvRecPostDealer(ISerialBuffer buffer)
        {
            bool isError = false;
            string errorMessage = "";
            try
            {
                byte[] _array = buffer.DeserialBlock;
                StringBuilder sb = new StringBuilder();
                sb.Append(_array.ToChars(CharEncoding.UTF8));

                string dpttransx = sb.ToString();
                int msgid = dpttransx.IndexOf(",\"DealMessage\":");
                string dptheadx = dpttransx.Substring(0, msgid) + "}";
                string dptmsgx = "{" + dpttransx.Substring(msgid, dpttransx.Length - msgid).Trim(',');

                string[] msgcntsx = dptmsgx.Split(new string[] { "\"Content\":" }, StringSplitOptions.RemoveEmptyEntries);
                string[] cntarrays = msgcntsx.Length > 0 ? msgcntsx[1].Split(new string[] { "\"Cards\":" }, StringSplitOptions.None) : null;
                int objectCount = 0;
                if (cntarrays != null)
                    for (int i = 1; i < cntarrays.Length; i += 1)
                    {
                        string[] itemarray = cntarrays[i].Split('[');
                        for (int x = 1; x < itemarray.Length; x += 1)
                        {
                            if (itemarray[x].IndexOf(']') > 0)
                                objectCount++;
                        }
                    }

                string msgcntx = msgcntsx[1].Trim(' ').Substring(0, 6);

                object dptheadb = dptheadx;
                object dptmsgb = dptmsgx;

                isError = SrvRecPostDealHeader(buffer);
                if(objectCount > 0 && !isError)
                    isError = SrvRecPostDealMessage(dptmsgb);

            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage = ex.ToString();
            }

            if (isError)
            {
                transaction.Context.SendMessage = false;
                if (errorMessage != "")
                {
                    transaction.MyHeader.Content += errorMessage;
                    transaction.MyHeader.Context.Echo += errorMessage;
                }
                transaction.MyHeader.Context.Errors++;
            }
            return isError;
        }
        private bool SrvRecPostDealHeader(ISerialBuffer buffer)
        { 
            bool isError = false;
            string errorMessage = "";
            try
            {
                DealHeader headerObject = (DealHeader)transaction.MyHeader.Deserialize(buffer, SerialFormat.Json);
                headerObject.Context.Identity.Ip = transaction.MyHeader.Context.RemoteEndPoint.Address.ToString();
                if (DealServer.Security.Register(headerObject.Context.Identity, true))
                {
                    transaction.HeaderReceived = (headerObject != null) ? headerObject : null;
                    transaction.MyHeader.Context.Complexity = headerObject.Context.Complexity;
                    transaction.MyHeader.Context.Identity =  headerObject.Context.Identity;

                    if (headerObject.Context.ContentType != null)
                    {

                        object instance = new object();
                        JsonParser.PrepareInstance(out instance, headerObject.Context.ContentType);
                        object content = headerObject.Content;
                        object result = ((ISerialFormatter)instance).Deserialize(buffer, SerialFormat.Json);
                        transaction.HeaderReceived.Content = result;
                        object _content = transaction.HeaderReceived.Content;

                        Type[] ifaces = _content.GetType().GetInterfaces();
                        if (ifaces.Contains(typeof(ISerialFormatter)) && ifaces.Contains(typeof(ISerialObject)))
                        {
                            int objectCount = buffer.DeserialBlockId;

                            object myheader = ((ISerialObject)_content).Locate();

                            //Settings clientConfig = ((ISettings)_content).Config;

                            if (myheader != null)
                            {
                                if (objectCount == 0)
                                {
                                    transferContext.ReceiveMessage = false;

                                    //if (clientConfig.DealerId.IsNotEmpty)
                                    //    transaction.MyHeader.Content = ((ISerialObject)myheader).Merge(_content);
                                    //else
                                    //{
                                      //  ((ISettings)myheader).State.Expeled = true;
                                        transaction.MyHeader.Content = myheader;
                                   // }
                                }
                                else
                                {
                                    transaction.MyHeader.Content = ((ISerialObject)myheader).Merge(_content);
                                    transaction.MessageReceived = new DealMessage(transaction, DirectionType.Receive, transaction.MyHeader.Content);
                                }
                            }
                            else
                            {
                                isError = true;
                                errorMessage += "Prime not exist - incorrect object target ";
                            }
                        }
                        else
                        {
                            isError = true;
                            errorMessage += "Incorrect DPOT object - deserialization error ";
                        }
                    }
                    else
                    {
                        transaction.MyHeader.Content = new Hashtable() { { "Register", true } };
                        transaction.MyHeader.Context.Echo += "Registration success - ContentType: null ";
                    }
                }
                else
                {
                    isError = true;
                    transaction.MyHeader.Content = new Hashtable() { { "Register", false } };
                    transaction.MyHeader.Context.Echo += "Registration failed - access denied ";
                }

            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage += ex.ToString();
            }

            if (isError)
            {
                transaction.Context.SendMessage = false;
                if (errorMessage != "")
                {
                    transaction.MyHeader.Content += errorMessage;
                    transaction.MyHeader.Context.Echo += errorMessage;
                }
                transaction.MyHeader.Context.Errors++;
            }
            return isError;
        }
        private bool SrvRecPostDealMessage(object buffer)
        {
            bool isError = false;
            string errorMessage = "";
            try
            {
               // ((ISerialFormatter[])transaction.MessageReceived.Content).DeserializeJsonCards(buffer);                
            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage += ex.ToString();
            }

            if (isError)
            {
                transaction.Context.SendMessage = false;
                if (errorMessage != "")
                {
                    transaction.MyHeader.Content = "Prime not exist - incorrect object path";
                    transaction.MyHeader.Context.Echo = "Error - Prime not exist - incorrect object path";
                }
                transaction.MyHeader.Context.Errors++;
            }
            return isError;
        }

        private void SrvSendPost()
        {
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
            transferContext.RequestBuilder.Clear();
            if (!transferContext.Denied)
            {
                SrvSendPostDealer();
                transferContext.HandlePostRequest("application/json");
            }
            else
                transferContext.HandleDeniedRequest();
        }
        private void SrvSendPostDealer()
        {
            SrvSendPostDealHeader();
            SrvSendPostDealMessage();            
        }
        private void SrvSendPostDealHeader()
        {
            transaction.Manager.HeaderContent(transferContext.Transfer.MyHeader.Content, transferContext.Transfer.MyHeader.Content, DirectionType.Send);
            transaction.MyHeader.SetJson(transferContext.RequestBuilder);
        }
        private void SrvSendPostDealMessage()
        {
            StringBuilder msgcnt = new StringBuilder();

            Type[] ifaces = transaction.MyMessage.Content.GetType().GetInterfaces();
           // if (ifaces.Contains(typeof(ICollection)) && transaction.MyHeader.Context.Errors == 0)
              //  ((ISerialFormatter[])transaction.MyMessage.Content).SerializeJsonCards(msgcnt, 0, 0, transaction.MyHeader.Context.Complexity);
          //  else
                msgcnt.Append("null");

            transaction.MyMessage.Content = new object();
            transaction.MyMessage.SetJson(transferContext.RequestBuilder);
            string msg = msgcnt.ToString().Replace("}\r\n{", ",").Trim(new char[] { '\n', '\r' });
            transferContext.RequestBuilder.Replace("\"Content\":{}", "\"Content\":" + msgcnt.ToString());          
            transferContext.RequestBuilder.Replace("}\r\n{", ",");
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
        }

        private void SrvRecOptions()
        {
            transaction.HeaderReceived = transaction.MyHeader;
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
            transferContext.HandleOptionsRequest("application/json");
        }
        private void SrvSendOptions()
        {
            transferContext.SendMessage = false;
            transferContext.ReceiveMessage = false;
        }
    }
}
