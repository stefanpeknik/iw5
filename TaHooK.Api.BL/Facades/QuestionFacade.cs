using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.Facades;

public class QuestionFacade :
    CrudFacadeBase<QuestionEntity, QuestionListModel, QuestionDetailModel, QuestionCreateUpdateModel>, IQuestionFacade
{
    public QuestionFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }

    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(QuestionEntity.Answers)}"
    };
    
    public override async Task<IdModel> UpdateAsync(QuestionCreateUpdateModel model, Guid id)
    {
        var answers = Mapper.Map<List<AnswerEntity>>(model.Answers);
        var answersIds = answers.Select(i => i.Id).ToList();
        var entity = Mapper.Map<QuestionEntity>(model);

        await using var uow = UnitOfWorkFactory.Create();
        // get repository
        var repository = uow.GetRepository<QuestionEntity>();

        if (!await repository.ExistsAsync(id))
        {
            return null;
        }

        entity.Id = id;
        var updatedEntity = await repository.UpdateAsync(entity);
        
        var currentQuestion = await repository.Get().Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
        var currentAnswersIds = currentQuestion!.Answers.Select(i => i.Id).ToList();
        
        // delete answers that are not in the updates question Question.Answers is (List<AnswerEntity>)  
        var answersToDelete = currentAnswersIds.Except(answersIds).ToList();
        
        
        var answersToAddIds = answersIds.Except(currentAnswersIds).ToList();
        var answersToAdd = answers.Where(i => answersToAddIds.Contains(i.Id)).ToList();
        
        var answersToUpdate = answers.Where(i => !answersToAddIds.Contains(i.Id) && !answersToDelete.Contains(i.Id)).ToList();
        
        await DeleteAnswers(answersToDelete, uow);
        await UpdateAnswers(answersToUpdate, uow);
        await AddAnswers(answersToAdd, uow);

        await uow.CommitAsync();
        
        var result = Mapper.Map<IdModel>(updatedEntity);
        
        return result;
    }
    
    private async Task DeleteAnswers(List<Guid> answersToDelete, IUnitOfWork uow)
    {
        var answerRepository = uow.GetRepository<AnswerEntity>();
        foreach (var answerId in answersToDelete)
        {
            await answerRepository.DeleteAsync(answerId);
        }
    }
    
    private async Task UpdateAnswers(List<AnswerEntity> answersToUpdate, IUnitOfWork uow)
    {
        var answerRepository = uow.GetRepository<AnswerEntity>();
        foreach (var answer in answersToUpdate)
        {
            await answerRepository.UpdateAsync(answer);
        }
    }
    
    private async Task AddAnswers(List<AnswerEntity> answersToAdd, IUnitOfWork uow)
    {
        var answerRepository = uow.GetRepository<AnswerEntity>();
        foreach (var answer in answersToAdd)
        {
            await answerRepository.InsertAsync(answer);
        }
    }
    
}