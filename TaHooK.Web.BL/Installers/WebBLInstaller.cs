using Microsoft.Extensions.DependencyInjection;
using TaHooK.Web.BL.Facades;

namespace TaHooK.Web.BL.Installers;

public class WebBLInstaller
{
    public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
    {
        serviceCollection.AddTransient<IAnswerApiClient, AnswerApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new AnswerApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<IQuestionApiClient, QuestionApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new QuestionApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<IScoreApiClient, ScoreApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new ScoreApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<ISearchApiClient, SearchApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new SearchApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<IUserApiClient, UserApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new UserApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<IQuizApiClient, QuizApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new QuizApiClient(client, apiBaseUrl);
        });

        serviceCollection.AddTransient<IQuizTemplateApiClient, QuizTemplateApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new QuizTemplateApiClient(client, apiBaseUrl);
        });

        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<WebBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IWebAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
    }
    
    public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
    {
        var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
        client.BaseAddress = new Uri(apiBaseUrl);
        return client;
    }
}