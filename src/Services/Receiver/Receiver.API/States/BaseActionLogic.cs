using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;

namespace Receiver.API.States
{
    public abstract class BaseActionLogic : BaseLogic, IActionLogic
    {
        protected readonly LogicBlock.Logic.ILogic _logic;
        public BaseActionLogic(LogicBlock.Logic.ILogic logic, ITranslationsRepository translation)
            : base(translation)
        {
            _logic = logic;
        }
        public async Task<string> GetNextTask(ChatSession session)
        {
            var info = new AfterActionRequestInfo
            {
                Request = new AfterActionRequest(session)
            };

            var result = await _logic.AfterAction(info);

            return result.Message;
        }
    }
}