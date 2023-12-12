using AutoMapper;
using TaHooK.Common.Extensions;
using TaHooK.IdentityProvider.App;
using TaHooK.IdentityProvider.App.Installers;
using TaHooK.IdentityProvider.BL.Installers;
using TaHooK.IdentityProvider.DAL.Installers;
using Serilog;
using TaHooK.IdentityProvider.DAL.Extensions;
using TaHooK.IdentityProvider.DAL.Migrators;
using TaHooK.IdentityProvider.DAL.Seeds;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();   

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new ArgumentException("The connection string is missing");
    builder.Services.AddInstaller<IdentityProviderDALInstaller>(connectionString);
    builder.Services.AddInstaller<IdentityProviderBLInstaller>();
    builder.Services.AddInstaller<IdentityProviderAppInstaller>();

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder.ConfigureServices();

    var mapper = app.Services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();

    app.ConfigurePipeline();

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
    var seeder = scope.ServiceProvider.GetRequiredService<AppUserSeeds>();
    seeder.SeedAsync().Wait();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}