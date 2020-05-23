using System.Threading.Tasks;
using LogicBlock.Session;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public interface ILogic
    {
        Task<ResponseModel> Act(string message, ChatSession session);
        Task<string> Back(ChatSession session);
        Task<string> Menu(ChatSession session);
    }

    public interface IIdleLogic : ILogic {}
    public interface ILanguageLogic : ILogic {}
    public interface IModeChooseLogic: ILogic {}
    public interface IActionLogic: ILogic {
        Task<string> GetNextTask(ChatSession session);
    }
    public interface IArcadeActionLogic: IActionLogic {}
    public interface ITutorialActionLogic: IActionLogic {}
}