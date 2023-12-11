using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaHook.Web.App;
using TaHooK.Web.BL.Extensions;
using TaHooK.Web.BL.Installers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl")!;

builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl);
builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler(serviceProvider
        => serviceProvider.GetService<AuthorizationMessageHandler>()
            ?.ConfigureHandler(
                authorizedUrls: new[] { apiBaseUrl },
                scopes: new[] { "tahookapi" }));
builder.Services.AddScoped<HttpClient>(serviceProvider => serviceProvider.GetService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("IdentityServer", options.ProviderOptions);
    var configurationSection = builder.Configuration.GetSection("IdentityServer");
    var authority = configurationSection["Authority"];
    options.ProviderOptions.DefaultScopes.Add("tahookapi");
});

await builder.Build().RunAsync();