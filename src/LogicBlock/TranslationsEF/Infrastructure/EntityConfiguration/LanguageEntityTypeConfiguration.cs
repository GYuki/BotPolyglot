using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogicBlock.Translations.Infrastructure.EntityConfiguration
{
    abstract class LanguageEntityTypeConfiguration
        : IEntityTypeConfiguration<AbstractLanguage>
    {
        protected string _tableName;

        public void Configure(EntityTypeBuilder<AbstractLanguage> builder)
        {
            builder
                .ToTable(_tableName);

            builder
                .HasKey(l => l.Id);
            builder
                .Property(l => l.Id)
                .IsRequired();

            builder
                .Property(l => l.Translation)
                .IsRequired()
                .HasMaxLength(255);
            
            builder
                .HasOne(l => l.Word)
                .WithMany(w => w.Translations)
                .HasForeignKey(l => l.WordId)
                .IsRequired();
        }
    }

    class RuLanguage : LanguageEntityTypeConfiguration
    {
        public RuLanguage()
        {
            _tableName = "ru_language";
        }
    }

    class EnLanguage : LanguageEntityTypeConfiguration
    {
        public EnLanguage()
        {
            _tableName = "en_language";
        }
    }
}