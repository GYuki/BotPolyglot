namespace LogicBlock.Info
{
    public interface IRequestInfo
    {
        IOperationRequest OperationRequest { get; }
    }

    public interface ITypedRequestInfo<out RequestType> : IRequestInfo
        where RequestType : IOperationRequest
    {
        RequestType Request { get; }
    }

    public interface ITextRequestInfo : ITypedRequestInfo<ITextOperationRequest>
    {
    }

    public interface IStartRequestInfo : ITypedRequestInfo<IStartOperationRequest>
    {
    }

    public interface IAfterActionRequestInfo : ITypedRequestInfo<IAfterActionOperationRequest>
    {
    }
}