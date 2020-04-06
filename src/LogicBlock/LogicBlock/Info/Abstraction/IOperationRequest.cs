using LogicBlock.Session;

namespace LogicBlock.Info
{
    public interface IOperationRequest
    {
        ChatSession Session { get; }
    }

    public interface ITextOperationRequest : IOperationRequest
    {
        string MessageText { get; }
    }

    public interface IStartOperationRequest : IOperationRequest
    {
        
    }
}