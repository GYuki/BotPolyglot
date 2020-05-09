using System.Threading.Tasks;
using LogicBlock.Session;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public abstract class BaseLogic : ILogic
    {
        public abstract Task<ResponseModel> Act(string message, ChatSession session);
        public virtual string Back(ChatSession session)
        {
            return null;
        }
        public string Menu(ChatSession session)
        {
            session.State = State.Idle;
            session.ExpectedWord = 0;
            session.WordSequence = null;
            return "Idle message";
        }
    }
}