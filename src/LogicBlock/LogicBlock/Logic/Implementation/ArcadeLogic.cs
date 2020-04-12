using System;
using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;
using LogicBlock.Utils.Extensions;

namespace LogicBlock.Logic
{
    public class ArcadeLogic : IArcadeLogic
    {
        private readonly ILanguageRepository<AbstractLanguage> _repository;
        private readonly Random _random = new Random();

        public ArcadeLogic(ILanguageRepository<AbstractLanguage> repository)
        {
            _repository = repository;
        }

        public async Task<IResponseInfo> StartChat(IStartRequestInfo info)
        {
            await GenerateSequenceAsync(info);
            return new StartResponseInfo(true, "Success!");
        }

        public async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            int wordId = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord];

            var translations = await _repository.GetWordTranslationsAsync(wordId);

            if (translations == null || translations.Count == 0)
            {
                return new ArcadeResponseInfo("Перевода нет.", false);
            }
            
            foreach (var t in translations)
            {
                if (t.Translation == info.Request.MessageText)
                {
                    info.Request.Session.ExpectedWord = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord + 1];
                    return new ArcadeResponseInfo("Верно!", true, t.Word.Award);
                }
            }

            return new ArcadeResponseInfo("Ответ неверный", false);            
        }

        private async Task GenerateSequenceAsync(IRequestInfo info)
        {
            int wordsCount = await _repository.GetWordsCountAsync();
            var wordSequence = Enumerable.Range(0, wordsCount - 1).ToList();
            wordSequence.Shuffle();
            
            info.OperationRequest.Session.ExpectedWord = 0;
            info.OperationRequest.Session.WordSequence = wordSequence;
        }
    }
}