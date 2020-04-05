using System;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;

namespace LogicBlock.Logic
{
    public class ArcadeLogic : IArcadeLogic
    {
        private readonly ILanguageRepository<AbstractLanguage> _repository;

        public ArcadeLogic(ILanguageRepository<AbstractLanguage> repository)
        {
            _repository = repository;
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
    }
}