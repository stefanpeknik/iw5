using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TaHooK.Api.App.Hubs;
using TaHooK.Api.BL.Installers;
using TaHooK.Api.Common.Tests.Installers;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Extensions;
using TaHooK.Api.DAL.Installers;
using TaHooK.Api.DAL.Migrators;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Responses;

//using TaHooK.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder.Services);

// Add services to the container.
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);
ConfigureAuthentication(builder.Services, builder.Configuration.GetValue<string>("IdentityServerUrl")!);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSignalR();
ConfigureControllers(builder.Services);
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument(document => document.Title = "TaHooK API");

var app = builder.Build();

ValidateAutoMapperConfiguration(app.Services);

app.UseCors();
app.UseOpenApi();
app.UseSwaggerUi3();


// Migrate database
using var scope = app.Services.CreateScope();
if (app.Environment.IsDevelopment())
{
    scope.ServiceProvider.GetRequiredService<IDbMigrator>().Migrate(true);
}
else
{
    scope.ServiceProvider.GetRequiredService<IDbMigrator>().Migrate(false);
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<QuizHub>("quizhub");

app.Run();

void ConfigureControllers(IServiceCollection serviceCollection)
{
    // Configure better error messages for invalid input
    serviceCollection.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorResponse = new BadRequestModel
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
}

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    if (Environment.GetEnvironmentVariable("E2E_TESTING").IsNullOrEmpty() ||
        Environment.GetEnvironmentVariable("E2E_TESTING") == "false")
    {
        var connectionString = configuration.GetConnectionString("SQLCONNSTR_DefaultConnection") ??
                               throw new ArgumentException("The connection string is missing");
        serviceCollection.AddInstaller<DALInstaller>(connectionString);
    }
    else
    {
        serviceCollection.AddInstaller<TestDALInstaller>();
    }

    serviceCollection.AddInstaller<BlInstaller>();
}

void ConfigureAuthentication(IServiceCollection serviceCollection, string identityServerUrl)
{
    serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = identityServerUrl;
            options.TokenValidationParameters.ValidateAudience = false;
        });

    serviceCollection.AddAuthorization();
    serviceCollection.AddHttpContextAccessor();
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


void ConfigureCors(IServiceCollection serviceCollection)
{
    serviceCollection.AddCors(options =>
    {
        options.AddDefaultPolicy(o =>
            o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });
}

// Make the implicit Program class public so test projects can access it
public partial class Program
{
}