using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class CategoriesConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.ParentId).HasColumnName("parent_id");

            builder.Property(e => e.Picture)
                .HasColumnName("picture")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("Categories_fk0");
        }
    }
}
