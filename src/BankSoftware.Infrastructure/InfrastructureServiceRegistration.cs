using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Infrastructure.Data.Contexts;
using BankSoftware.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankSoftware.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BankDbConnection") ?? throw new InvalidOperationException("Connection string 'BankDbConnection' not found");

            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });


            services.AddTransient<ILoanRepository, LoanRepository>();

            services
                .AddHealthChecks()
                .AddNpgSql(connectionString);

            return services;
        }
    }
}
