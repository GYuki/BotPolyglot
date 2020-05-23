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
        public override Task<ResponseModel> Act(string message, ChatSession session)
        {
            var result = new ResponseModel();

            switch (message)
            {
                case "language":
                    session.State = State.LanguageChoose;
                    result.Message = "Choose language";
                    break;
                case "help":
                    result.Message = "Help info";
                    break;
                default:
                    result.Message = "Idle message";
                    break;
            }
            result.Session = session;
            return Task.FromResult(result);
        }
    }
}