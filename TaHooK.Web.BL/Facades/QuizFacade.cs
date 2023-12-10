using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Web.BL.Facades;

public class QuizFacade : IWebAppFacade
{
    private readonly IQuizApiClient _apiClient;

    public QuizFacade(
        IQuizApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<QuizListModel>> GetAllAsync()
    {
        return (await _apiClient.QuizzesGetAsync()).ToList();
    }
}