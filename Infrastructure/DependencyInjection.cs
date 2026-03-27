using Application.Common.Interfaces;
using Application.Interfaces;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
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
        /// Attempts to locate and load API/.env for local development.
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
        /// Resolves database connection string from environment variables first,
        /// then falls back to configuration key for backward compatibility.
        /// </summary>
        /// <param name="configuration">Application configuration root.</param>
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

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            TryLoadApiDotEnv();

            // Database
            var connectionString = GetDatabaseConnectionString(configuration);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string is missing. Set DB_CONNECTION_STRING in API/.env or environment variables.");
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Common Services
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

            return services;
        }
    }
}

