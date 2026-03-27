using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Loads environment variables from a .env file.
        /// </summary>
        /// <param name="filePath">Absolute .env file path.</param>
        private static void LoadEnvFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = line[..separatorIndex].Trim();
                var value = line[(separatorIndex + 1)..].Trim();
                Environment.SetEnvironmentVariable(key, value);
            }
        }

        /// <summary>
        /// Attempts to locate and load API/.env for design-time commands.
        /// </summary>
        private static void TryLoadApiDotEnv()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var candidates = new[]
            {
                Path.Combine(currentDirectory, ".env"),
                Path.Combine(currentDirectory, "API", ".env"),
                Path.Combine(currentDirectory, "..", "API", ".env"),
                Path.Combine(currentDirectory, "..", "..", "API", ".env")
            };

            foreach (var candidate in candidates.Select(Path.GetFullPath).Distinct())
            {
                if (File.Exists(candidate))
                {
                    LoadEnvFile(candidate);
                    return;
                }
            }
        }

        /// <summary>
        /// Resolves database connection string for EF design-time operations.
        /// Environment variable has highest priority.
        /// </summary>
        /// <param name="configuration">Configuration root.</param>
        /// <returns>Resolved connection string or null when missing.</returns>
        private static string? GetDatabaseConnectionString(IConfiguration configuration)
        {
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConnectionString))
            {
                return envConnectionString;
            }

            return configuration.GetConnectionString("DefaultConnection");
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            TryLoadApiDotEnv();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = GetDatabaseConnectionString(configuration);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string is missing. Set DB_CONNECTION_STRING in API/.env or environment variables.");
            }

            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

