using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class LanguageTranslationsConfiguration : IEntityTypeConfiguration<LanguageTranslations>
    {
        public void Configure(EntityTypeBuilder<LanguageTranslations> builder)
        {
            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.NativeLangId).HasColumnName("native_lang_id");

            builder.Property(e => e.Translation)
                .IsRequired()
                .HasColumnName("translation")
                .HasMaxLength(100);

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.LanguageTranslationsLang)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LanguageTranslations_fk0");

            builder.HasOne(d => d.NativeLang)
                .WithMany(p => p.LanguageTranslationsNativeLang)
                .HasForeignKey(d => d.NativeLangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LanguageTranslations_fk1");
        }
    }
}
