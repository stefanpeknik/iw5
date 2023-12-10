namespace TaHooK.Web.BL;

public partial class QuizTemplateApiClient
{
    public QuizTemplateApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
