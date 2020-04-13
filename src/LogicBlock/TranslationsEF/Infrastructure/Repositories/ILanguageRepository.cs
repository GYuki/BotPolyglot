using System.Collections.Generic;
using System.Threading.Tasks;
using LogicBlock.Translations.Model;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public interface ILanguageRepository
    {
        Task<int> GetWordsCountAsync();
        Task<List<ILanguage>> GetWordTranslationsAsync(int wordId);
    }
}