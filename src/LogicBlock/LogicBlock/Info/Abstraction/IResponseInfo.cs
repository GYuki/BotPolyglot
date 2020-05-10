
namespace LogicBlock.Info
{
    public interface IResponseInfo
    {
        byte ResponseCode { get; }
        string Message { get; }
    }

    public interface IStartResponseInfo : IResponseInfo
    {
        
    }

    public interface ITutorialResponseInfo : IResponseInfo
    {

    }

    public interface IArcadeResponseInfo : IResponseInfo
    {
        int Award { get; }
    }

    public interface IAfterActionResponseInfo: IResponseInfo
    {
        
    }
}