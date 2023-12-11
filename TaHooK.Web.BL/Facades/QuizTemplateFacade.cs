using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

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

    public async ValueTask DeleteById(Guid id)
    {
        await _apiClient.TemplatesDeleteAsync(id);
    }
    
    public async Task<IdModel> CreateTemplateAsync(String title)
    {
        var model = new QuizTemplateCreateUpdateModel
        {
            Title = title
        };
        
        return await _apiClient.TemplatesPostAsync(model);
    }
    
}