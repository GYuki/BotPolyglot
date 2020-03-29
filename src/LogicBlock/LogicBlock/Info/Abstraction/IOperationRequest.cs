namespace LogicBlock.Info
{
    public interface IOperationRequest
    {
        Session.Session Session { get; }
    }

    public interface ITextOperationRequest : IOperationRequest
    {
        string MessageText { get; }
    }
}