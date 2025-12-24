using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Phrase_App.Infrastructure.Persistence
{
    public class PhraseDbContextFactory : IDesignTimeDbContextFactory<PhraseDbContext>
    {
        public PhraseDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<PhraseDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return the DbContext
            return new PhraseDbContext(optionsBuilder.Options);
        }
    }
}
