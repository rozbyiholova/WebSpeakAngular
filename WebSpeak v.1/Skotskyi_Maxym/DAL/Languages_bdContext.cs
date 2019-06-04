using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DAL.ModelConfiguration;

namespace DAL.Models
{
    public partial class Languages_bdContext : DbContext
    {
        public Languages_bdContext()
        {
        }

        public Languages_bdContext(DbContextOptions<Languages_bdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoriesTranslations> CategoriesTranslations { get; set; }
        public virtual DbSet<LanguageTranslations> LanguageTranslations { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<TestResults> TestResults { get; set; }
        public virtual DbSet<TestTranslations> TestTranslations { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }
        public virtual DbSet<TotalScores> TotalScores { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WordTranslations> WordTranslations { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=VINW0231;Database=Languages_bd;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());

            modelBuilder.ApplyConfiguration(new CategoriesTranslationsConfiguration());

            modelBuilder.ApplyConfiguration(new LanguageTranslationsConfiguration());

            modelBuilder.ApplyConfiguration(new LanguagesConfiguration());

            modelBuilder.ApplyConfiguration(new TestResultsConfiguration());

            modelBuilder.ApplyConfiguration(new TestTranslationsConfiguration());

            modelBuilder.ApplyConfiguration(new TestsConfiguration());

            modelBuilder.ApplyConfiguration(new TotalScoresConfiguration());

            modelBuilder.ApplyConfiguration(new UsersConfiguration());

            modelBuilder.ApplyConfiguration(new WordTranslationsConfiguration());

            modelBuilder.ApplyConfiguration(new WordsConfiguration());
        }
    }
}
