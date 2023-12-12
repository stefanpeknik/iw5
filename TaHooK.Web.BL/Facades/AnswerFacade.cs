using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Responses;


namespace TaHooK.Web.BL.Facades;

public class AnswerFacade : IWebAppFacade
{
    private readonly IAnswerApiClient _apiClient;
    
    public AnswerFacade(IAnswerApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<List<AnswerListModel>> GetAllAsync()
    {
        return (await _apiClient.AnswersGetAsync()).ToList();
    }
    
    public async Task<AnswerDetailModel> GetByIdAsync(Guid id)
    {
        return await _apiClient.AnswersGetAsync(id);
    }
    
    public async Task<List<AnswerDetailModel>> GetByQuestionIdAsync(Guid questionId)
    {
        // get all answers, then get detail for each answer, then filter by questionId
        var answers = await _apiClient.AnswersGetAsync();
        var answersDetail = new List<AnswerDetailModel>();
        foreach (var answer in answers)
        {
            answersDetail.Add(await _apiClient.AnswersGetAsync(answer.Id));
        }
        return answersDetail.Where(a => a.QuestionId == questionId).ToList();
    }
    
    public async Task<IdModel> CreateAnswerAsync(AnswerCreateUpdateModel model)
    {
        return await _apiClient.AnswersPostAsync(model);
    }
    
    public async Task<IdModel> UpdateAnswerAsync(AnswerCreateUpdateModel model, Guid answerId)
    {
        return await _apiClient.AnswersPutAsync(answerId, model);
    }
    
    
}