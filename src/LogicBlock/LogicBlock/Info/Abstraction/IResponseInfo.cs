
namespace LogicBlock.Info
{
    public interface IResponseInfo
    {
        string Message { get; }
    }

    public interface ITutorialResponseInfo : IResponseInfo
    {

    }

    public interface IArcadeResponseInfo : IResponseInfo
    {
        int Award { get; }
    }
}