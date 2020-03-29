using System;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure;
using LogicBlock.Translations.Infrastructure.Repositories;

namespace LogicBlock.Logic
{
    public class ArcadeLogic : IArcadeLogic
    {
        private readonly ILanguageRepository _repository;

        public ArcadeLogic(ILanguageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            int wordId = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord];

            var translations = await _repository.GetWordTranslationsAsync(wordId);

            if (translations == null || translations.Length == 0)
            {
                return new ArcadeResponseInfo("Перевода нет.");
            }
            
            foreach (var t in translations)
            {
                if (t.Translation == info.Request.MessageText)
                {
                    info.Request.Session.ExpectedWord = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord + 1];
                    return new ArcadeResponseInfo("Верно!", t.Word.Award);
                }
            }

            return new ArcadeResponseInfo("Ответ неверный");            
        }
    }
}