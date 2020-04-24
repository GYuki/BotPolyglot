using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public class LanguageRepository<T>
        : ILanguageRepository
        where T : class, ILanguage
    {
        private readonly TranslationContext _context;

        public LanguageRepository(TranslationContext context)
        {
            _context = context;
        }

        public async Task<int> GetWordsCountAsync()
        {
            return await _context.Words.CountAsync();
        }

        public async Task<List<ILanguage>> GetWordTranslationsAsync(int wordId)
        {
            if (wordId <= 0)
                return null;
            
            var translations = await _context
                                            .Set<T>()
                                            .Where(l => l.WordId == wordId)
                                            .Include(l => l.Word)
                                            .ToListAsync() as List<ILanguage>;

            return translations;                    
        }

        public async Task<string> GetNextTask(int wordId)
        {
            if (wordId <= 0)
                return null;
            
            var translation = await _context
                                            .Set<T>()
                                            .FirstOrDefaultAsync(l => l.WordId == wordId);
            
            if (translation == null)
                return null;
            
            return translation.Translation;
        }
    }

    public class RuLanguageRepository : LanguageRepository<RuLanguage>
    {
        public RuLanguageRepository(TranslationContext context)
            : base(context)
        {}
    }

    public class EnLanguageRepository : LanguageRepository<EnLanguage>
    {
        public EnLanguageRepository(TranslationContext context)
            : base(context)
        {}
    }
}