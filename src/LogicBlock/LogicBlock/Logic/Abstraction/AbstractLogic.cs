using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;
using LogicBlock.Utils.Extensions;

namespace LogicBlock.Logic
{
    public abstract class AbstractLogic : IArcadeLogic
    {
        protected readonly ILanguageRepository<AbstractLanguage> _repository;
        
        public AbstractLogic(ILanguageRepository<AbstractLanguage> repository)
        {
            _repository = repository;
        }

        public abstract Task<IResponseInfo> StartChat(IStartRequestInfo info);
        public abstract Task<IResponseInfo> HandleText(ITextRequestInfo info);

        protected async Task GenerateSequenceAsync(IRequestInfo info)
        {
            int wordsCount = await _repository.GetWordsCountAsync();
            var wordSequence = Enumerable.Range(0, wordsCount - 1).ToList();
            wordSequence.Shuffle();
            
            info.OperationRequest.Session.ExpectedWord = 0;
            info.OperationRequest.Session.WordSequence = wordSequence;
        }
    }
}