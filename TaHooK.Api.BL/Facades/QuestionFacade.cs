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
    
    public override async Task<IdModel> UpdateAsync(Guid id, QuestionCreateUpdateModel model)
    {
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
        
        var currentQuestion = await repository.GetAll().Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
        
        // delete answers that are not in the updates question Question.Answers is (List<AnswerEntity>)   
        var answersToDelete = model.Answers.Except(currentQuestion.Answers.Select(a => a.Id)).ToList();
        
        var answersToAdd = model.Answers.Where(a => currentQuestion.Answers.Any(ca => ca.Id == a.Id)).ToList();
        
        // delete answers

        await uow.CommitAsync();
        
        var result = Mapper.Map<IdModel>(updatedEntity);
        
        return result;
    }
    
}