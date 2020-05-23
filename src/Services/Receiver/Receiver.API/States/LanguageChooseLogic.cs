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

        public override Task<ResponseModel> Act(string message, LogicBlock.Session.ChatSession session)
        {
            var result = new ResponseModel();
            if (_languageList.Contains(message))
            {
                session.Language = message;
                session.State = State.ModeChoose;
                result.Message = "Choose mode";
            }
            else
            {
                result.Message = "Language list";
            }

            result.Session = session;
            return Task.FromResult(result);
        }

        public override string Back(ChatSession session)
        {
            session.State = State.Idle;
            return "Idle message";
        }
    }
}