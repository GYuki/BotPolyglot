using System.Linq;
using LogicBlock.Session;

namespace Receiver.API.States
{
    public class LanguageChooseLogic : BaseLogic, ILanguageLogic
    {
        private readonly string[] _languageList = new string[] { "en" };
        public override string Act(string message, LogicBlock.Session.ChatSession session)
        {
            if (_languageList.Contains(message))
            {
                session.Language = message;
                session.State = State.ModeChoose;
                return "Choose mode";
            }
            else
            {
                return "Language list";
            }
        }

        public override string Back(ChatSession session)
        {
            session.State = State.Idle;
            return "Idle message";
        }
    }
}