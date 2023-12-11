using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaHooK.Api.App.EndToEndTests.Mock;

namespace TaHooK.Api.App.EndToEndTests;

public class TaHooKApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // this is needed to make the app use special setup for e2e tests only
        Environment.SetEnvironmentVariable("E2E_TESTING", "true");
        builder.ConfigureServices(collection =>
        {
            collection.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>("Test", options => { });
            var controllerAssemblyName = typeof(Program).Assembly.FullName;
            collection.AddMvc().AddApplicationPart(Assembly.Load(controllerAssemblyName));
        });
        return base.CreateHost(builder);
    }
}