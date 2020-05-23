using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class ModeChooseLogic : BaseLogic, IModeChooseLogic
    {
        private readonly string[] _modeList = new string[] { "arcade", "tutorial" };

        public ModeChooseLogic(ITranslationsRepository translation)
            :base(translation)
        {
            _translation = translation;
        }

        public async override Task<ResponseModel> Act(string message, LogicBlock.Session.ChatSession session)
        {
            var result = new ResponseModel();
            switch(message)
            {
                case "arcade":
                    session.State = State.ArcadeAction;
                    result.Message = (await _translation.GetText("start_arcade")).Russian;
                    break;
                case "tutorial":
                    session.State = State.TutorialAction;
                    result.Message = (await _translation.GetText("start_tutorial")).Russian;
                    break;
                default:
                    result.Message = (await _translation.GetText("choose_mode")).Russian;
                    break;
            };

            result.Session = session;
            return result;
        }

        public async override Task<string> Back(ChatSession session)
        {
            session.State = State.LanguageChoose;
            session.Language = null;
            return (await _translation.GetText("language_list")).Russian;
        }
    }
}