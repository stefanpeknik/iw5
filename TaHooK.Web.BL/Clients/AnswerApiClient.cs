using System.Net.Http;

namespace TaHooK.Web.BL;

public partial class AnswerApiClient
{
    public AnswerApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
