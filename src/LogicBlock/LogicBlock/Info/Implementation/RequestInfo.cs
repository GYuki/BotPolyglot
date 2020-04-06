using System;

namespace LogicBlock.Info
{
    public abstract class RequestInfo : IRequestInfo
    {
        
        public IOperationRequest OperationRequest { get; set; }

        public RequestInfo()
        {

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

    public class StartRequestInfo : TypedRequestInfo<IStartOperationRequest>, IStartRequestInfo
    {
        public StartRequestInfo()
            : base()
        {
        }
    }
}