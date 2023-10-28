using AutoMapper;
using TaHooK.Api.BL.MapperProfiles;

namespace TaHooK.Api.App.EndToEndTests;

public class EndToEndTestsBase : IAsyncDisposable
{
    private readonly TaHooKApiApplicationFactory application;
    protected readonly Lazy<HttpClient> client;
    protected readonly IMapper mapper;

    public EndToEndTestsBase()
    {
        application = new TaHooKApiApplicationFactory();
        client = new Lazy<HttpClient>(application.CreateClient());
        
        var mapperConfig = new MapperConfiguration(cfg => {
            cfg.AddProfile<ScoreMapperProfile>();
            cfg.AddProfile<UserMapperProfile>();
            cfg.AddProfile<QuestionMapperProfile>();
            cfg.AddProfile<AnswerMapperProfile>();
            cfg.AddProfile<QuizMapperProfile>();
        });
        mapper = mapperConfig.CreateMapper();
    }
    
    public async ValueTask DisposeAsync()
    {
        await application.DisposeAsync();
    }
}
