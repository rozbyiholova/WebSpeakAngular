using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class TestsConfiguration : IEntityTypeConfiguration<Tests>
    {
        public void Configure(EntityTypeBuilder<Tests> builder)
        {
            builder.Property(e => e.Icon)
                .IsRequired()
                .HasColumnName("icon")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
