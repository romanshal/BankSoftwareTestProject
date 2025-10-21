using BankSoftware.API.Extensions;
using BankSoftware.API.Middlewares;
using BankSoftware.Application;
using BankSoftware.Infrastructure;
using BankSoftware.Infrastructure.Data.Contexts;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
        .WithOrigins("http://localhost:8080", "http://localhost:80")
        //.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BankSoftware.API", Version = "v1" });
    options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
});

builder.Services.AddControllers();

var app = builder.Build();

app.MigrateDatabase<BankDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<DataSeedMaker>>();
    DataSeedMaker
        .SeedAsync(context, logger!)
        .Wait();
});

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Banksoftware.API v1");
        options.RoutePrefix = string.Empty;
    });
}
//else
//{
//    app.UseHttpsRedirection();
//}

app.UseCors("AllowSpecificOrigin");

app.UseExeptionWrappingMiddleware();

app.MapControllers();

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
