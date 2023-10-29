using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Common;
using TaHooK.Common.Models;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface ICrudFacade<TEntity, TListModel, TDetailModel, TCreateUpdateModel> : IFacade
    where TEntity : class, IEntity
    where TListModel : IWithId
    where TDetailModel : class, IWithId
    where TCreateUpdateModel : class
{
    void IncludeNavigationPathDetails(ref IQueryable<TEntity> query);
    
    Task<IEnumerable<TListModel>> GetAllAsync();
    Task<TDetailModel?> GetByIdAsync(Guid id);
    Task<IdModel> CreateAsync(TCreateUpdateModel model);
    Task<IdModel> UpdateAsync(TCreateUpdateModel model, Guid id);
    Task DeleteAsync(Guid id);
}
