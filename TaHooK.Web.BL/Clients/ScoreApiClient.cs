using System.Net.Http;

namespace TaHooK.Web.BL;

public partial class ScoreApiClient
{
    public ScoreApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
