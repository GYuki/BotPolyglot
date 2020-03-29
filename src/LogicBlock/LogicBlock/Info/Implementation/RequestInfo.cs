using System;

namespace LogicBlock.Info
{
    public abstract class RequestInfo : IRequestInfo
    {
        public Action<IResponseInfo> OnFail;
        public Action<IResponseInfo> OnSuccess;
        
        public IOperationRequest OperationRequest { get; set; }
        public byte Status { get; protected set; }
        public bool IsNew => Status == CallStatus.New;
        public bool IsSucceeded => Status == CallStatus.Succeeded;
        public bool IsFailed => Status == CallStatus.Failed;
        public bool IsProcessed => Status >= CallStatus.Succeeded;

        public RequestInfo()
        {

        }

        public void Success(IResponseInfo message)
        {
            if (IsProcessed)
                throw new Exception("Already called Success/Fail");
            
            OnSuccess?.Invoke(message);

            Status = CallStatus.Succeeded;
        }

        public void Fail(IResponseInfo message)
        {
            if (IsProcessed)
                throw new Exception("Already called Success/Fail");
            
            Status = CallStatus.Failed;

            OnFail?.Invoke(message);
        }
    }

    public abstract class TypedRequestInfo<RequestType> : RequestInfo, ITypedRequestInfo<RequestType>
        where RequestType : IOperationRequest
    {
        protected TypedRequestInfo()
            : base()
        {

        }
        public RequestType Request
        {
            get
            {
                return (RequestType)OperationRequest;
            }

            set
            {
                OperationRequest = value;
            }
        }
    }

    public class TextRequestInfo : TypedRequestInfo<ITextOperationRequest>, ITextRequestInfo
    {
        public TextRequestInfo()
            : base()
        {
            
        }
    }
}