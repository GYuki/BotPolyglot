using System.Threading.Tasks;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class IdleLogic : BaseLogic, IIdleLogic
    {
        public IdleLogic(ITranslationsRepository translation)
            :base(translation)
        {
        }
        public async override Task<ResponseModel> Act(string message, ChatSession session)
        {
            var result = new ResponseModel();

            switch (message)
            {
                case "/language":
                    session.State = State.LanguageChoose;
                    result.Message = (await _translation.GetText("choose_language")).Russian;
                    break;
                case "/help":
                    result.Message = (await _translation.GetText("help_info")).Russian;
                    break;
                default:
                    result.Message = (await _translation.GetText("idle_message")).Russian;
                    break;
            }
            result.Session = session;
            return result;
        }
    }
}