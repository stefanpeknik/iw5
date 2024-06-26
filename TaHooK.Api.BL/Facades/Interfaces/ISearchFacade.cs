﻿using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface ISearchFacade : IFacade
{
    SearchListModel GetSearched(string query, int page, int pageSize);
}