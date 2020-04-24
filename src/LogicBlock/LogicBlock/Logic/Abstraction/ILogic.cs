using System.Threading.Tasks;
using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public interface ILogic
    {
        Task<IResponseInfo> StartChat(IStartRequestInfo info);
        Task<IResponseInfo> HandleText(ITextRequestInfo info);
        Task<IResponseInfo> AfterAction(IAfterActionRequestInfo info);
    }

    public interface ITutorialLogic : ILogic {}
    public interface IArcadeLogic : ILogic {}
}