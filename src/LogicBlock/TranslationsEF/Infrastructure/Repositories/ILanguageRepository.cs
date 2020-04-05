using System.Collections.Generic;
using System.Threading.Tasks;
using LogicBlock.Translations.Model;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public interface ILanguageRepository<T>
        where T : AbstractLanguage
    {
        Task<List<T>> GetWordTranslationsAsync(int wordId);
    }
}