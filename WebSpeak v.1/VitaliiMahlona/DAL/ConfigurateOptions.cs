using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL
{
    public static class ConfigurateOptions
    {
        private static string _connectionString = string.Empty;
        public static DbContextOptions<LearningLanguagesContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            _connectionString = config.GetConnectionString("LearningLanguages");

            var optionsBuilder = new DbContextOptionsBuilder<LearningLanguagesContext>();
            var options = optionsBuilder
                .UseSqlServer(_connectionString)
                .Options;

            return options;
        }
        public static string ConnectionString
        {
            get => _connectionString;
        }
    }
}
