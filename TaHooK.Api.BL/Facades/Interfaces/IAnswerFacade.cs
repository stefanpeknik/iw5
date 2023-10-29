using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.Answer;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IAnswerFacade : ICrudFacade<AnswerEntity, AnswerListModel, AnswerDetailModel, AnswerCreateUpdateModel>
{
}