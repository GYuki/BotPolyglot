using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogicBlock.Translations.Infrastructure.EntityConfiguration
{
    class LanguageEntityTypeConfiguration<T>
        : IEntityTypeConfiguration<AbstractLanguage>
        where T: AbstractLanguage
    {
        public void Configure(EntityTypeBuilder<AbstractLanguage> builder)
        {
            builder
                .ToTable(typeof(T).ToString());

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
}