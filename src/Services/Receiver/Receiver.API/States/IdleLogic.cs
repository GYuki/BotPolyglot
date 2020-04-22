using LogicBlock.Session;

namespace Receiver.API.States
{
    public class IdleLogic : BaseLogic
    {
        public override string Act(string message, ChatSession session)
        {
            switch (message)
            {
                case "language":
                    session.State = State.LanguageChoose;
                    return "Choose language";
                case "help":
                    return "Help info";
                default:
                    return "Idle message";
            }
        }
    }
}