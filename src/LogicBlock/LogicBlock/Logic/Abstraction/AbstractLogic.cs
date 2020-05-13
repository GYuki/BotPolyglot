using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;
using LogicBlock.Utils;
using LogicBlock.Utils.Extensions;

namespace LogicBlock.Logic
{
    public abstract class AbstractLogic : ILogic
    {
        protected readonly ILanguageRepository _repository;
        protected readonly ITranslationsRepository _translations;
        
        public AbstractLogic(ILanguageRepository repository, ITranslationsRepository translations)
        {
            _repository = repository;
            _translations = translations;
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

        public async Task<IResponseInfo> AfterAction(IAfterActionRequestInfo info)
        {
            if(info.Request.Session.WordSequence == null || info.Request.Session.ExpectedWord >= info.Request.Session.WordSequence.Count)
                await GenerateSequenceAsync(info);

            var next = await _repository.GetNextTask(info.Request.Session.WordSequence[info.Request.Session.ExpectedWord]);
            
            var text = await _translations.GetText("text_afterAction");

            if (text == null)
                return new AfterActionResponseInfo(ResponseCodes.LogicInternalError, "No Translation for text_afterAction");

            var message = string.Format(text.Russian, next);
            return new AfterActionResponseInfo(next != null ? ResponseCodes.OK : ResponseCodes.LogicInternalError , message);
        }
    }
}