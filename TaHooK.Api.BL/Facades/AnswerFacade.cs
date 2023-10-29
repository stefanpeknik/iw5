using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Answer;

namespace TaHooK.Api.BL.Facades;

public class AnswerFacade : CrudFacadeBase<AnswerEntity, AnswerListModel, AnswerDetailModel, AnswerCreateUpdateModel>,
    IAnswerFacade
{
    public AnswerFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
}