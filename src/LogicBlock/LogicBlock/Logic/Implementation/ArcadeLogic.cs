using System;
using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Translations.Infrastructure;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;
using LogicBlock.Utils;
using LogicBlock.Utils.Extensions;

namespace LogicBlock.Logic
{
    public class ArcadeLogic : AbstractLogic, IArcadeLogic
    {
        private readonly Random _random = new Random();

        public ArcadeLogic(ILanguageRepository repository, ITranslationsRepository translations)
            : base(repository, translations)
        {

        }

        public override async Task<IResponseInfo> StartChat(IStartRequestInfo info)
        {
            await GenerateSequenceAsync(info);
            var message = await _translations.GetText("text_start");
            return new StartResponseInfo(ResponseCodes.OK, message.Russian);
        }

        public override async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            if (info.Request.Session.WordSequence == null || info.Request.Session.ExpectedWord >= info.Request.Session.WordSequence.Count)
                return new ArcadeResponseInfo("Internal error", ResponseCodes.LogicInternalError);

            int wordId = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord];

            var translations = await _repository.GetWordTranslationsAsync(wordId);

            if (translations == null || translations.Count == 0)
            {
                var noTranslations = await _translations.GetText("text_noTranslations");
                return new ArcadeResponseInfo(noTranslations.Russian, ResponseCodes.NoAsnwers);
            }
            
            foreach (var t in translations)
            {
                if (t.Translation == info.Request.MessageText)
                {
                    if (info.Request.Session.ExpectedWord == info.Request.Session.WordSequence.Count - 1)
                    {
                        await GenerateSequenceAsync(info);
                    }
                    else
                        info.Request.Session.ExpectedWord++;

                    var success = await _translations.GetText("text_success");
                    info.Request.Session.Award += t.Word.Award;
                    return new ArcadeResponseInfo(success.Russian, ResponseCodes.OK, t.Word.Award);
                }
            }

            if (info.Request.Session.ExpectedWord == info.Request.Session.WordSequence.Count - 1)
            {
                info.Request.Session.ExpectedWord = 0;
                info.Request.Session.WordSequence = null;
            }
            else
                info.Request.Session.ExpectedWord++;
            
            var wrongAnswer = await _translations.GetText("text_wrongAnswer");
            info.Request.Session.Award = 0;

            return new ArcadeResponseInfo(
                string.Format(wrongAnswer.Russian, string.Join(", ", translations.Select(x => x.Translation))),
                ResponseCodes.WrongAnswer);            
        }
    }
}