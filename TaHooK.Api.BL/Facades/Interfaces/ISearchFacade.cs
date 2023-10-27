using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface ISearchFacade : IFacade
{
    Task<IEnumerable<SearchListModel>> GetSearchedAsync(string query, int page, int pageSize);
}