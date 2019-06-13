using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.ModelConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL
{
    public partial class LearningLanguagesContext : IdentityDbContext<Users>
    {
        public LearningLanguagesContext()
        {
        }

        public LearningLanguagesContext(DbContextOptions<LearningLanguagesContext> options)
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
        public virtual DbSet<WordTranslations> WordTranslations { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigurateOptions.GetOptions();
                optionsBuilder.UseSqlServer(ConfigurateOptions.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
