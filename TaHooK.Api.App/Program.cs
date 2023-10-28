using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TaHooK.Api.BL.Installers;
using TaHooK.Api.Common.Tests;
using TaHooK.Api.Common.Tests.Installers;
using TaHooK.Common.Extensions;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Extensions;
using TaHooK.Api.DAL.Installers;
using TaHooK.Api.DAL.Migrators;

//using TaHooK.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ValidateAutoMapperConfiguration(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Migrate database
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IDbMigrator>().Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    if (Environment.GetEnvironmentVariable("E2E_TESTING").IsNullOrEmpty() || Environment.GetEnvironmentVariable("E2E_TESTING") == "false")
    {
        var connectionString = configuration.GetConnectionString("SQLCONNSTR_DefaultConnection") ?? throw new ArgumentException("The connection string is missing");
        serviceCollection.AddInstaller<DALInstaller>(connectionString);
    }
    else
    {
        serviceCollection.AddInstaller<TestDALInstaller>();
    }

    serviceCollection.AddInstaller<BlInstaller>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection)
{
    serviceCollection.AddAutoMapper(typeof(IEntity), typeof(BlInstaller));
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}


// Make the implicit Program class public so test projects can access it
public partial class Program
{
}
