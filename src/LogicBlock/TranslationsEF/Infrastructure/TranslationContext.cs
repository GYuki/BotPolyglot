using LogicBlock.Translations.Infrastructure.EntityConfiguration;
using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;

namespace LogicBlock.Translations.Infrastructure
{
    public class TranslationContext : DbContext
    {
        public TranslationContext(DbContextOptions<TranslationContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new LanguageEntityTypeConfiguration<RuLanguage>());
            builder.ApplyConfiguration(new LanguageEntityTypeConfiguration<EnLanguage>());
        }
    }
}