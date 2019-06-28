using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.ModelConfiguration
{
    class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {

        }
    }
}
