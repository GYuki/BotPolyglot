using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Session;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class ModeChooseLogic : BaseLogic, IModeChooseLogic
    {
        private readonly string[] _modeList = new string[] { "arcade", "tutorial" };
        public override Task<ResponseModel> Act(string message, LogicBlock.Session.ChatSession session)
        {
            var result = new ResponseModel();
            switch(message)
            {
                case "arcade":
                    session.State = State.ArcadeAction;
                    result.Message = "Start arcade";
                    break;
                case "tutorial":
                    session.State = State.TutorialAction;
                    result.Message = "Start tutorial";
                    break;
                default:
                    result.Message = "Choose mode";
                    break;
            };

            result.Session = session;
            return Task.FromResult(result);
        }

        public override string Back(ChatSession session)
        {
            session.State = State.LanguageChoose;
            session.Language = null;
            return "Language list";
        }
    }
}