using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogicBlock.Translations.Infrastructure.EntityConfiguration
{
    class WordEntityTypeConfiguration
        : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("word");

            builder
                .HasKey(w => w.Id);
            
            builder
                .Property(w => w.Id)
                .IsRequired();
        }
    }
}