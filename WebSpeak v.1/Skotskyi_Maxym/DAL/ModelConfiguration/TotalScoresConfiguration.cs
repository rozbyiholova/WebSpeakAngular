using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class TotalScoresConfiguration : IEntityTypeConfiguration<TotalScores>
    {
        public void Configure(EntityTypeBuilder<TotalScores> builder)
        {
            builder.Property(e => e.LangId).HasColumnName("lang_id");

            builder.Property(e => e.Total).HasColumnName("total");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.Lang)
                .WithMany(p => p.TotalScores)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TotalScores_fk1");

            builder.HasOne(d => d.User)
                .WithMany(p => p.TotalScores)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TotalScores_fk0");
        }
    }
}
