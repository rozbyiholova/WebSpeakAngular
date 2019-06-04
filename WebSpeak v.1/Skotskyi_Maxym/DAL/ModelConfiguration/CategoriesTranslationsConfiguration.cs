using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class CategoriesTranslationsConfiguration : IEntityTypeConfiguration<CategoriesTranslations>
    {
        public void Configure(EntityTypeBuilder<CategoriesTranslations> builder)
        {
            builder.Property(e => e.CategoryId).HasColumnName("category_id");

            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.Translation)
                .IsRequired()
                .HasColumnName("translation")
                .HasMaxLength(100);

            builder.HasOne(d => d.Category)
                .WithMany(p => p.CategoriesTranslations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CategoriesTranslations_fk0");

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.CategoriesTranslations)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CategoriesTranslations_fk1");
        }
    }
}
