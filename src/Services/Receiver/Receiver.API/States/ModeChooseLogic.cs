using System.Linq;
using LogicBlock.Session;

namespace Receiver.API.States
{
    public class ModeChooseLogic : BaseLogic
    {
        private readonly string[] _modeList = new string[] { "arcade", "tutorial" };
        public override string Act(string message, LogicBlock.Session.ChatSession session)
        {
            switch(message)
            {
                case "arcade":
                    session.State = State.ActionArcade;
                    return "Start arcade";
                case "tutorial":
                    session.State = State.ActionTutorial;
                    return "Start tutorial";
                case "back":
                    session.State = State.LanguageChoose;
                    return "Choose language";
                default:
                    return "Choose mode";
            };
        }

        public override string Back(ChatSession session)
        {
            session.State = State.LanguageChoose;
            return "Language list";
        }
    }
}