using System.Net.Http;

namespace TaHooK.Web.BL;

public partial class SearchApiClient
{
    public SearchApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
