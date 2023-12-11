using Microsoft.Extensions.DependencyInjection;
using TaHooK.Web.BL.Facades;

namespace TaHooK.Web.BL.Installers;

public class WebBLInstaller
{
    public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
    {
        serviceCollection.AddScoped<IAnswerApiClient, AnswerApiClient>();
        serviceCollection.AddScoped<IQuestionApiClient, QuestionApiClient>();
        serviceCollection.AddScoped<IScoreApiClient, ScoreApiClient>();
        serviceCollection.AddScoped<ISearchApiClient, SearchApiClient>();
        serviceCollection.AddScoped<IUserApiClient, UserApiClient>();
        serviceCollection.AddScoped<IQuizApiClient, QuizApiClient>();
        serviceCollection.AddScoped<IQuizTemplateApiClient, QuizTemplateApiClient>();
        
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<WebBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IWebAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
    }
}
