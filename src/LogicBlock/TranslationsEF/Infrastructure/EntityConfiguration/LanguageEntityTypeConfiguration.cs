using System.Collections.Generic;
using LogicBlock.Translations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogicBlock.Translations.Infrastructure.EntityConfiguration
{
    class LanguageEntityTypeConfiguration<T>
        : IEntityTypeConfiguration<T>
        where T: AbstractLanguage
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .ToTable(typeof(T).Name);

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
                .Property(l => l.WordId)
                .IsRequired();

            builder
                .HasIndex(l => l.WordId)
                .IsUnique(false);
        }
    }
}