namespace LogicBlock.Info
{
    public interface IOperationRequest
    {
        int SenderId { get; }

        string[] Commands { get; }
    }

    public interface ITextOperationRequest : IOperationRequest
    {
        string MessageText { get; }
    }
}