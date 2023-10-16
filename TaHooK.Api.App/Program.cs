using Serilog;
using TaHooK.Common.Extensions;
using TaHooK.Api.DAL.Entities.Interfaces;
//using TaHooK.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("The connection string is missing");
    serviceCollection.AddInstaller<ApiDALInstaller>(connectionString);
    serviceCollection.AddInstaller<ApiBLInstaller>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection)
{
    serviceCollection.AddAutoMapper(typeof(IEntity), typeof(ApiBLInstaller));
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ValidateAutoMapperConfiguration(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();