using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;

namespace LogicBlock.Logic
{
    public class TutorialLogic : AbstractLogic, ITutorialLogic
    {
        public TutorialLogic(ILanguageRepository repository, ITranslationsRepository translations)
            : base(repository, translations)
        {
            
        }
        public override async Task<IResponseInfo> StartChat(IStartRequestInfo info)
        {
            return null;
        }

        public override async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            return null;
        }
    }
}