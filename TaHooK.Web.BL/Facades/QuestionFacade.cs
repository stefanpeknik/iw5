using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Web.BL.Facades;

public class QuestionFacade : IWebAppFacade
{
    private readonly IQuestionApiClient _apiClient;
    
    public QuestionFacade(IQuestionApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<List<QuestionListModel>> GetAllAsync()
    {
        return (await _apiClient.QuestionsGetAsync()).ToList();
    }
    
    public async Task<QuestionDetailModel> GetByIdAsync(Guid id)
    {
        return await _apiClient.QuestionsGetAsync(id);
    }
    
    public async Task<IdModel> CreateQuestionAsync(QuestionCreateUpdateModel model)
    {
        return await _apiClient.QuestionsPostAsync(model);
    }
    
    public async Task<IdModel> UpdateQuestionAsync(Guid questionId, string questionText, Guid templateId)
    {
        
        var model = new QuestionCreateUpdateModel
        {
            Text = questionText,
            QuizTemplateId = templateId
        };
        
        return await _apiClient.QuestionsPutAsync(questionId, model);
    }
    
}