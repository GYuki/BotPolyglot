using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public class LanguageRepository<T>
        : ILanguageRepository<T>
        where T : AbstractLanguage
    {
        private readonly TranslationContext _context;

        public LanguageRepository(TranslationContext context)
        {
            _context = context;
        }

        public Task<int> GetWordsCountAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<T>> GetWordTranslationsAsync(int wordId)
        {
            if (wordId <= 0)
                return null;
            
            var translations = await _context
                                            .Set<T>()
                                            .Where(l => l.WordId == wordId)
                                            .Include(l => l.Word)
                                            .ToListAsync();

            return translations;                    
        }
    }
}