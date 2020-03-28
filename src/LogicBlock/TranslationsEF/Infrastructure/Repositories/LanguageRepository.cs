using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public class LanguageRepository<T>
        : ILanguageRepository
        where T : AbstractLanguage
    {
        private readonly TranslationContext _context;

        public LanguageRepository(TranslationContext context)
        {
            _context = context;
        }

        public async Task<string[]> GetWordTranslationsAsync(int wordId)
        {
            if (wordId <= 0)
                return null;
            
            string[] translations = await _context
                                            .Set<T>()
                                            .Where(l => l.WordId == wordId)
                                            .Select(l => l.Translation)
                                            .ToArrayAsync();

            return translations;                    
        }
    }
}