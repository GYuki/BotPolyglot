
namespace LogicBlock.Info
{
    public interface IResponseInfo
    {
        bool Success { get; }
    }

    public interface IStartResponseInfo : IResponseInfo
    {
        
    }

    public interface ITutorialResponseInfo : IResponseInfo
    {

    }

    public interface IArcadeResponseInfo : IResponseInfo
    {
        string Message { get; }
        int Award { get; }
    }
}