using AutoMapper;
using Microsoft.AspNetCore.Http;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.Facades;

public class QuizFacade : CrudFacadeBase<QuizEntity, QuizListModel, QuizDetailModel, QuizCreateUpdateModel>, IQuizFacade
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QuizFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, IHttpContextAccessor contextAccessor) : base(unitOfWorkFactory, mapper)
    {
        _httpContextAccessor = contextAccessor;
    }

    public override async Task<IdModel> CreateAsync(QuizCreateUpdateModel model)
    {
        var idClaim = _httpContextAccessor.HttpContext!.User.Claims.First(claim => claim.Type.Equals("Id"));
        var entity = Mapper.Map<QuizEntity>(model);

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<QuizEntity>();

        entity.Id = Guid.NewGuid();
        entity.CreatorId = Guid.Parse(idClaim.Value);
        var createdEntity = await repository.InsertAsync(entity);

        await uow.CommitAsync();

        var result = Mapper.Map<IdModel>(createdEntity);
        return result;
    }

    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(QuizEntity.Scores)}",
        $"{nameof(QuizEntity.Template)}.{nameof(QuizTemplateEntity.Questions)}",
        $"{nameof(QuizEntity.Creator)}"
    };
}