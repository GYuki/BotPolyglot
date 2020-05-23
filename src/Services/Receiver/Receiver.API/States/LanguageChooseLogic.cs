using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class LanguageChooseLogic : BaseLogic, ILanguageLogic
    {
        private readonly string[] _languageList = new string[] { "en" };

        public LanguageChooseLogic(ITranslationsRepository translation)
            :base(translation)
        {
            
        }

        public async override Task<ResponseModel> Act(string message, LogicBlock.Session.ChatSession session)
        {
            var result = new ResponseModel();
            if (_languageList.Contains(message))
            {
                session.Language = message;
                session.State = State.ModeChoose;
                result.Message = (await _translation.GetText("choose_mode")).Russian;
            }
            else
            {
                result.Message = (await _translation.GetText("language_list")).Russian;
            }

            result.Session = session;
            return result;
        }

        public async override Task<string> Back(ChatSession session)
        {
            session.State = State.Idle;
            return (await _translation.GetText("idle_message")).Russian;
        }
    }
}