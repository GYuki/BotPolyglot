using System.Threading.Tasks;
using LogicBlock.Session;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public interface ILogic
    {
        Task<ResponseModel> Act(string message, ChatSession session);
        string Back(ChatSession session);
        string Menu(ChatSession session);
    }

    public interface IIdleLogic : ILogic {}
    public interface ILanguageLogic : ILogic {}
    public interface IModeChooseLogic: ILogic {}
    public interface IActionLogic: ILogic {}
    public interface IArcadeActionLogic: IActionLogic {}
    public interface ITutorialActionLogic: IActionLogic {}
}