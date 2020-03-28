using System.Threading.Tasks;
using LogicBlock.Translations.Model;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public interface ILanguageRepository
    {
        Task<string[]> GetWordTranslationsAsync(int wordId);
    }
}