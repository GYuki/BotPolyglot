namespace LogicBlock.Info
{
    public interface IRequestInfo
    {
        IOperationRequest OperationRequest { get; }
    
        void Success(string msg);

        void Fail(string msg);

        byte Status { get; }

        bool IsNew { get; }

        bool IsSucceeded { get; }

        bool IsFailed { get; }

        bool IsProcessed { get; }
    }

    public interface ITypedRequestInfo<out RequestType> : IRequestInfo
        where RequestType : IOperationRequest
    {
        RequestType Request { get; }
    }

    public interface ITextRequestInfo : ITypedRequestInfo<ITextOperationRequest>
    {
    }
}