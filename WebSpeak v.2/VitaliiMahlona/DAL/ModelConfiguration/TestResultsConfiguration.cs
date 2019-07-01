using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class TestResultsConfiguration : IEntityTypeConfiguration<TestResults>
    {
        public void Configure(EntityTypeBuilder<TestResults> builder)
        {
            builder.Property(e => e.CategoryId).HasColumnName("category_id");

            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.Result).HasColumnName("result");

            builder.Property(e => e.TestDate)
                .HasColumnName("testDate")
                .HasColumnType("datetime");

            builder.Property(e => e.TestId).HasColumnName("test_id");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.Category)
                .WithMany(p => p.TestResults)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestResults_fk3");

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.TestResults)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestResults_fk2");

            builder.HasOne(d => d.Test)
                .WithMany(p => p.TestResults)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestResults_fk0");

            builder.HasOne(d => d.User)
                .WithMany(p => p.TestResults)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestResults_fk1");
        }
    }
}
