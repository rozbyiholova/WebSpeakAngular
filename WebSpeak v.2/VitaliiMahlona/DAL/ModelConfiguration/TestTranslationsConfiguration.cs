using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class TestTranslationsConfiguration : IEntityTypeConfiguration<TestTranslations>
    {
        public void Configure(EntityTypeBuilder<TestTranslations> builder)
        {
            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.TestId).HasColumnName("test_id");

            builder.Property(e => e.Translation)
                .IsRequired()
                .HasColumnName("translation")
                .HasMaxLength(100);

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.TestTranslations)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestTranslations_fk1");

            builder.HasOne(d => d.Test)
                .WithMany(p => p.TestTranslations)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestTranslations_fk0");
        }
    }
}
