using LogicBlock.Session;

namespace Receiver.API.States
{
    public abstract class BaseLogic : ILogic
    {
        public abstract string Act(string message, ChatSession session);
        public virtual string Back(ChatSession session)
        {
            return null;
        }
        public string Menu(ChatSession session)
        {
            session.State = State.Idle;
            return "Idle message";
        }
    }
}