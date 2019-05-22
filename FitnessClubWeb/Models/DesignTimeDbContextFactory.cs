using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FitnessClubWeb.Models
{
    /// <summary>
    /// класс для миграции БД. Возвращает сконфигурированный DBContext 
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitnessClubContext>
    {
        public FitnessClubContext CreateDbContext(string[] args) 
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<FitnessClubContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new FitnessClubContext(builder.Options);
        }
    }
}
