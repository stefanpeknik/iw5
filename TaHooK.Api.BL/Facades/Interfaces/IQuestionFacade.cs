using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.Question;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface
    IQuestionFacade : ICrudFacade<QuestionEntity, QuestionListModel, QuestionDetailModel, QuestionCreateUpdateModel>
{
}