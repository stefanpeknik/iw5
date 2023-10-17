using AutoMapper;
using Serilog;
using TaHooK.Api.BL.Installers;
using TaHooK.Common.Extensions;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Extensions;
using TaHooK.Api.DAL.Installers;
//using TaHooK.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add services to the container.
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);
ValidateAutoMapperConfiguration(app.Services);

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("The connection string is missing");
    serviceCollection.AddInstaller<DALInstaller>(connectionString);
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


// Migrate database
using var scope = app.Services.CreateScope();
if (app.Environment.IsDevelopment())
{
    scope.ServiceProvider.GetRequiredService<IDbMigrator>().Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();