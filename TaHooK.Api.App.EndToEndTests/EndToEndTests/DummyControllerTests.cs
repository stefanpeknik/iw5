namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class DummyControllerTests : EndToEndTestsBase, IAsyncDisposable
{
    private readonly TaHooKApiApplicationFactory application;
    private readonly Lazy<HttpClient> client;

    public DummyControllerTests(TaHooKApiApplicationFactory application, Lazy<HttpClient> client)
    {
        this.application = application;
        this.client = client;
    }

    public new async ValueTask DisposeAsync()
    {
        await application.DisposeAsync();
        await base.DisposeAsync();
    }
}
