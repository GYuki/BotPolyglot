
namespace LogicBlock.Info
{
    public interface IResponseInfo
    {
        string Message { get; }
        Session.Session Session { get; }
    }

    public interface ITutorialResponseInfo : IResponseInfo
    {

    }

    public interface IArcadeResponseInfo : IResponseInfo
    {
        int Award { get; }
    }
}