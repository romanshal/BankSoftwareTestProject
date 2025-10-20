using BankSoftware.Application.Interfaces.Cache;
using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Infrastructure.Cache;
using BankSoftware.Infrastructure.Data.Contexts;
using BankSoftware.Infrastructure.Repositories;
using BankSoftware.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BankSoftware.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSettings>(configuration.GetSection("Redis"));

            var connectionString = configuration.GetConnectionString("BankDbConnection") ?? throw new InvalidOperationException("Connection string 'BankDbConnection' not found");
            var redisSettings = configuration.GetRequiredSection("Redis").Get<RedisSettings>() ?? throw new InvalidOperationException("Connection settings for Redis not found");

            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                var connection = $"{redisSettings.Host}:{redisSettings.Port},password={redisSettings.Password}";
                options.Configuration = connection;
            });

            var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { $"{redisSettings.Host}:{redisSettings.Port}" },
                AbortOnConnectFail = false,
                Ssl = false,
                Password = redisSettings.Password,
                TieBreaker = "",
                AllowAdmin = true,
                CommandMap = CommandMap.Create(
                [
                    "INFO", "CONFIG", "CLUSTER", "PING", "ECHO", "CLIENT"
                ], available: false)
            });
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

            services
                .AddScoped<ICacheService, RedisCacheService>()
                .AddSingleton<IKeyFactory, RedisKeyFactory>();

            services.AddTransient<ILoanRepository, LoanRepository>();

            services
                .AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddRedis($"{redisSettings.Host}:{redisSettings.Port}");

            return services;
        }
    }
}
