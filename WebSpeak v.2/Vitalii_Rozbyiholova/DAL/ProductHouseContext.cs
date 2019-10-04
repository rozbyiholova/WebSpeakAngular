using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class ProductHouseContext : DbContext
    {
        public ProductHouseContext()
        {
        }

        public ProductHouseContext(DbContextOptions<ProductHouseContext> options)
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
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WordTranslations> WordTranslations { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:vitaliiserver.database.windows.net,1433;Initial Catalog=ProductHouse;Persist Security Info=False;User ID=rozbiygolova;Password=Vetal12314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasIndex(e => e.ParentId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("Categories_fk0");
            });

            modelBuilder.Entity<CategoriesTranslations>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.LangId);

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasColumnName("translation")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoriesTranslations)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CategoriesTranslations_fk0");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.CategoriesTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CategoriesTranslations_fk1");
            });

            modelBuilder.Entity<LanguageTranslations>(entity =>
            {
                entity.HasIndex(e => e.LangId);

                entity.HasIndex(e => e.NativeLangId);

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.NativeLangId).HasColumnName("native_lang_id");

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasColumnName("translation")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.LanguageTranslationsLang)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LanguageTranslations_fk0");

                entity.HasOne(d => d.NativeLang)
                    .WithMany(p => p.LanguageTranslationsNativeLang)
                    .HasForeignKey(d => d.NativeLangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LanguageTranslations_fk1");
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TestResults>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.LangId);

                entity.HasIndex(e => e.TestId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.TestDate)
                    .HasColumnName("testDate")
                    .HasColumnType("date");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestResults_fk3");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestResults_fk2");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestResults_fk0");
            });

            modelBuilder.Entity<TestTranslations>(entity =>
            {
                entity.HasIndex(e => e.LangId);

                entity.HasIndex(e => e.TestId);

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasColumnName("translation")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.TestTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestTranslations_fk1");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestTranslations)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestTranslations_fk0");
            });

            modelBuilder.Entity<Tests>(entity =>
            {
                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnName("icon")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TotalScores>(entity =>
            {
                entity.HasIndex(e => e.LangId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.TotalScores)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TotalScores_fk1");
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSetti__UserI__6B24EA82");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<WordTranslations>(entity =>
            {
                entity.HasIndex(e => e.LangId);

                entity.HasIndex(e => e.WordId);

                entity.Property(e => e.LangId).HasColumnName("lang_id");

                entity.Property(e => e.Pronounce)
                    .IsRequired()
                    .HasColumnName("pronounce")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasColumnName("translation")
                    .HasMaxLength(100);

                entity.Property(e => e.WordId).HasColumnName("word_id");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.WordTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WordTranslations_fk1");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.WordTranslations)
                    .HasForeignKey(d => d.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WordTranslations_fk0");
            });

            modelBuilder.Entity<Words>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sound)
                    .HasColumnName("sound")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Words)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Words_fk0");
            });
        }
    }
}
