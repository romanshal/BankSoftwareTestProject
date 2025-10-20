using Microsoft.Extensions.Logging;

namespace BankSoftware.Infrastructure.Data.Contexts
{
    public class DataSeedMaker
    {
        public static async Task SeedAsync(BankDbContext context, ILogger<DataSeedMaker> logger)
        {
            logger.LogInformation($"Seed database associated with context {nameof(BankDbContext)}");
        }
    }
}
