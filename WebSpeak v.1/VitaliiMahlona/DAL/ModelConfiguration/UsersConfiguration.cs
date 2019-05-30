using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;

namespace DAL.ModelConfiguration
{
    class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .ValueGeneratedNever();
        }
    }
}
