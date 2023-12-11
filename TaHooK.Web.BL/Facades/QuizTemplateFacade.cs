using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Web.BL.Facades;

public class QuizTemplateFacade : IWebAppFacade
{
    private readonly IQuizTemplateApiClient _apiClient;

    public QuizTemplateFacade(IQuizTemplateApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<QuizTemplateListModel>> GetAllAsync()
    {
        return (await _apiClient.TemplatesGetAsync()).ToList();
    }

    public async Task<QuizTemplateDetailModel> GetByIdAsync(Guid id)
    {
        return await _apiClient.TemplatesGetAsync(id);
    }
}