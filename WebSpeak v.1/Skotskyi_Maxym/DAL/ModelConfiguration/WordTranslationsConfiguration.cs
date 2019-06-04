using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class WordTranslationsConfiguration : IEntityTypeConfiguration<WordTranslations>
    {
        public void Configure(EntityTypeBuilder<WordTranslations> builder)
        {
            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.Pronounce)
                .IsRequired()
                .HasColumnName("pronounce")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Translation)
                .IsRequired()
                .HasColumnName("translation")
                .HasMaxLength(100);

            builder.Property(e => e.WordId).HasColumnName("word_id");

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.WordTranslations)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WordTranslations_fk1");

            builder.HasOne(d => d.Word)
                .WithMany(p => p.WordTranslations)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WordTranslations_fk0");
        }
    }
}
