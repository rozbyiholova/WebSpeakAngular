using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class WordsConfiguration : IEntityTypeConfiguration<Words>
    {
        public void Configure(EntityTypeBuilder<Words> builder)
        {
            builder.Property(e => e.CategoryId).HasColumnName("category_id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Picture)
                .HasColumnName("picture")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Sound)
                .HasColumnName("sound")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.HasOne(d => d.Category)
                .WithMany(p => p.Words)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Words_fk0");
        }
    }
}
