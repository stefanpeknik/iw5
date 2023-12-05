using AutoMapper;
using TaHooK.Api.BL.MapperProfiles;

namespace TaHooK.Api.App.EndToEndTests;

public class EndToEndTestsBase : IAsyncDisposable
{
    private readonly TaHooKApiApplicationFactory _application;
    protected readonly Lazy<HttpClient> Client;
    protected readonly IMapper Mapper;

    public EndToEndTestsBase()
    {
        _application = new TaHooKApiApplicationFactory();
        Client = new Lazy<HttpClient>(_application.CreateClient());

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ScoreMapperProfile>();
            cfg.AddProfile<UserMapperProfile>();
            cfg.AddProfile<QuestionMapperProfile>();
            cfg.AddProfile<AnswerMapperProfile>();
            cfg.AddProfile<QuizMapperProfile>();
            cfg.AddProfile<QuizTemplateMapperProfile>();
        });
        Mapper = mapperConfig.CreateMapper();
    }

    public async ValueTask DisposeAsync()
    {
        await _application.DisposeAsync();
    }
}