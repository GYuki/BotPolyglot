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

        public ArcadeLogic(ILanguageRepository repository)
            : base(repository)
        {

        }

        public override async Task<IResponseInfo> StartChat(IStartRequestInfo info)
        {
            await GenerateSequenceAsync(info);
            return new StartResponseInfo(ResponseCodes.OK, "Success!");
        }

        public override async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            if (info.Request.Session.WordSequence == null || info.Request.Session.ExpectedWord >= info.Request.Session.WordSequence.Count)
                return new ArcadeResponseInfo("Internal error", ResponseCodes.LogicInternalError);
            int wordId = info.Request.Session.WordSequence[info.Request.Session.ExpectedWord];

            var translations = await _repository.GetWordTranslationsAsync(wordId);

            if (translations == null || translations.Count == 0)
            {
                return new ArcadeResponseInfo("Перевода нет.", ResponseCodes.NoAsnwers);
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
                    return new ArcadeResponseInfo("Верно!", ResponseCodes.OK, t.Word.Award);
                }
            }

            return new ArcadeResponseInfo("Ответ неверный", ResponseCodes.WrongAnswer);            
        }
    }
}