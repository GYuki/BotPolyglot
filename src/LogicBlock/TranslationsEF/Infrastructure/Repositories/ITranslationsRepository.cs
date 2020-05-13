using System.Threading.Tasks;
using LogicBlock.Translations.Model.Texts;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public interface ITranslationsRepository
    {
        Task<Text> GetText(string key);
    }
}