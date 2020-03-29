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

        public async Task<AbstractLanguage[]> GetWordTranslationsAsync(int wordId)
        {
            if (wordId <= 0)
                return null;
            
            AbstractLanguage[] translations = await _context
                                            .Set<T>()
                                            .Where(l => l.WordId == wordId)
                                            .Include(l => l.Word)
                                            .ToArrayAsync();

            return translations;                    
        }
    }
}