using LogicBlock.Session;

namespace Receiver.API.States
{
    public interface ILogic
    {
        string Act(string message, ChatSession session);
        string Back(ChatSession session);
        string Menu(ChatSession session);
    }

    public interface IIdleLogic : ILogic {}
    public interface ILanguageLogic : ILogic {}
    public interface IModeChooseLogic: ILogic {}
    
}